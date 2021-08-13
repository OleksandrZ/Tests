import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Test } from "../core/models/test.model";
import { Question } from "./../core/models/question.model";
import { Answer } from "./../core/models/answer.model";
import { TestsService } from "./../core/services/tests.service";
import { Router } from "@angular/router";
import { first } from 'rxjs/operators';

@Component({
  selector: "app-create-test",
  templateUrl: "./create-test.component.html",
  styleUrls: ["./create-test.component.css"],
})
export class CreateTestComponent implements OnInit {
  createTestForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private testService: TestsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createTestForm = this.fb.group({
      title: ["", [Validators.required, Validators.minLength(4)]],
      description: ["", [Validators.required, Validators.minLength(20)]],
      questions: this.fb.array([]),
    });

    this.addQuestion();
  }

  get questions(): FormArray {
    return this.createTestForm.get("questions") as FormArray;
  }

  newQuestion() {
    return this.fb.group({
      questionTitle: ["", [Validators.required, Validators.minLength(5)]],
      answers: this.fb.array([]),
    });
  }

  addQuestion() {
    this.questions.push(this.newQuestion());
    this.addQuestionAnswer(this.questions.length - 1);
    this.addQuestionAnswer(this.questions.length - 1);
  }

  removeQuestion(questionIndex: number) {
    this.questions.removeAt(questionIndex);
  }

  questionAnswers(questionIndex: number) {
    return this.questions.at(questionIndex).get("answers") as FormArray;
  }

  newAnswer() {
    return this.fb.group({
      answerDescription: ["", [Validators.required]],
    });
  }

  addQuestionAnswer(questionIndex: number) {
    this.questionAnswers(questionIndex).push(this.newAnswer());
  }

  removeQuestionAnswer(questionIndex: number, answerIndex: number) {
    this.questionAnswers(questionIndex).removeAt(answerIndex);
  }

  onSubmit() {
    let form = this.createTestForm.value;
    let test: Test = new Test();
    test.description = this.createTestForm.value.description;
    test.title = this.createTestForm.value.title;
    test.questions = [];

    for (let question of this.createTestForm.value.questions) {
      let questionObject: Question = new Question();
      let title = question.questionTitle;
      questionObject.title = title;
      questionObject.answers = [];
      for (let index = 0; index < question.answers.length; index++) {
        let answer: Answer = new Answer();
        answer.description = question.answers[index].answerDescription;

        if (index === 0) answer.isAnswer = true;
        else answer.isAnswer = false;

        questionObject.answers.push(answer);
      }

      test.questions.push(questionObject);
    }
    test.minCorrectAnswers = 1;

    // const formData: FormData = new FormData();
    // formData.append("test", JSON.stringify(test));
    // console.log(formData.get("test"));
    console.log(JSON.stringify(test));

    this.testService.createTest(test).pipe(first()).subscribe({
      next: () => {
        this.router.navigate(["/"]);
      },
      error: (error) => {
        console.log(error);

      }
    });


  }
}
