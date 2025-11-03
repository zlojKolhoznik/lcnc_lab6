import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { SurveyService } from './services/survey.service';
import { SurveyStatsDto } from './models/survey.model';

@Component({
  selector: 'app-survey-stats',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './survey-stats.component.html',
  styleUrls: ['./survey-stats.component.scss']
})
export class SurveyStatsComponent implements OnInit {
  stats?: SurveyStatsDto;
  loading = false;

  constructor(private route: ActivatedRoute, private svc: SurveyService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    this.loading = true;
    this.svc.getStats(id).subscribe({
      next: (s) => (this.stats = s),
      complete: () => (this.loading = false),
      error: () => (this.loading = false)
    });
  }
}
