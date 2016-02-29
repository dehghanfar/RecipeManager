using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeManager
{
    public interface ISettings
    {
        decimal GetDiscountPercentage();

        decimal GetTaxPercentage();
    }
}