using prjBookMvcCore.Controllers;

namespace TestBookShop
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestCategory()
        {
            CategoryController categoryController = new CategoryController();
            dynamic xx = categoryController.分類頁面(0,0);
            Assert.That(xx.ActionID, Is.EqualTo("分類頁面"));
            //Assert.Pass();
        }
    }
}