using LojaVirtual.Models.ModelsEcommerce;

namespace LojaVirtual.Repository.Interface
{
    public interface IProduto
    {
        public IQueryable<Produtos> QuerybleProdutos();

        public Task<List<Produtos>> ListProdutos();

        public Task<Produtos> CreateProdutos(Produtos Produtos);

        public Task<Produtos> UpdateProdutos(Produtos Produtos);

        public Task<Produtos> GetProdutos(int? id);

        public Task<Produtos> DetailsProdutos(int? id);

        public Task<Produtos> DeleteProdutos(int? id);
    }
}
