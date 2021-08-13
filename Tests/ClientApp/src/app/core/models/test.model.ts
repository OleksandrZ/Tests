import { Question } from "./question.model";

export class Test{
  id: string;
  title: string;
  description: string;
  questions: Question[]
  minCorrectAnswers: number;
}
