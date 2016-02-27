﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeManager.Models
{
    public class IngredientModel
    {

        public int Id { get; set; }

     
        public string Title { get; set; }

    
        public string Description { get; set; }

        public decimal Price { get; set; }

     
        public bool IsOrganic { get; set; }

        public short IngredientType { get; set; }

    }
}