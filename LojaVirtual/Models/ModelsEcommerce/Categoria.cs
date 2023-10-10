using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LojaVirtual.Models.ModelsEcommerce
{
    public enum Categoria
    {
        [Display(Name = "Bolos Inteiros")]
        BolosInteiros,

        [Display(Name = "Bolos Especiais")]
        BolosEspeciais,

        [Display(Name = "Cupcakes")]
        Cupcakes,

        [Display(Name = "Sobremesas")]
        Sobremesas,

        [Display(Name = "Bebidas")]
        Bebidas,
    }
}