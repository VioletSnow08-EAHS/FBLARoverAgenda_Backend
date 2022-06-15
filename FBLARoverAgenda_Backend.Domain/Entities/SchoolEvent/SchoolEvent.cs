using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Domain.Entities.SchoolEvent;

public class SchoolEvent
{
    [Key] public int Id { get; set; }
    [Microsoft.Build.Framework.Required] public string Name { get; set; }
    public string Description { get; set; }
    [Microsoft.Build.Framework.Required] public DateTime StartTime { get; set; }
    [Microsoft.Build.Framework.Required] public DateTime EndTime { get; set; }

}