using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeManager.Controllers;
using RecipeManager.Domain.Enums;
using RecipeManager.Domain.Model;
using RecipeManager.Domain.ServiceContract;
using RecipeManager.Models;
using Rhino.Mocks;

namespace RecipeManager.Tests.Controllers 
{
    [TestClass]
    public class HomeControllerTest
    {

        private HomeController _homeController;
        private IRecipeService _recipeService;
        private List<RecipeModel> _recipeModelList;
        private ISettings _configSettings;


        [TestInitialize]
        public void TestSetup()
        {

            _recipeService = MockRepository.GenerateMock<IRecipeService>();
            _configSettings= MockRepository.GenerateMock<ISettings>();
            _homeController = new HomeController(_recipeService, _configSettings);
            _recipeModelList = new List<RecipeModel>();
            _configSettings.Expect(x => x.GetTaxPercentage()).Return(0.086m);
            _configSettings.Expect(x => x.GetDiscountPercentage()).Return(0.05m);

            _recipeService.Expect(x => x.GetItemById(1)).Return(new Ingredient
            {
                Price = 0.67m,
                IsOrganic = true,
                IngredientType = (int)IngredientTypeEnum.Produce

            });
            _recipeService.Expect(x => x.GetItemById(2)).Return(new Ingredient
            {
                Price = 2.03m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Produce

            });
            _recipeService.Expect(x => x.GetItemById(3)).Return(new Ingredient
            {
                Price = 0.87m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Produce

            });
            _recipeService.Expect(x => x.GetItemById(4)).Return(new Ingredient
            {
                Price = 2.19m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Meat

            });
            _recipeService.Expect(x => x.GetItemById(5)).Return(new Ingredient
            {
                Price = 0.24m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Meat

            });
            _recipeService.Expect(x => x.GetItemById(6)).Return(new Ingredient
            {
                Price = 0.31m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Pantry

            });
            _recipeService.Expect(x => x.GetItemById(7)).Return(new Ingredient
            {
                Price = 1.92m,
                IsOrganic = true,
                IngredientType = (int)IngredientTypeEnum.Pantry

            });
            _recipeService.Expect(x => x.GetItemById(8)).Return(new Ingredient
            {
                Price = 1.26m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Pantry

            });
            _recipeService.Expect(x => x.GetItemById(9)).Return(new Ingredient
            {
                Price = 0.16m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Pantry

            });
            _recipeService.Expect(x => x.GetItemById(10)).Return(new Ingredient
            {
                Price = 0.17m,
                IsOrganic = false,
                IngredientType = (int)IngredientTypeEnum.Pantry

            });

        }

        [TestMethod]
        public void CalculateDiscount_Recipe1()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 2 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 10 });

            //act

            var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0.11m, result);
        }


        [TestMethod]
        public void CalculateDiscount_Recipe2()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 4 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 8 });

            //act

            var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0.09m, result);
        }


        [TestMethod]
        public void CalculateDiscount_Recipe3()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 3 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 5 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 8, ItemId = 6 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.333m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1.25m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 10 });

            //act

            var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0.07m, result);
        }


        [TestMethod]
        public void CalculateDiscount_Recipe_WhenThereIsNoRecipe()
        {
            //arrange
            _recipeModelList = new List<RecipeModel>();
              //act
              var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0m, result);
        }
        [TestMethod]
        public void CalculateDiscount_Recipe_WhenThereIsBelowZeroItemUnit()
        {
            _recipeModelList.Add(new RecipeModel { ItemUnit = -1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0, ItemId = 1 });

            //act
            var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0.04m, result);
        }


        [TestMethod]
        public void CalculateDiscount_Recipe_WhenItemId_DosntExist()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 99 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4 });

            //act
            var result = _homeController.CalculateDiscount(_recipeModelList);
            //assert
            Assert.AreEqual(0.00m, result);
        }


        [TestMethod]
        public void CalculateTax_Recipe1()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 2 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 10 });

            //act

            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0.21m, result);
        }


        [TestMethod]
        public void CalculateTax_Recipe2()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 4 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 8 });

            //act

            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0.91m, result);
        }

        [TestMethod]
        public void CalculateTax_Recipe3()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 3 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 5 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 8, ItemId = 6 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.333m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1.25m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 10 });

            //act

            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0.42m, result);
        }

        [TestMethod]
        public void CalculateTax_Recipe_WhenThereIsNoRecipe()
        {
            //arrange
            _recipeModelList = new List<RecipeModel>();
            //act
            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0m, result);
        }
        [TestMethod]
        public void CalculateTax_Recipe_WhenThereIsBelowZeroItemUnit()
        {
            _recipeModelList.Add(new RecipeModel { ItemUnit = -1, ItemId = 4 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 4 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0, ItemId = 4 });

            //act
            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0.21m, result);
        }

        [TestMethod]
        public void CalculateTax_Recipe_WhenItemId_DosntExist()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 99 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4 });

            //act
            var result = _homeController.CalculateTax(_recipeModelList);
            //assert
            Assert.AreEqual(0.00m, result);
        }
        [TestMethod]
        public void CalculateTotalCost_Recipe1()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 2 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 10 });
            decimal recipe1Discount = 0.11m;
            decimal recipe1Tax = 0.21m;

            //act

            var result = _homeController.CalculateTotalCost(recipe1Tax, recipe1Discount, _recipeModelList);
            //assert
            Assert.AreEqual(4.45m, result);
        }

        [TestMethod]
        public void CalculateTotalCost_Recipe2()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 4 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.5m, ItemId = 8 });
            decimal recipe1Discount = 0.09m;
            decimal recipe1Tax = 0.91m;

            //act

            var result = _homeController.CalculateTotalCost(recipe1Tax, recipe1Discount, _recipeModelList);
            //assert
            Assert.AreEqual(11.84m, result);
        }


        [TestMethod]
        public void CalculateTotalCost_Recipe3()
        {
            //arrange
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1, ItemId = 1 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 3 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 4, ItemId = 5 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 8, ItemId = 6 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.333m, ItemId = 7 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 1.25m, ItemId = 9 });
            _recipeModelList.Add(new RecipeModel { ItemUnit = 0.75m, ItemId = 10 });
            decimal recipe1Discount = 0.07m;
            decimal recipe1Tax = 0.42m;

            //act

            var result = _homeController.CalculateTotalCost(recipe1Tax, recipe1Discount, _recipeModelList);
            //assert
            Assert.AreEqual(8.91m, result);
        }

    }
}
