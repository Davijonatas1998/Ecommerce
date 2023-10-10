using LojaVirtual.Context;
using LojaVirtual.Models.ModelsEcommerce;
using LojaVirtual.Models.ModelsUsuario;
using LojaVirtual.Repository.Interface;
using LojaVirtual.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace LojaVirtual.Repository
{
    public class RepositoryBase : IProduto, ICheckout, IProfileUser
    {
        private readonly Contexto _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepositoryBase(Contexto contexto, UserManager<ApplicationUser> userManager)
        {
            _context = contexto;
            _userManager = userManager;
        }

        public async Task<Produtos> CreateProdutos(Produtos Produtos)
        {
            _context.Add(Produtos);
            await _context.SaveChangesAsync();
            return Produtos;
        }

        public async Task<Produtos> DeleteProdutos(int? id)
        {
            var Produtos = await _context.Produtos.FindAsync(id);
            if (Produtos != null)
            {
                _context.Produtos.Remove(Produtos);
                await _context.SaveChangesAsync();
            }
            return Produtos;
        }

        public async Task<Produtos> DetailsProdutos(int? id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(c => c.IdProdutos == id);
        }

        public async Task<Produtos> GetProdutos(int? id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<List<Produtos>> ListProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        public IQueryable<Produtos> QuerybleProdutos()
        {
            return _context.Produtos.AsQueryable();
        }

        public async Task<Produtos> UpdateProdutos(Produtos Produtos)
        {
            _context.Update(Produtos);
            await _context.SaveChangesAsync();
            return Produtos;
        }

        public async Task<Carrinho> CreateCarrinho(Carrinho Carrinho)
        {
            _context.Add(Carrinho);
            await _context.SaveChangesAsync();
            return Carrinho;
        }

        public async Task<Carrinho> DeleteCarrinho(int? id, string IdUser)
        {
            var Carrinho = await _context.Carrinhos.Where(c => c.Id == IdUser).FirstOrDefaultAsync(c => c.IdCarrinho == id);
            if (Carrinho != null)
            {
                _context.Carrinhos.Remove(Carrinho);
                await _context.SaveChangesAsync();
            }
            return Carrinho;
        }

        public async Task<Carrinho> DetailsCarrinho(int? id, string userId)
        {
            return await _context.Carrinhos.Where(c => c.Id == userId).FirstOrDefaultAsync(c => c.IdProdutos == id);
        }

        public async Task<Carrinho> GetCarrinho(int? id)
        {
            return await _context.Carrinhos.FindAsync(id);
        }

        public async Task<List<Carrinho>> ListCarrinho(string User)
        {
            return await _context.Carrinhos.Where(c => c.Id == User).ToListAsync();
        }

        public IQueryable<Carrinho> QuerybleCarrinho()
        {
           return _context.Carrinhos.AsQueryable();
        }

        public async Task<Carrinho> UpdateCarrinho(Carrinho Carrinho)
        {
            _context.Update(Carrinho);
            await _context.SaveChangesAsync();
            return Carrinho;
        }

        public async Task<IList<string>> GetRole(ApplicationUser user)
        {
            var Profile = await _userManager.GetRolesAsync(user);
            return Profile;
        }

        public async Task<ApplicationUser> GetProfileAsync(ClaimsPrincipal userClaimsPrincipal)
        {
            return await _userManager.GetUserAsync(userClaimsPrincipal);
        }

        public async Task<List<ApplicationUser>> ListProfilesAsync()
        {
            var Users = await _userManager.Users.ToListAsync();
            var Profiles = new List<ApplicationUser>();

            foreach (var User in Users)
            {
                var roles = await _userManager.GetRolesAsync(User);
                if (!roles.Contains("Admin"))
                {
                    Profiles.Add(User);
                }
            }

            return Profiles;
        }

        public async Task<Carrinho> GetPayment(int id)
        {
            var Carrinho = await _context.Carrinhos.FindAsync(id);
            string accessToken = ServicesManager.Key;
            string url = $"https://api.mercadopago.com/v1/payments/search?external_reference={Carrinho.Metadata}&access_token={accessToken}";

            if (!Carrinho.Comprado)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject paymentJson = JObject.Parse(jsonResponse);

                        if (paymentJson["results"] is JArray results && results.Count > 0)
                        {
                            string status = (string)paymentJson["results"][0]["status"];
                            if (status == "approved")
                            {
                                Carrinho.Comprado = true;
                                _context.Update(Carrinho);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            return Carrinho;
        }
    }
}
