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
        public string Display
        {
            get
            {
                return $"{Product.ProductName} - {Quantity}";
            }
        }
    }
}
