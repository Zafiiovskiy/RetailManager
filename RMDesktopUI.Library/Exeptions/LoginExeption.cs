using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Helpers
{
    public class LoginExeption : Exception
    {
        public LoginExeption() 
        {
           
        }

        public LoginExeption(string message) : base(message) { }

        public LoginExeption(string message, Exception innerExeption) : base(message, innerExeption) { }
    }
}
