using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBLARoverAgenda_Backend.Domain.Entities.Teacher
{
    public class Teacher
    {
        [Key] public string Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        [Required][EmailAddress] public string Email { get; set; }
    }
}
