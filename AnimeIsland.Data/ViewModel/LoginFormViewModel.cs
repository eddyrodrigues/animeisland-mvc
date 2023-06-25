using System.ComponentModel.DataAnnotations;

namespace AnimeIsland.Data.ViewModel;
public class LoginFormViewModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
