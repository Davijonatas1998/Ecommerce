using Microsoft.AspNetCore.Identity;
using LojaVirtual.Models.ModelsUsuario
;

namespace LojaVirtual.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Cliente").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Cliente";
                role.NormalizedName = "Cliente";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        public void SeedUsers()
        {
            for (int i = 1; i <= 15; i++)
            {
                string email = $"Cliente{i}@localhost";

                if (_userManager.FindByEmailAsync(email).Result == null)
                {
                    if (_userManager.FindByEmailAsync(email).Result == null)
                    {
                        ApplicationUser user = new ApplicationUser();
                        user.FullName = $"Cliente {i}";
                        user.UserName = email;
                        user.Email = email;
                        user.NormalizedUserName = email.ToUpper();
                        user.NormalizedEmail = email.ToUpper();
                        user.EmailConfirmed = true;
                        user.LockoutEnabled = false;
                        user.SecurityStamp = Guid.NewGuid().ToString();

                        IdentityResult result = _userManager.CreateAsync(user, "hP0X31DnX9g&1Q-6").Result;
                        if (result.Succeeded)
                        {
                            _userManager.AddToRoleAsync(user, "Cliente").Wait();
                        }
                    }
                }
            }

            if (_userManager.FindByEmailAsync("Admin@localhost").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FullName = "Admin";
                user.UserName = "Admin@localhost";
                user.Email = "Admin@localhost";
                user.NormalizedUserName = "ADMIN@LOCALHOST";
                user.NormalizedEmail = "ADMIN@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = _userManager.CreateAsync(user, "hP0X31DnX9g&1Q-6").Result;
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}