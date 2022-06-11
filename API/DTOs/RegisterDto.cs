using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
   [Required] public string DisplayName { get; set; }
   [Required] [EmailAddress] public string Email { get; set; }
   [Required] [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "Password Requirements wrong ! 1 1 1 1 ")] 
   public string Password { get; set; }
}