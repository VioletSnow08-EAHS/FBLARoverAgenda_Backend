using Microsoft.Build.Framework;

namespace FBLARoverAgenda_Backend.Domain.Entities.LunchMenuItem;

public class LunchMenuItem
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
}