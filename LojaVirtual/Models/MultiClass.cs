using LojaVirtual.Models.ModelsEcommerce;

namespace LojaVirtual.Models
{
    public class MultiClass
    {
        public virtual List<Carrinho> ListCarrinhos { get; set;}

        public virtual List<Produtos> ListProdutos { get; set; }
    }
}
