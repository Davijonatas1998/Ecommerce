using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Models.ModelsEcommerce
{
    public class Produtos
    {
        [Key]
        public int IdProdutos { get; set; }

        [DataType(DataType.ImageUrl)]
        public string URLImagem { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MinLength(10, ErrorMessage = "O tamanho da descrição deve ser de 10 ou mais caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MinLength(10, ErrorMessage = "O tamanho da descrição deve ser de 10 ou mais caracteres.")]
        public string Descricao { get; set; }

        public Categoria Categoria { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Preco { get; set; }

        public virtual ICollection<Carrinho> Carrinho { get; set;}
    }
}