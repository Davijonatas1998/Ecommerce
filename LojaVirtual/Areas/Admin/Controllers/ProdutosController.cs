using LojaVirtual.Models.Config_Arquivos;
using LojaVirtual.Models.ModelsEcommerce;
using LojaVirtual.Repository.Interface;
using LojaVirtual.Services;
using MercadoPago.Resource.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReflectionIT.Mvc.Paging;
using System.Data;
using System.Reflection.Metadata;

namespace LojaVirtual.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProdutosController : Controller
    {
        private readonly IProduto _produto;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ConfigurationImagens _myConfig;

        public ProdutosController(IProduto produto, IWebHostEnvironment hostingEnvironment, IOptions<ConfigurationImagens> myConfiguration)
        {
            _produto = produto;
            _hostingEnvironment = hostingEnvironment;
            _myConfig = myConfiguration.Value;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var result = _produto.QuerybleProdutos();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Nome.Contains(filter));
            }

            var model = await PagingList.CreateAsync(result, 4, pageindex, sort, "Nome");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProdutos, URLImagem, Nome, Descricao, Categoria, Preco")] Produtos produtos, List<IFormFile> Imagem)
        {
            var TokenImage = ServicesManager.TokenSystem(8);
            foreach (var formFile in Imagem)
            {
                if (!ServicesManager.IsValidImage(formFile))
                {
                    ModelState.AddModelError("", "Insira uma imagem válida.");
                    return View(produtos);
                }

                var fileName = TokenImage + formFile.FileName;
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaProduto);

                using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                produtos.URLImagem = fileName;
            }

            await _produto.CreateProdutos(produtos);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string IdResult)
        {
            int? id = ServicesManager.IdAction(IdResult);
            var servico = await _produto.GetProdutos(id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdProdutos, URLImagem, Nome, Descricao, Categoria, Preco")] Produtos produtos, List<IFormFile> Imagem)
        {
            var TokenImage = ServicesManager.TokenSystem(8);
            foreach (var formFile in Imagem)
            {
                if (!ServicesManager.IsValidImage(formFile))
                {
                    ModelState.AddModelError("", "Insira uma imagem válida.");
                    return View(produtos);
                }

                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaProduto);
                if (!string.IsNullOrEmpty(produtos.URLImagem))
                {
                    ServicesManager.DeleteImage(_hostingEnvironment, produtos.URLImagem, _myConfig.NomePastaProduto);
                }

                var fileName = TokenImage + formFile.FileName;
                var newImagePath = Path.Combine(filePath, fileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                produtos.URLImagem = fileName;
            }

            await _produto.UpdateProdutos(produtos);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string IdResult)
        {
            int? id = ServicesManager.IdAction(IdResult);
            var produto = await _produto.GetProdutos(id);

            if (!string.IsNullOrEmpty(produto.URLImagem))
            {
                ServicesManager.DeleteImage(_hostingEnvironment, produto.URLImagem, _myConfig.NomePastaProduto);
            }

            await _produto.DeleteProdutos(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
