using RMDataManager.Library.ManagingDataFiles.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void PostSale(SaleModel sale, string cashierId)
        {
            List<SaleDetailDataModel> saleDetails = new List<SaleDetailDataModel>();
            ProductData productData = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate();
            foreach (var item in sale.SaleDetails)
            {
                var details = new SaleDetailDataModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                var productInfo = productData.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product WHERE id = {item.ProductId} is null");
                }
                details.PurchasePrice = productInfo.RetailPrice * details.Quantity;
                if (productInfo.isTaxable)
                {
                    details.Tax = details.PurchasePrice * taxRate;
                }
                saleDetails.Add(details);
            }


            SaleDataModel saleData = new SaleDataModel
            {
                SubTotal = saleDetails.Sum(x => x.PurchasePrice),
                Tax = saleDetails.Sum(x => x.Tax),
                CashierId = cashierId,
            };
            saleData.Total = saleData.SubTotal + saleData.Tax;

            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData("dbo.spSaleInsert", saleData, "RMData");

            saleData.Id = sql.LoadData<int, dynamic>("dbo.spSaleLookup", new {saleData.CashierId,saleData.SaleDate }, "RMData").FirstOrDefault();
            foreach (var item in saleDetails)
            {
                item.SaleId = saleData.Id;
                sql.SaveData("dbo.spSaleDetailInsert", item, "RMData");
            }
        }
    }
}
