using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Controllers;
using prjBookMvcCore.Models;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private string GetCategoryNameById(int categoryId)
        {
            using (var db = new BookShopContext())
            {
                var category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                return category?.CategoryName;
            }
        }

        [Test]
        public void TestCategory()
        {
            // Arrange
            CategoryController categoryController = new CategoryController();

            // Act
            IActionResult result = categoryController.分類頁面(18, 0, 1);
            ViewResult viewResult = result as ViewResult;
            var menuInformation = viewResult.Model as MenuInformation;
            var categoryName = GetCategoryNameById(menuInformation.categoryId);

            // Assert
            Assert.AreEqual("日中對照", categoryName);
        }
    }
}