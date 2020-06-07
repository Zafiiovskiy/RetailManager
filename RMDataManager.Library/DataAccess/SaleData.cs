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

            using(SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");
                    sql.SaveDataInTransaction("dbo.spSaleInsert", saleData);
                    saleData.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", new { saleData.CashierId, saleData.SaleDate }, "RMData").FirstOrDefault();
                    foreach (var item in saleDetails)
                    {
                        item.SaleId = saleData.Id;
                        sql.SaveDataInTransaction("dbo.spSaleDetailInsert", item);
                    }
                }
                catch
                {
                    sql.RollBackTransaction();
                    throw;
                }
            }
        }

        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSaleGetReport", new { }, "RMData");
            return output;
        }
    }
}
