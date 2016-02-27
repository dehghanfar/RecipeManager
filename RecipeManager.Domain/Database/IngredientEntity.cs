using RecipeManager.Domain.Model;
using System.Data.Entity;

namespace RecipeManager.Domain.Database
{


    public partial class IngredientEntity : DbContext
    {
        public IngredientEntity()
            : base("name=RecipeEntity")
        {
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
