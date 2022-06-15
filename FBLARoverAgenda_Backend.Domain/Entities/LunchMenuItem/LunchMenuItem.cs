using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Domain.Entities.LunchMenuItem;

public class LunchMenuItem
{
    [Key]
    public string Id { get; set; }
    [Microsoft.Build.Framework.Required]
    public string Name { get; set; }
    [Microsoft.Build.Framework.Required]
    public DateTime StartTime { get; set; }
    [Microsoft.Build.Framework.Required]
    public DateTime EndTime { get; set; }
}