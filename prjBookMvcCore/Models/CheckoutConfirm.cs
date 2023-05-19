using prjBookMvcCore.ViewModel;

namespace prjBookMvcCore.Models
{
	public class CheckoutConfirm
	{
		public ShoppingcartInformation ?shoppingcartInformation { get; set; }
		public CheckOutConfirmViewModel ?checkoutConfirmViewModel { get; set; }
	}
}
