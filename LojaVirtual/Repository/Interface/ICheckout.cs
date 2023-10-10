using LojaVirtual.Models.ModelsEcommerce;
using System.Reflection.Metadata;

namespace LojaVirtual.Repository.Interface
{
    public interface ICheckout
    {
        public IQueryable<Carrinho> QuerybleCarrinho();

        public Task<List<Carrinho>> ListCarrinho(string User);

        public Task<Carrinho> CreateCarrinho(Carrinho Carrinho);

        public Task<Carrinho> UpdateCarrinho(Carrinho Carrinho);

        public Task<Carrinho> GetCarrinho(int? id);

        public Task<Carrinho> DetailsCarrinho(int? id, string userId);

        public Task<Carrinho> DeleteCarrinho(int? id, string IdUser);

        public Task<Carrinho> GetPayment(int id);
    }
}
