import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "./../../../environments/environment";
import { Test } from "./../models/test.model";
import { Observable } from "rxjs";
import { TestResult } from "./../models/testResult.model";
import { UserAnswer } from "./../models/userAnswer.model";

@Injectable({
  providedIn: "root",
})
export class TestsService {
  constructor(private http: HttpClient) {}

  getTestById(id: string): Observable<Test> {
    return this.http.get<Test>(`${environment.apiUrl}tests/${id}`);
  }

  getTests(): Observable<Test[]> {
    return this.http.get<Test[]>(`${environment.apiUrl}tests`);
  }

  getTestResult(userAnswers: UserAnswer[]) {
    return this.http.post<any>(
      `${environment.apiUrl}tests/checktest`,
      userAnswers,
      { withCredentials: true }
    );
  }

  createTest(test: Test) {
    return this.http.post<any>(`${environment.apiUrl}tests`, test, {
      withCredentials: true,
    });
  }
}
