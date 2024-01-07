using System.ComponentModel.DataAnnotations;

namespace TermProject.models;

public class AppointAdminModel
{
    [Required]
    public String Email { get; set; }
}