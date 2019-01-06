using Autofac;
using Autofac.Configuration;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Services;
using TravelApp.ViewModels;

namespace TravelApp
{
    class ViewModelLocator
    {
        private AppViewModel appViewModel;
        private StartPageViewModel startPageViewModel;
        private SignUpViewModel signUpViewModel;
        private TripBoardViewModel tripBoardViewModel;
        private AddNewTripViewModel addNewTripViewModel;
        private AddDestinationsViewModel addDestinationsViewModel;

        private INavigationService navigationService;
        public static IContainer Container;

        public ViewModelLocator()
        {
            try
            {
                var config = new ConfigurationBuilder();
                config.AddJsonFile("autofac.json");
                var module = new ConfigurationModule(config.Build());
                var builder = new ContainerBuilder();
                builder.RegisterModule(module);
                Container = builder.Build();

                navigationService = Container.Resolve<INavigationService>();
                appViewModel = Container.Resolve<AppViewModel>();
                startPageViewModel = Container.Resolve<StartPageViewModel>();
                signUpViewModel = Container.Resolve<SignUpViewModel>();
                tripBoardViewModel = Container.Resolve<TripBoardViewModel>();
                addNewTripViewModel= Container.Resolve<AddNewTripViewModel>();
                addDestinationsViewModel = Container.Resolve<AddDestinationsViewModel>();

                navigationService.Register<StartPageViewModel>(startPageViewModel);
                navigationService.Register<SignUpViewModel>(signUpViewModel);
                navigationService.Register<TripBoardViewModel>(tripBoardViewModel);
                navigationService.Register<AddNewTripViewModel>(addNewTripViewModel);
                navigationService.Register<AddDestinationsViewModel>(addDestinationsViewModel);
                

                navigationService.Navigate<StartPageViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ViewModelBase GetAppViewModel()
        {
            return appViewModel;
        }
    }
}
