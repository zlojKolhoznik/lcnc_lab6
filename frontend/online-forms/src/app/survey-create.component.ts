import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormArray, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { SurveyService } from './services/survey.service';

@Component({
  selector: 'app-survey-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './survey-create.component.html',
  styleUrls: ['./survey-create.component.scss']
})
export class SurveyCreateComponent {
  form!: FormGroup;

  get questions() {
    return this.form.get('questions') as FormArray;
  }

  constructor(private fb: FormBuilder, private svc: SurveyService, private router: Router) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: [''],
      questions: this.fb.array([])
    });
    // start with one question
    this.addQuestion();
  }

  addQuestion() {
    this.questions.push(
      this.fb.group({
        text: ['', [Validators.required, Validators.maxLength(500)]],
        optionA: ['', [Validators.required, Validators.maxLength(200)]],
        optionB: ['', [Validators.required, Validators.maxLength(200)]],
        optionC: ['', [Validators.required, Validators.maxLength(200)]],
        optionD: ['', [Validators.required, Validators.maxLength(200)]],
        order: [this.questions.length + 1]
      })
    );
  }

  removeQuestion(idx: number) {
    this.questions.removeAt(idx);
  }

  submit() {
    if (this.form.invalid) return;
    const value = this.form.value;
    // transform to backend CreateSurveyDto shape: title, description, questions
    const payload = {
      title: value.title,
      description: value.description,
      questions: value.questions as any
    };
    this.svc.createSurvey(payload).subscribe({
      next: () => this.router.navigate(['/']),
      error: (err) => console.error(err)
    });
  }
}
