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
            var user = await context.Users
               .Include(x =>x.AvailableTests)
               .ThenInclude(x => x.Test)
               .ThenInclude(x => x.Questions)
               .ThenInclude(x => x.Answers)
               .FirstOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername());


            if (user == null)
                return Unauthorized();

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
            var test = await context.Tests
                .Include(x => x.Author)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(string id)
        {
            var test = await context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            context.Tests.Remove(test);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Tests/checktest
        [HttpPost("checktest")]
        public async Task<ActionResult<TestResultDto>> PostCheckTest([FromBody] ICollection<UserAnswerDto> userAnswersDto)
        {
            if (TestExists(userAnswersDto.FirstOrDefault()?.TestId))
            {
                var test = await context.Tests
                    .Include(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                    .FirstOrDefaultAsync(x => x.Id == userAnswersDto.FirstOrDefault().TestId);

                TestResultDto testResult = new()
                {
                    MinCorrectAnswers = test.MinCorrectAnswers,
                    MaxCorrectAnswers = test.Questions.Count
                };

                foreach (var answer in userAnswersDto)
                {
                    var question = test.Questions.FirstOrDefault(x => x.Id == answer.QuestionId);
                    if (question?.Answers.FirstOrDefault(x => x.Id == answer.AnswerId)?.IsAnswer == true)
                    {
                        testResult.TotalCorrectAnswers++;
                    }
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
