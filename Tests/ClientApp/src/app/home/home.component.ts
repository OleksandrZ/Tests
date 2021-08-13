import { Component } from '@angular/core';
import { Test } from '../core/models/test.model';
import { TestsService } from '../core/services/tests.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ["./home.component.css"],
})
export class HomeComponent {
  tests: Test[];
  constructor(private testsService: TestsService) {

  }

  ngOnInit(): void {
    this.getTests();
  }

  getTests(): void{
    this.testsService.getTests().subscribe(tests=>{
      this.tests = tests;
    });
  }
}
