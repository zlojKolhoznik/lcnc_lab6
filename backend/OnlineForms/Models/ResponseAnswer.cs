using System.ComponentModel.DataAnnotations;

namespace OnlineForms.Models;

public enum OptionChoice
{
    A = 1,
    B = 2,
    C = 3,
    D = 4
}

public class ResponseAnswer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ResponseId { get; set; }
    public Response? Response { get; set; }

    [Required]
    public Guid QuestionId { get; set; }
    public Question? Question { get; set; }

    [Required]
    public OptionChoice SelectedOption { get; set; }
}
