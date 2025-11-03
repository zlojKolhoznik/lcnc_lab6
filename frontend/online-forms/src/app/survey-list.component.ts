import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { SurveyService } from './services/survey.service';
import { SurveyDto } from './models/survey.model';

@Component({
  selector: 'app-survey-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.scss']
})
export class SurveyListComponent implements OnInit {
  surveys: SurveyDto[] = [];
  loading = false;

  constructor(private svc: SurveyService, private router: Router) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.loading = true;
    this.svc.getSurveys().subscribe({
      next: (s) => (this.surveys = s || []),
      complete: () => (this.loading = false),
      error: () => (this.loading = false)
    });
  }

  goCreate() {
    this.router.navigate(['/create']);
  }

  take(id: string) {
    this.router.navigate(['/survey', id]);
  }

  stats(id: string) {
    this.router.navigate(['/survey', id, 'stats']);
  }
}
