using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeManager.Domain.Model
{
    

    [Table("Ingredient")]
    public partial class Ingredient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsOrganic { get; set; }

        public short IngredientType { get; set; }
    }
}
