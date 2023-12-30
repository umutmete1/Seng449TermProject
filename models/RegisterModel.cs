using System.ComponentModel.DataAnnotations;

namespace TermProject.models;

public class RegisterModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    public string Address { get; set; }
}