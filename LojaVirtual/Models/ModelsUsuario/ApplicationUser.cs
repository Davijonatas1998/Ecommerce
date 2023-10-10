using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LojaVirtual.Models.ModelsUsuario
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}