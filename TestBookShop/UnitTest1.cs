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
            dynamic xx = categoryController.��������(0,0);
            Assert.That(xx.ActionID, Is.EqualTo("��������"));
            //Assert.Pass();
        }
    }
}