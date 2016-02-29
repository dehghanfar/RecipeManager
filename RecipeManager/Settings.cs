using System.Web.Configuration;

namespace RecipeManager
{
    public  class Settings: ISettings
    {
      

        public decimal GetDiscountPercentage()
        {
            return decimal.Parse(WebConfigurationManager.AppSettings["DiscountPercentage"]);
        }

        public decimal GetTaxPercentage()
        {
            return decimal.Parse(WebConfigurationManager.AppSettings["TaxPercentage"]);
        }
    }
}