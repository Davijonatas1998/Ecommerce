using LojaVirtual.Models.ModelsUsuario;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtual.Models.ModelsEcommerce
{
    public class Carrinho
    {
        [Key]
        public int IdCarrinho { get; set; }

        public bool Comprado { get; set; }

        public int Quantidade { get; set; }

        public string Metadata { get; set; }

        public DateTime DataCompra { get; set; }

        public int IdProdutos { get; set; }

        public string Id { get; set; }

        [ForeignKey("IdProdutos")]
        public virtual Produtos Produtos { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser Usuarios { get; set; }
    }
}
