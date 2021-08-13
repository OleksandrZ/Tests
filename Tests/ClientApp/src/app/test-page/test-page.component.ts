import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Test } from "../core/models/test.model";
import { TestsService } from "./../core/services/tests.service";
import { Question } from "./../core/models/question.model";
import { UserAnswer } from "../core/models/userAnswer.model";
import { BehaviorSubject, Observable } from "rxjs";
import { faThumbsDown } from "@fortawesome/free-regular-svg-icons";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TestResult } from './../core/models/testResult.model';

@Component({
  selector: "app-test-page",
  templateUrl: "./test-page.component.html",
  styleUrls: ["./test-page.component.css"],
})
export class TestPageComponent implements OnInit {
  test: Test;
  private testId: string;
  totalQuestions: number;
  questions: Question[];
  indexCurrentQuestion: number;
  answeredQuestions: UserAnswer[];
  currentAnswer: UserAnswer;
  isStarted: boolean = false;
  isTestResult: boolean = false;
  startTestForm: FormGroup;
  testResult: TestResult;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private testsService: TestsService
  ) {}

  ngOnInit(): void {
    this.testId = this.route.snapshot.paramMap.get("id");
    this.indexCurrentQuestion = 0;
    this.answeredQuestions = [];

    this.currentAnswer = new UserAnswer();

    this.startTestForm = this.fb.group({
      start: ["", Validators.requiredTrue],
    });

    this.getTest(this.testId);
  }

  getTest(id: string) {
    this.testsService.getTestById(id).subscribe((test) => {
      this.test = test;
      this.totalQuestions = this.test.questions.length;
      this.questions = test.questions;

      this.currentAnswer.testId = this.test.id;
      this.currentAnswer.questionId = this.questions[0].id;
      this.currentAnswer.answerId = "";
    });
  }

  getQuestion() {
    this.questions.pop();
  }

  onAnswerChange(answerId) {
    this.currentAnswer.answerId = answerId;
  }

  next() {
    //check if user clicked prev button and currently on answered question
    if (this.indexCurrentQuestion < this.answeredQuestions.length) {
      console.log(this.answeredQuestions[this.indexCurrentQuestion].answerId);
      console.log(this.currentAnswer.answerId);

      if (
        this.answeredQuestions[this.indexCurrentQuestion].answerId !==
        this.currentAnswer.answerId
      ) {
        console.log("Different answer");

        this.answeredQuestions[this.indexCurrentQuestion] = this.currentAnswer;
      }
    } else {
      this.answeredQuestions.push(this.currentAnswer);

      this.currentAnswer = new UserAnswer();
      this.currentAnswer.testId = this.test.id;
      this.currentAnswer.answerId = "";
    }

    //next question if there's more
    if (this.indexCurrentQuestion < this.questions.length - 1) {
      this.indexCurrentQuestion++;

      //check if next question was answered
      if (this.indexCurrentQuestion < this.answeredQuestions.length) {
        this.currentAnswer = this.answeredQuestions[this.indexCurrentQuestion];
      }
      //else init question id
      else {
        this.currentAnswer.questionId =
          this.questions[this.indexCurrentQuestion].id;
      }
    }
    //if no more question, when button submit clicked
    else {
      this.testsService.getTestResult(this.answeredQuestions).subscribe({
        next: (testResult: TestResult) => {
          this.testResult = testResult;
          this.isTestResult = true;
        }
      });
    }

    console.log(this.answeredQuestions);
  }
  prev() {
    if (this.indexCurrentQuestion > 0) {
      this.indexCurrentQuestion--;
      this.currentAnswer = this.answeredQuestions[this.indexCurrentQuestion];
    }
  }

  start(event) {
    event.preventDefault();
    if (!this.startTestForm.valid) {
      return;
    }
    this.isStarted = true;
  }
}
