# OnlineForms Backend (ASP.NET Core + EF Core)

An online survey service that lets users create surveys with multiple questions (ABCD options), collect participant responses, and view per-question stats.

## What’s included

- .NET 8 Web API with Swagger (Development only)
- EF Core with SQLite database (`surveys.db` in the app folder)
- Entities: Survey, Question, Response, ResponseAnswer
- Endpoints to create/get/list surveys, submit responses, list responses, and view stats
- HTTP sample file: `OnlineForms/OnlineForms.http`

## Quick start

1. Restore & build

```cmd
dotnet build "OnlineForms/OnlineForms.csproj"
```

2. Create database (first time)

```cmd
dotnet ef migrations add InitialCreate --project "OnlineForms/OnlineForms.csproj" --startup-project "OnlineForms/OnlineForms.csproj"
dotnet ef database update --project "OnlineForms/OnlineForms.csproj" --startup-project "OnlineForms/OnlineForms.csproj"
```

3. Run the API

```cmd
dotnet run --project "OnlineForms/OnlineForms.csproj"
```

- Swagger UI: http://localhost:5112/swagger (Development)
- Health check: `GET /weatherforecast` (demo)

## Key endpoints

- POST `/api/surveys` – Create a survey with questions and ABCD options
- GET `/api/surveys` – List surveys (with questions)
- GET `/api/surveys/{id}` – Get survey by id (with questions)
- POST `/api/surveys/{id}/responses` – Submit responses for all questions
- GET `/api/surveys/{id}/responses` – List responses for a survey
- GET `/api/surveys/{id}/stats` – Per-question counts for A/B/C/D

## Payload examples

Create a survey:

```json
{
  "title": "Course Feedback",
  "description": "Tell us about your experience",
  "questions": [
    { "text": "How do you rate the course overall?", "optionA": "Excellent", "optionB": "Good", "optionC": "Fair", "optionD": "Poor", "order": 1 },
    { "text": "Difficulty level?", "optionA": "Too easy", "optionB": "Easy", "optionC": "Just right", "optionD": "Hard", "order": 2 }
  ]
}
```

Submit responses:

```json
{
  "participantId": "student-123",
  "answers": [
    { "questionId": "<Q1_GUID>", "selectedOption": "A" },
    { "questionId": "<Q2_GUID>", "selectedOption": "C" }
  ]
}
```

Note: Enums are serialized as strings, so `selectedOption` can be "A"|"B"|"C"|"D".

## Implementation notes

- SQLite is used for easy local setup; swap to SQL Server by replacing `UseSqlite` in `Program.cs` and updating the connection string.
- Deleting a survey cascades to its questions, responses, and answers.
- Basic validation enforces answering each question exactly once.
- Indexes exist for common lookups (responses by survey/participant, question ordering).

## Next steps / ideas

- Pagination and filtering for responses
- Optional uniqueness per participant (one submission per survey)
- Authentication/authorization (e.g., for survey management)
- Soft delete and audit fields
- Export stats to CSV
