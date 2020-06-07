using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		IProductEndPoint _productEndPoint;
		IConfigHelper _configHelper;
		ISaleEndPoint _saleEndPoint;

		public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint)
		{
			_productEndPoint = productEndPoint;
			_configHelper = configHelper;
			_saleEndPoint = saleEndPoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}
		public async Task LoadProducts()
		{
			var prods = await _productEndPoint.GetAll();
			Products = new BindingList<ProductModel>(prods);
		}
		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set { _products = value; NotifyOfPropertyChange(() => Products); }
		}

		private ProductModel _selectedProduct;

		public ProductModel SelectedProduct
		{
			get { return _selectedProduct; }
			set 
			{ 
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private CartProductModel _selectedCartItem;

		public CartProductModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{ 
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}


		private BindingList<CartProductModel> _cart = new BindingList<CartProductModel>();

		public BindingList<CartProductModel> Cart
		{
			get { return _cart; }
			set { _cart = value; }
		}

		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value; 
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		//Buttons
		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
				return output;
			}
		}

		public void AddToCart()
		{
			CartProductModel inCartItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (inCartItem != null)
			{
				inCartItem.Quantity += ItemQuantity;
				Cart.Remove(inCartItem);
				Cart.Add(inCartItem);
			}
			else
			{
				CartProductModel item = new CartProductModel
				{
					Product = SelectedProduct,
					Quantity = ItemQuantity
				};
				Cart.Add(item);
			}
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => SelectedCartItem);
			NotifyOfPropertyChange(() => CanRemoveFromCart);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				if (SelectedCartItem != null && Cart.Count > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public void RemoveFromCart()
		{
			Cart.Remove(SelectedCartItem);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				if (Cart.Count > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public async Task CheckOut()
		{
			SaleModel sale = new SaleModel();
			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.Quantity
				});
			}
			await _saleEndPoint.PostSale(sale);
			Cart.Clear();
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		//Values
		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}
		
		public string SubTotal
		{
			get
			{
				return CalculateSubTotal().ToString("C");
			}
		}
		public string Total
		{
			get
			{
				return CalculateTotal().ToString("C");
			}
		}

		//Calculations
		private decimal CalculateSubTotal() {
			decimal subTotal = 0;
			subTotal = Cart.Sum(x => x.Product.RetailPrice * x.Quantity);
			return subTotal;
		}
		private decimal CalculateTax()
		{
			decimal tax = 0;
			decimal taxRate = _configHelper.GetTaxRate();
			tax = Cart.Where(x => x.Product.isTaxable)
				.Sum(x => x.Product.RetailPrice * x.Quantity * taxRate);
			return tax;
		}
		private decimal CalculateTotal()
		{
			return CalculateSubTotal() + CalculateTax();
		}
		
	}
}
