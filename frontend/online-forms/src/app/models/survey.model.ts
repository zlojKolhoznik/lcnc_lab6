export type OptionChoice = 'A' | 'B' | 'C' | 'D';

export interface QuestionDto {
  id: string;
  text?: string | null;
  optionA?: string | null;
  optionB?: string | null;
  optionC?: string | null;
  optionD?: string | null;
  order?: number;
}

export interface SurveyDto {
  id: string;
  title?: string | null;
  description?: string | null;
  createdAt?: string;
  questions?: QuestionDto[] | null;
}

export interface SubmitAnswerDto {
  questionId: string;
  selectedOption: OptionChoice;
}

export interface SubmitSurveyResponseDto {
  participantId?: string | null;
  answers?: SubmitAnswerDto[] | null;
}

export interface QuestionStatsDto {
  questionId: string;
  text?: string | null;
  countA?: number;
  countB?: number;
  countC?: number;
  countD?: number;
  totalResponses?: number;
}

export interface SurveyStatsDto {
  surveyId: string;
  title?: string | null;
  questions?: QuestionStatsDto[] | null;
}
