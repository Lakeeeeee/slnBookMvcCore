using prjBookMvcCore.ViewModel;

namespace prjBookMvcCore.Models
{
	public class CheckoutConfirm
	{
		public ShoppingcartInformation ?shoppingcartInformation { get; set; }
		public OrderViewModel ?checkoutConfirmViewModel { get; set; }
	}
}
