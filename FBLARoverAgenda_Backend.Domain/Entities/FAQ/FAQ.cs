using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Domain.Entities.FAQ;

public class FAQ
{
    [Key] public string Id { get; set; }
    [Required] public string Question { get; set; }
    [Required] public string Answer { get; set; }
}