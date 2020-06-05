using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Helpers;
using RMDesktopUI.Library.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName;
		private string _password;
		private IAPIHelper _apiHelper;
		private IEventAggregator _events;
		private bool _IsErrorVisible;
		private string _errorMessage;

		public LoginViewModel(IAPIHelper aPIHelper, IEventAggregator events)
		{
			_apiHelper = aPIHelper;
			_events = events;
		}

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

		

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set 
			{ 
				_errorMessage = value;
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}


		public bool IsErrorVisible
		{
			get 
			{
				bool output = false;
				if (ErrorMessage?.Length > 0)
				{
					output = true;
				}
				return output;
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

		public async Task LogInButton()
		{
			try
			{
				ErrorMessage = "";
				var result = await _apiHelper.Authenticate(UserName, Password);
				await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
				_events.PublishOnUIThread(new LogInEventModel());
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
			
		}
	}
}
