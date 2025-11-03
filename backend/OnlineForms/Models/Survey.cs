using System.ComponentModel.DataAnnotations;

namespace OnlineForms.Models;

public class Survey
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Question> Questions { get; set; } = new List<Question>();

    public ICollection<Response> Responses { get; set; } = new List<Response>();
}
