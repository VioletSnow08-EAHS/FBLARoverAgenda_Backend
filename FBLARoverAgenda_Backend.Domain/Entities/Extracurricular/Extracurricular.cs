using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBLARoverAgenda_Backend.Domain.Entities.Extracurricular
{
    public class Extracurricular
    {
        [Key] public string Id { get; set; }
        
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public List<DateTime> MeetingDates { get; set; }

        [Required] public Teacher.Teacher Teacher { get; set; }
        [Required] public string TeacherId { get; set; }


    }
}
