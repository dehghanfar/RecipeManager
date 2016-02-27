using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeManager.Models
{
    public class RecipeModel
    {

        public decimal ItemUnit { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

    }


    public class RecipeReciptModel

    {
        
        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }


    }

}