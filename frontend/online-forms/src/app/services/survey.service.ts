import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  SurveyDto,
  SubmitSurveyResponseDto,
  SurveyStatsDto
} from '../models/survey.model';

@Injectable({ providedIn: 'root' })
export class SurveyService {
  private base = 'http://localhost:5112/api/Surveys';

  constructor(private http: HttpClient) {}

  getSurveys(): Observable<SurveyDto[]> {
    return this.http.get<SurveyDto[]>(this.base);
  }

  createSurvey(payload: Partial<SurveyDto>): Observable<SurveyDto> {
    return this.http.post<SurveyDto>(this.base, payload);
  }

  getSurvey(id: string): Observable<SurveyDto> {
    return this.http.get<SurveyDto>(`${this.base}/${id}`);
  }

  submitResponse(id: string, payload: SubmitSurveyResponseDto) {
    return this.http.post(`${this.base}/${id}/responses`, payload);
  }

  getResponses(id: string) {
    return this.http.get(`${this.base}/${id}/responses`);
  }

  getStats(id: string): Observable<SurveyStatsDto> {
    return this.http.get<SurveyStatsDto>(`${this.base}/${id}/stats`);
  }
}
