using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Domain.Entities.Calendaric
{
    public class Extracurricular
    {
        [Key] public string Id { get; set; }
        
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string MeetingDates { get; set; }

        [Required] public Teacher.Teacher Teacher { get; set; }
        [Required] public string TeacherId { get; set; }


    }
}
