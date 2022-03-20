using System.ComponentModel.DataAnnotations;
using Audit.EntityFramework;

namespace FBLARoverAgenda_Backend.Domain.Entities;

[AuditInclude]
public class ConfigurationItem
{
    [Key]
    public string Key { get; set; }

    public string Value { get; set; }
}