﻿using RMDataManager.Library.ManagingDataFiles.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductLookup", new { }, "RMData");
            return output;
        }
        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "RMData").FirstOrDefault();
            return output;
        }
    }
}
