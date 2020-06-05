using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Views;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogInEventModel>
    {
        private SimpleContainer _container;

        private IEventAggregator _events;
        private SalesViewModel _salesViewModel;

        public ShellViewModel(IEventAggregator events,
            SalesViewModel salesViewModel, SimpleContainer container)
        {
            _container = container;
            _events = events;
            _salesViewModel = salesViewModel;
            _events.Subscribe(this);
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogInEventModel log)
        {
            ActivateItem(_salesViewModel);
        }

    }
}
