using Microsoft.Build.Framework;

namespace FBLARoverAgenda_Backend.Domain.Entities.SchoolEvent;

public class SchoolEvent
{
    [Required] public int Id { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }

}