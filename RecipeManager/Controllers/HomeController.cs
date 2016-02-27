﻿using System;
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

        public HomeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
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
            string message = "Success";

            if (itemUnit <= 0)
            {
                message = "ZeroEntry";
                return Json(new
                {
                    message
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                RecipeModel recipeModel = new RecipeModel
                {
                    ItemId = itemId,
                    ItemUnit = (decimal)itemUnit,
                    ItemName = itemName,

                };

                var myRecipeModelList = (List<RecipeModel>)Session["MyStoredSessionRecipeModel"];

                if (myRecipeModelList != null)
                {
                    myRecipeModelList.Add(recipeModel);
                }
                else
                {
                    myRecipeModelList = new List<RecipeModel> { recipeModel };
                }
                Session["MyStoredSessionRecipeModel"] = myRecipeModelList;

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

        [HttpPost]
        public JsonResult RemoveRecipeOrders(float itemUnit, int itemId)
        {
            string message = "Success";

            try
            {
                var myRecipeModelList = (List<RecipeModel>)Session["MyStoredSessionRecipeModel"];
                var itemToRemove = myRecipeModelList.FirstOrDefault(r => r.ItemId == itemId && r.ItemUnit == (decimal)itemUnit);
                if (itemToRemove != null)
                {
                    myRecipeModelList.Remove(itemToRemove);
                }
                Session["MyStoredSessionRecipeModel"] = myRecipeModelList;
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
            var myRecipeModelList = (List<RecipeModel>)Session["MyStoredSessionRecipeModel"];

            return PartialView(myRecipeModelList);
        }


        public ActionResult CalculateFinalRecipt()
        {
            var myRecipeModelList = (List<RecipeModel>)Session["MyStoredSessionRecipeModel"];

            var recipeReciptModel = new RecipeReciptModel();

            if (myRecipeModelList != null)
            {

                recipeReciptModel.Discount = CalculateDiscount(myRecipeModelList);
                recipeReciptModel.Tax = CalculateTax(myRecipeModelList);
                recipeReciptModel.Total = CalculateTotalCost(recipeReciptModel.Tax, recipeReciptModel.Discount,
                    myRecipeModelList);
            }

            return PartialView(recipeReciptModel);



        }


        public decimal CalculateDiscount(List<RecipeModel> recipeList)
        {

          

            decimal itemsToBeDiscounted = (from item in recipeList where item.ItemUnit > 0 let ingredient = _recipeService.GetItemById(item.ItemId) where ingredient != null && ingredient.IsOrganic select ingredient.Price*item.ItemUnit).Sum();


            //decimal itemsToBeDiscounted = 0m;
            //foreach (var item in recipeList)
            //{
            //    if (item.ItemUnit > 0)
            //    {
            //        var ingredient = _recipeService.GetItemById(item.ItemId);

            //        if (ingredient != null && ingredient.IsOrganic)
            //        {

            //            decimal itemPrice = ingredient.Price * item.ItemUnit;
            //            itemsToBeDiscounted += itemPrice;
            //        }
            //    }

            //}



            return CalculateDiscount(itemsToBeDiscounted);

        }

        private decimal CalculateDiscount(decimal itemPrice)
        {

            itemPrice = itemPrice - itemPrice * (1 - 0.05m);

            double multiplier = Math.Pow(10, Convert.ToDouble(2));
            return Math.Ceiling(itemPrice * (decimal)multiplier) / (decimal)multiplier;


        }
        public decimal CalculateTax(List<RecipeModel> recipeList)
        {
         
            decimal itemsToBeTaxed = (from item in recipeList where item.ItemUnit > 0 let ingredient = _recipeService.GetItemById(item.ItemId) where ingredient != null && ingredient.IngredientType != (int) IngredientTypeEnum.Produce select ingredient.Price*item.ItemUnit).Sum();


            //decimal itemsToBeTaxed = 0m;
            //foreach (var item in recipeList)
            //{
            //    if (item.ItemUnit > 0)
            //    {
            //        var ingredient = _recipeService.GetItemById(item.ItemId);


            //        if (ingredient != null && ingredient.IngredientType != (int)IngredientTypeEnum.Produce)
            //        {
            //            decimal itemPrice = ingredient.Price * item.ItemUnit;
            //            itemsToBeTaxed += itemPrice;
            //        }
            //    }

            //}


            return CalculateTax(itemsToBeTaxed);

        }

        private decimal CalculateTax(decimal itemPrice)
        {

            decimal tax = itemPrice * 0.086m;

            return Math.Ceiling(tax / 0.07m) * 0.07m;


        }

        public decimal CalculateTotalCost(decimal tax, decimal discount, List<RecipeModel> recipeList)
        {
            decimal totalCost = (from item in recipeList let ingredient = _recipeService.GetItemById(item.ItemId) select ingredient.Price * item.ItemUnit).Aggregate<decimal, decimal>(0, (current, itemPrice) => current + itemPrice);
            //foreach (var item in recipeList)
            //{
            //    var ingredient = _recipeService.GetItemById(item.ItemId);
            //    decimal itemPrice = ingredient.Price * item.ItemUnit;
            //    totalCost = totalCost + itemPrice;
            //}

            totalCost = totalCost + tax - discount;


            double multiplier = Math.Pow(10, Convert.ToDouble(2));
            return Math.Ceiling(totalCost * (decimal)multiplier) / (decimal)multiplier;


        }

    }
}