using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Models.ModelsUsuario
{
    public class LoginUser
    {

        [Display(Name = "Nome de Usuario")]
        public string UserId { get; set; }

        public string Password { get; set; } = "hP0X31DnX9g&1Q-6";

        public string ReturnUrl { get; set; }
    }
}
