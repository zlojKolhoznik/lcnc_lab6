using System.ComponentModel.DataAnnotations;

namespace OnlineForms.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid SurveyId { get; set; }
    public Survey? Survey { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string OptionA { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionB { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionC { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string OptionD { get; set; } = string.Empty;

    public int Order { get; set; } = 0;

    public ICollection<ResponseAnswer> Answers { get; set; } = new List<ResponseAnswer>();
}
