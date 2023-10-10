using LojaVirtual.Models.ModelsEcommerce;
using LojaVirtual.Models.ModelsUsuario;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Context
{
    public class Contexto : IdentityDbContext<ApplicationUser>
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Produtos> Produtos { get; set; }

        public DbSet<Carrinho> Carrinhos { get; set; }
    }
}
