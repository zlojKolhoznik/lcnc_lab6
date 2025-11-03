using System.ComponentModel.DataAnnotations;
using OnlineForms.Models;

namespace OnlineForms.Dtos;

public class CreateSurveyDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    [MinLength(1)]
    public List<CreateQuestionDto> Questions { get; set; } = new();
}

public class CreateQuestionDto
{
    [Required, MaxLength(500)]
    public string Text { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionA { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionB { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionC { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionD { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class SurveyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class SubmitSurveyResponseDto
{
    [MaxLength(200)]
    public string? ParticipantId { get; set; }
    [MinLength(1)]
    public List<SubmitAnswerDto> Answers { get; set; } = new();
}

public class SubmitAnswerDto
{
    [Required]
    public Guid QuestionId { get; set; }
    [Required]
    public OptionChoice SelectedOption { get; set; }
}

public class ResponseDto
{
    public Guid Id { get; set; }
    public string? ParticipantId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}

public class AnswerDto
{
    public Guid QuestionId { get; set; }
    public OptionChoice SelectedOption { get; set; }
}

public class SurveyStatsDto
{
    public Guid SurveyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<QuestionStatsDto> Questions { get; set; } = new();
}

public class QuestionStatsDto
{
    public Guid QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int CountA { get; set; }
    public int CountB { get; set; }
    public int CountC { get; set; }
    public int CountD { get; set; }
    public int TotalResponses { get; set; }
}
