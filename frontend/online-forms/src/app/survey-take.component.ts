import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { SurveyService } from './services/survey.service';
import { SurveyDto, SubmitAnswerDto, SubmitSurveyResponseDto } from './models/survey.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-survey-take',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './survey-take.component.html',
  styleUrls: ['./survey-take.component.scss']
})
export class SurveyTakeComponent implements OnInit {
  survey?: SurveyDto;
  answers: Record<string, string> = {};
  loading = false;

  constructor(private route: ActivatedRoute, private svc: SurveyService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    this.loading = true;
    this.svc.getSurvey(id).subscribe({
      next: (s) => (this.survey = s),
      complete: () => (this.loading = false),
      error: () => (this.loading = false)
    });
  }

  submit() {
    if (!this.survey) return;
    const payload: SubmitSurveyResponseDto = {
      participantId: undefined,
      answers: Object.keys(this.answers).map((questionId) => ({
        questionId,
        selectedOption: this.answers[questionId] as any
      }))
    };
    this.svc.submitResponse(this.survey.id, payload).subscribe({
      next: () => alert('Thank you! Your answers were submitted.'),
      error: (e) => alert('Failed to submit: ' + e)
    });
  }
}
