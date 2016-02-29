using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RecipeManager.Domain.Enums;
using RecipeManager.Domain.ServiceContract;
using RecipeManager.Models;

namespace RecipeManager.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRecipeService _recipeService;
        private readonly ISettings _config;

        public HomeController(IRecipeService recipeService, ISettings config)
        {
            _recipeService = recipeService;
            _config = config;
        }

        public ActionResult Index()
        {
            return RedirectToAction("RecipeManager", "Home");
        }

        public ActionResult RecipeManager()
        {
            ViewBag.Items = _recipeService.GetAllItems();

            return View();
        }

        [HttpPost]
        public JsonResult AddRecipeOrders(float itemUnit, int itemId, string itemName)
        {
            string message;
            try
            {
                message = CheckRecipeUnitMessage(itemUnit);
                if (IsValidRecipeUnit(itemUnit))
                {
                    var recipeModel = new RecipeModel
                    {
                        ItemId = itemId,
                        ItemUnit = (decimal) itemUnit,
                        ItemName = itemName,

                    };
                    var myRecipeModelList = MySession.MySessionRecipeModel;
                    myRecipeModelList.Add(recipeModel);
                }
            }
            catch (Exception e)
            {
                message = "GeneralFailure";
                //todo: log the specific error
            }

            return Json(new
            {
                message
            }, JsonRequestBehavior.AllowGet);
        }
        private bool IsValidRecipeUnit(float itemUnit)
        {
            return !(itemUnit <= 0);
        }

        private string CheckRecipeUnitMessage(float itemUnit)
        {
            var output = "Success";
            if (itemUnit <= 0)
            {
                output = "ZeroEntry";
            }

            return output;
        }

        [HttpPost]
        public JsonResult RemoveRecipeOrders(float itemUnit, int itemId)
        {
            var message = "Success";
            try
            {
                var myRecipeModelList = MySession.MySessionRecipeModel;
                var itemToRemove = myRecipeModelList.FirstOrDefault(r => r.ItemId == itemId && r.ItemUnit == (decimal)itemUnit);
                if (itemToRemove != null)
                {
                    MySession.MySessionRecipeModel.Remove(itemToRemove);
                }
            }
            catch (Exception e)
            {
                message = "Failed";
                //todo: log the specific error
            }

            return Json(new
            {
                message
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _RecipeOrdersList()
        {
            var myRecipeModelList = MySession.MySessionRecipeModel;

            return PartialView(myRecipeModelList);
        }

        public ActionResult CalculateFinalRecipt()
        {
            var myRecipeModelList = MySession.MySessionRecipeModel;
            var recipeReciptModel = new RecipeReciptModel();
            if (myRecipeModelList == null) return PartialView(recipeReciptModel);
            recipeReciptModel.Discount = CalculateDiscount(myRecipeModelList);
            recipeReciptModel.Tax = CalculateTax(myRecipeModelList);
            recipeReciptModel.Total = CalculateTotalCost(recipeReciptModel.Tax, recipeReciptModel.Discount,
                myRecipeModelList);

            return PartialView(recipeReciptModel);
        }

        public decimal CalculateDiscount(List<RecipeModel> recipeList)
        {
            var itemsToBeDiscounted = (from item in recipeList where item.ItemUnit > 0 let ingredient = _recipeService.GetItemById(item.ItemId) where ingredient != null && ingredient.IsOrganic select ingredient.Price * item.ItemUnit).Sum();

            return CalculateDiscount(itemsToBeDiscounted);
        }

        private decimal CalculateDiscount(decimal itemPrice)
        {
            itemPrice = itemPrice - itemPrice * (1 - _config.GetDiscountPercentage());
            var multiplier = Math.Pow(10, Convert.ToDouble(2));

            return Math.Ceiling(itemPrice * (decimal)multiplier) / (decimal)multiplier;
        }
        public decimal CalculateTax(List<RecipeModel> recipeList)
        {
            var itemsToBeTaxed = (from item in recipeList where item.ItemUnit > 0 let ingredient = _recipeService.GetItemById(item.ItemId) where ingredient != null && ingredient.IngredientType != (int)IngredientTypeEnum.Produce select ingredient.Price * item.ItemUnit).Sum();

            return CalculateTax(itemsToBeTaxed);
        }

        private decimal CalculateTax(decimal itemPrice)
        {
            var tax = itemPrice * _config.GetTaxPercentage();

            return Math.Ceiling(tax / 0.07m) * 0.07m;
        }

        public decimal CalculateTotalCost(decimal tax, decimal discount, List<RecipeModel> recipeList)
        {
            var totalCost = (from item in recipeList let ingredient = _recipeService.GetItemById(item.ItemId) select ingredient.Price * item.ItemUnit).Aggregate<decimal, decimal>(0, (current, itemPrice) => current + itemPrice);
            totalCost = totalCost + tax - discount;
            var multiplier = Math.Pow(10, Convert.ToDouble(2));

            return Math.Ceiling(totalCost * (decimal)multiplier) / (decimal)multiplier;
        }

    }
}