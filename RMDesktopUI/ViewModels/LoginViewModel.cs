using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName;

		private string _password;

		public string UserName
		{
			get { return _userName; }
			set 
			{ 
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
				NotifyOfPropertyChange(() => CanLogInButton);
			}
		}

		public string Password
		{
			get { return _password; }
			set 
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogInButton);
			}
		}

		public bool CanLogInButton
		{
			get
			{
				bool output = false;
				if (_userName?.Length > 0 && _password?.Length > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public void LogInButton()
		{
			Console.WriteLine();
		}
	}
}
