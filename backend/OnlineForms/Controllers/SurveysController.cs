using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineForms.Data;
using OnlineForms.Dtos;
using OnlineForms.Models;
using System.Text.Json.Serialization;

namespace OnlineForms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController : ControllerBase
{
    private readonly OnlineFormsDbContext _db;

    public SurveysController(OnlineFormsDbContext db)
    {
        _db = db;
    }

    // POST api/surveys
    [HttpPost]
    public async Task<ActionResult<SurveyDto>> CreateSurvey([FromBody] CreateSurveyDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var survey = new Survey
        {
            Title = dto.Title,
            Description = dto.Description,
            Questions = dto.Questions
                .OrderBy(q => q.Order)
                .Select(q => new Question
                {
                    Text = q.Text,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD,
                    Order = q.Order
                })
                .ToList()
        };

        _db.Surveys.Add(survey);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, ToSurveyDto(survey));
    }

    // GET api/surveys
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SurveyDto>>> ListSurveys()
    {
        var surveys = await _db.Surveys
            .Include(s => s.Questions.OrderBy(q => q.Order))
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return Ok(surveys.Select(ToSurveyDto));
    }

    // GET api/surveys/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SurveyDto>> GetSurvey([FromRoute] Guid id)
    {
        var survey = await _db.Surveys
            .Include(s => s.Questions.OrderBy(q => q.Order))
            .FirstOrDefaultAsync(s => s.Id == id);

        if (survey == null) return NotFound();
        return Ok(ToSurveyDto(survey));
    }

    // POST api/surveys/{id}/responses
    [HttpPost("{id:guid}/responses")]
    public async Task<ActionResult<ResponseDto>> SubmitResponse([FromRoute] Guid id, [FromBody] SubmitSurveyResponseDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var survey = await _db.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.Id == id);
        if (survey == null) return NotFound();

        // Validate that all answers belong to this survey and are valid options
        var questionIds = survey.Questions.Select(q => q.Id).ToHashSet();
        foreach (var answer in dto.Answers)
        {
            if (!questionIds.Contains(answer.QuestionId))
                return BadRequest($"Question {answer.QuestionId} does not belong to this survey.");
            if (!Enum.IsDefined(typeof(OptionChoice), answer.SelectedOption))
                return BadRequest($"Invalid option for question {answer.QuestionId}.");
        }

        // Optional: ensure all questions are answered exactly once
        var uniqueAnswered = dto.Answers.Select(a => a.QuestionId).Distinct().Count();
        if (uniqueAnswered != survey.Questions.Count)
            return BadRequest("You must answer all questions exactly once.");

        var response = new Response
        {
            SurveyId = id,
            ParticipantId = string.IsNullOrWhiteSpace(dto.ParticipantId) ? null : dto.ParticipantId!.Trim(),
            Answers = dto.Answers.Select(a => new ResponseAnswer
            {
                QuestionId = a.QuestionId,
                SelectedOption = a.SelectedOption
            }).ToList()
        };

        _db.Responses.Add(response);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResponses), new { id }, ToResponseDto(response));
    }

    // GET api/surveys/{id}/responses
    [HttpGet("{id:guid}/responses")]
    public async Task<ActionResult<IEnumerable<ResponseDto>>> GetResponses([FromRoute] Guid id)
    {
        var exists = await _db.Surveys.AnyAsync(s => s.Id == id);
        if (!exists) return NotFound();

        var responses = await _db.Responses
            .Where(r => r.SurveyId == id)
            .Include(r => r.Answers)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();

        return Ok(responses.Select(ToResponseDto));
    }

    // GET api/surveys/{id}/stats
    [HttpGet("{id:guid}/stats")]
    public async Task<ActionResult<SurveyStatsDto>> GetStats([FromRoute] Guid id)
    {
        var survey = await _db.Surveys
            .Include(s => s.Questions.OrderBy(q => q.Order))
            .FirstOrDefaultAsync(s => s.Id == id);
        if (survey == null) return NotFound();

        var answers = await _db.ResponseAnswers
            .Where(a => a.Response!.SurveyId == id)
            .Include(a => a.Question)
            .ToListAsync();

        var byQuestion = answers.GroupBy(a => a.QuestionId).ToDictionary(g => g.Key, g => g.ToList());

        var stats = new SurveyStatsDto
        {
            SurveyId = survey.Id,
            Title = survey.Title,
            Questions = survey.Questions.Select(q =>
            {
                byQuestion.TryGetValue(q.Id, out var list);
                list ??= new List<ResponseAnswer>();
                return new QuestionStatsDto
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    CountA = list.Count(a => a.SelectedOption == OptionChoice.A),
                    CountB = list.Count(a => a.SelectedOption == OptionChoice.B),
                    CountC = list.Count(a => a.SelectedOption == OptionChoice.C),
                    CountD = list.Count(a => a.SelectedOption == OptionChoice.D),
                    TotalResponses = list.Count
                };
            }).ToList()
        };

        return Ok(stats);
    }

    private static SurveyDto ToSurveyDto(Survey s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Description = s.Description,
        CreatedAt = s.CreatedAt,
        Questions = s.Questions
            .OrderBy(q => q.Order)
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                Order = q.Order
            }).ToList()
    };

    private static ResponseDto ToResponseDto(Response r) => new()
    {
        Id = r.Id,
        ParticipantId = r.ParticipantId,
        SubmittedAt = r.SubmittedAt,
        Answers = r.Answers.Select(a => new AnswerDto
        {
            QuestionId = a.QuestionId,
            SelectedOption = a.SelectedOption
        }).ToList()
    };
}
