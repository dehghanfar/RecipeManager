using System;
using System.Collections.Generic;
using System.Linq;
using RecipeManager.Domain.Database;
using RecipeManager.Domain.Model;
using RecipeManager.Domain.ServiceContract;

namespace RecipeManager.Service
{
   public class RecipeService : IRecipeService
    {
        static readonly IngredientEntity Context = new IngredientEntity();

        public List<Ingredient> GetAllItems()
        {
            var items = Context.Ingredients.ToList();

            return items;
        }

       public Ingredient GetItemById(int id)
       {
            return Context.Ingredients.Find(id);
        }
    }
}
