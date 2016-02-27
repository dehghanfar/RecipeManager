using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.Domain.Model;

namespace RecipeManager.Domain.ServiceContract
{
    public interface IRecipeService
    {
       


        List<Ingredient> GetAllItems();


        // get ingre by item


        Ingredient GetItemById(int id);

    }
}
