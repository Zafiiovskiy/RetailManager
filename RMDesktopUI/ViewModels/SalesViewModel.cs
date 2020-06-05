﻿using Caliburn.Micro;
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
		private BindingList<string> _products;

		public BindingList<string> Products
		{
			get { return _products; }
			set { _products = value; }
		}

		private BindingList<string> _cart;

		public BindingList<string> Cart
		{
			get { return _cart; }
			set { _cart = value; }
		}

		private int _itemQuantity;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value; 
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		//Buttons
		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public void AddToCart()
		{

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public void RemoveFromCart()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				return output;
			}
		}

		public void CheckOut()
		{

		}

		//Values
		public string SubTotal { 
			get
			{
				return "$0.00";
			}
		}
		public string Tax
		{
			get
			{
				return "$0.00";
			}
		}
		public string Total
		{
			get
			{
				return "$0.00";
			}
		}
	}
}
