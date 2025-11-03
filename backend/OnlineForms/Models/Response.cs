using System.ComponentModel.DataAnnotations;

namespace OnlineForms.Models;

public class Response
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid SurveyId { get; set; }
    public Survey? Survey { get; set; }

    [MaxLength(200)]
    public string? ParticipantId { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ResponseAnswer> Answers { get; set; } = new List<ResponseAnswer>();
}
