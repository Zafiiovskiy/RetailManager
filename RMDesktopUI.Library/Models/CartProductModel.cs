using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Models
{
    public class CartProductModel
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }

        public string DisplaySumPriceInCart
        {
            get
            {
                return (Product.RetailPrice * Quantity).ToString("C");
            }
        }
        public string DisplayQuantity
        {
            get
            {
                return Quantity.ToString();
            }
        }
    }
}
