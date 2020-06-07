using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Views;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogInEventModel>
    {

        private IEventAggregator _events;
        private SalesViewModel _salesViewModel;
        private ILoggedInUserModel _user;

        public ShellViewModel(IEventAggregator events,
            SalesViewModel salesViewModel, SimpleContainer container,
            ILoggedInUserModel user)
        {
            _events = events;
            _salesViewModel = salesViewModel;
            _user = user;
            _events.Subscribe(this);
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogInEventModel log)
        {
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    output = true;
                }
                return output;
            }
        }
        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        }

    }
}
