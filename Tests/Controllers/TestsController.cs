using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tests.Models;
using Tests.Models.DTOs;
using Tests.Utils.Interfaces;

namespace Tests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestsController : ControllerBase
    {
        private readonly TestsDbContext context;
        private readonly ICurrentUserAccessor currentUserAccessor;
        private readonly UserManager<TestsUser> userManager;

        public TestsController(TestsDbContext context, ICurrentUserAccessor currentUserAccessor, UserManager<TestsUser> userManager)
        {
            this.context = context;
            this.currentUserAccessor = currentUserAccessor;
            this.userManager = userManager;
        }

        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDto>>> GetTests()
        {
            //get available to user tests
            var user = await context.Users
               .Include(x => x.AvailableTests)
               .ThenInclude(x => x.Test)
               .ThenInclude(x => x.Questions)
               .ThenInclude(x => x.Answers)
               .FirstOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername());


            if (user == null)
                return Unauthorized();

            //populate list of testdtos
            List<TestDto> testDtos = new List<TestDto>();
            TestDto testDto;
            foreach (var availableTests in user.AvailableTests)
            {
                testDto = new TestDto()
                {
                    Description = availableTests.Test.Description,
                    Id = availableTests.Test.Id,
                    MinCorrectAnswers = availableTests.Test.MinCorrectAnswers,
                    Title = availableTests.Test.Title
                };
                testDto.Questions = new List<QuestionDto>();
                QuestionDto questionDto;
                foreach (var question in availableTests.Test.Questions)
                {
                    questionDto = new QuestionDto()
                    {
                        Id = question.Id,
                        Title = question.Title
                    };
                    questionDto.Answers = new List<AnswerDto>();
                    AnswerDto answerDto;
                    foreach (var answer in question.Answers)
                    {
                        answerDto = new AnswerDto()
                        {
                            Description = answer.Description,
                            Id = answer.Id,
                            IsAnswer = answer.IsAnswer
                        };
                        questionDto.Answers.Add(answerDto);
                    }

                    testDto.Questions.Add(questionDto);

                }
                testDtos.Add(testDto);
            }


            return testDtos;
        }

        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDto>> GetTest(string id)
        {
            var user = await context.Users
              .Include(x => x.AvailableTests)
              .ThenInclude(x => x.Test)
              .ThenInclude(x => x.Questions)
              .ThenInclude(x => x.Answers)
              .FirstOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername());
            var test = user.AvailableTests.FirstOrDefault(x => x.TestId == id).Test;

            if (test == null)
            {
                return NotFound();
            }

            TestDto testDto = new TestDto()
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description,
                MinCorrectAnswers = test.MinCorrectAnswers
            };

            testDto.Questions = new List<QuestionDto>();
            QuestionDto questionDto;
            foreach (var question in test.Questions)
            {
                questionDto = new QuestionDto()
                {
                    Id = question.Id,
                    Title = question.Title
                };
                questionDto.Answers = new List<AnswerDto>();
                AnswerDto answerDto;
                foreach (var answer in question.Answers)
                {
                    answerDto = new AnswerDto()
                    {
                        Description = answer.Description,
                        Id = answer.Id,
                        IsAnswer = answer.IsAnswer
                    };
                    questionDto.Answers.Add(answerDto);
                }

                testDto.Questions.Add(questionDto);

            }

            return testDto;
        }

        // POST: api/Tests
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest([FromBody] Test test)
        {
            var isExist = context.Tests.FirstOrDefault(x => x.Title == test.Title);
            if (isExist != null)
            {
                return BadRequest();
            }
            test.Id = Guid.NewGuid().ToString();

            test.Author = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());

            foreach (var question in test.Questions)
            {
                question.Id = Guid.NewGuid().ToString();
                foreach (var answer in question.Answers)
                {
                    answer.Id = Guid.NewGuid().ToString();
                }
            }
            context.Tests.Add(test);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TestExists(test.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTest", new { id = test.Id }, test);
        }

        // DELETE: api/Tests/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTest(string id)
        //{
        //    var test = await context.Tests.FindAsync(id);
        //    if (test == null)
        //    {
        //        return NotFound();
        //    }

        //    context.Tests.Remove(test);
        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}

        // POST: api/Tests/checktest
        [HttpPost("checktest")]
        public async Task<ActionResult<TestResultDto>> PostCheckTest([FromBody] ICollection<UserAnswerDto> userAnswersDto)
        {
            if (TestExists(userAnswersDto.FirstOrDefault()?.TestId))
            {
                var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());
                var test = await context.Tests
                    .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                    .FirstOrDefaultAsync(x => x.Id == userAnswersDto.FirstOrDefault().TestId);

                List<UserAnswer> userAnswers = new List<UserAnswer>();
                PassedTests passedTestResult = new PassedTests()
                {
                    Test = test,
                    Id = Guid.NewGuid().ToString()
                };

                TestResultDto testResult = new()
                {
                    MinCorrectAnswers = test.MinCorrectAnswers,
                    MaxCorrectAnswers = test.Questions.Count
                };

                foreach (var answer in userAnswersDto)
                {
                    var question = test.Questions.FirstOrDefault(x => x.Id == answer.QuestionId);
                    var Answer = question?.Answers.FirstOrDefault(x => x.Id == answer.AnswerId);
                    if (Answer != null)
                    {
                        if(Answer.IsAnswer == true)
                        {
                            testResult.TotalCorrectAnswers++;
                        }
                        userAnswers.Add(new UserAnswer()
                        {
                            Answer = Answer,
                            Question = question,
                            Id = Guid.NewGuid().ToString()
                        });
                    }
                }

                passedTestResult.Answers = userAnswers;
                passedTestResult.TotalScore = testResult.TotalCorrectAnswers;

                context.PassedTests.Add(passedTestResult);

                //If user passed test remove test from available tests
                if (testResult.TotalCorrectAnswers >= testResult.MinCorrectAnswers)
                {
                    context.AvailableTests
                        .Remove(await context
                                    .AvailableTests
                                    .Include(x => x.Test)
                                    .FirstOrDefaultAsync(x => x.TestId == test.Id));
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }

                return Ok(testResult);
            }

            return BadRequest();
        }

        private bool TestExists(string id)
        {
            return context.Tests.Any(e => e.Id == id);
        }
    }
}
