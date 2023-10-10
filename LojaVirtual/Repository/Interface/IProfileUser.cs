using LojaVirtual.Models.ModelsUsuario;
using System.Security.Claims;

namespace LojaVirtual.Repository.Interface
{
    public interface IProfileUser
    {
        public Task<ApplicationUser> GetProfileAsync(ClaimsPrincipal userClaimsPrincipal);

        public Task<List<ApplicationUser>> ListProfilesAsync();

        public Task<IList<string>> GetRole(ApplicationUser user);
    }
}
