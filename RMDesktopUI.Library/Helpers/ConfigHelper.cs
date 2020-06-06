using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["UAtaxRate"];
            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);
            if (!isValidTaxRate)
            {
                throw new ConfigurationErrorsException("Tax rate is not correct");
            }
            return output;
        }
    }
}
