using System.ComponentModel.DataAnnotations;
using FBLARoverAgenda_Backend.Domain.Entities.Identity;

namespace FBLARoverAgenda_Backend.Domain.Entities.Class;

public class Class
{
    [Key] public string Id { get; set; }
    [Required] public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }

    [Required] public string Color { get; set; } = "#0000FF";
    [Required] public Teacher.Teacher Teacher { get; set; }
    [Required] public string TeacherId { get; set; }
    [Required] public string RecurrenceRule { get; set; }
    [Required] public ApplicationUser User { get; set; }
    [Required] public string UserId { get; set; }
}