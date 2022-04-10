using System.ComponentModel.DataAnnotations;
using FBLARoverAgenda_Backend.Domain.Entities.Identity;

namespace FBLARoverAgenda_Backend.Domain.Entities.Calendaric;

public class Event
{
    [Key] public string Id { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }
    [Required] public string Subject { get; set; }
    [Required] public string Color { get; set; }
    [Required] public string RecurrenceRule { get; set; }
    [Required] public ApplicationUser User { get; set; }
    [Required] public string UserId { get; set; }
}