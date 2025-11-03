import { Routes } from '@angular/router';
import { SurveyListComponent } from './survey-list.component';
import { SurveyCreateComponent } from './survey-create.component';
import { SurveyTakeComponent } from './survey-take.component';
import { SurveyStatsComponent } from './survey-stats.component';

export const routes: Routes = [
	{ path: '', component: SurveyListComponent },
	{ path: 'create', component: SurveyCreateComponent },
	{ path: 'survey/:id', component: SurveyTakeComponent },
	{ path: 'survey/:id/stats', component: SurveyStatsComponent },
	{ path: '**', redirectTo: '' }
];
