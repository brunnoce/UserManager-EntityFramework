
using System.ComponentModel.DataAnnotations;
public class LoginViewModel
{
    [Required]
    [Display(Name = "Usuario")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
}

