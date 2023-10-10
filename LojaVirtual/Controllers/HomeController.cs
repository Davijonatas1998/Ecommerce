using LojaVirtual.Models;
using LojaVirtual.Models.ModelsEcommerce;
using LojaVirtual.Repository.Interface;
using LojaVirtual.Services;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;


namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduto _produto;
        private readonly ICheckout _checkout;
        private readonly IProfileUser _profileUser;

        public HomeController(IProduto produto, ICheckout checkout, IProfileUser profileUser)
        {
            _produto = produto;
            _checkout = checkout;
            _profileUser = profileUser;
        }

        public IActionResult Index(string result)
        {
            var Produto = _produto.QuerybleProdutos();
            if (!string.IsNullOrEmpty(result))
            {
                Produto = Produto.Where(c => c.Nome.Contains(result));
            }

            return View(Produto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            return View(await _produto.GetProdutos(id));
        }

        [Authorize]
        public async Task<IActionResult> Carrinho()
        {
            var profile = await _profileUser.GetProfileAsync(User);
            var Carrinho = await _checkout.ListCarrinho(profile.Id);
            return View(Carrinho.Where(c => c.Comprado == false).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverProduto(int? id)
        {
            var profile = await _profileUser.GetProfileAsync(User);
            await _checkout.DeleteCarrinho(id, profile.Id);
            return RedirectToAction(nameof(Carrinho));
        }

        [Authorize]
        public async Task<IActionResult> MeusPedidos()
        {
            var profile = await _profileUser.GetProfileAsync(User);
            var Carrinho = await _checkout.ListCarrinho(profile.Id);
            return View(Carrinho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCard(int? id, int Quantidade)
        {
            var profile = await _profileUser.GetProfileAsync(User);
            var ProductCheck = await _checkout.DetailsCarrinho(id, profile.Id);
            if(ProductCheck == null)
            {
                var Checkout = new Carrinho
                {
                    Comprado = false,
                    Quantidade = Quantidade,
                    DataCompra = DateTime.Now.AddDays(3),
                    IdProdutos = id.Value,
                    Id = profile.Id
                };

                await _checkout.CreateCarrinho(Checkout);
            }
            else
            {
                if (!ProductCheck.Comprado)
                {
                    ProductCheck.Quantidade = Quantidade;
                    await _checkout.UpdateCarrinho(ProductCheck);
                }
            }
           
            return Json(ProductCheck);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(int Preco)
        {
            var profile = await _profileUser.GetProfileAsync(User);
            var Check = await _checkout.ListCarrinho(profile.Id);

            var Token = ServicesManager.External(9);
            MercadoPagoConfig.AccessToken = ServicesManager.Key;

            var client = new PreferenceClient();
            var item = new PreferenceItemRequest
            {
                Title = "Loja de Bolos - Cruzeiro do Sul",
                Quantity = 1,
                CurrencyId = "BRL",
                UnitPrice = Preco,
            };

            foreach (var produtos in Check)
            {
                var Carrinho = await _checkout.GetCarrinho(produtos.IdCarrinho);

                Carrinho.Metadata = Token;
                Carrinho.DataCompra = DateTime.Now.AddDays(3);
                await _checkout.UpdateCarrinho(Carrinho);
            }

            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest> { item },
                Payer = new PreferencePayerRequest
                {
                    Email = profile.Email,
                },
                ExternalReference = Token,
            };

            Preference preference = await client.CreateAsync(preferenceRequest);
            return Redirect(preference.InitPoint);
        }
    }
}