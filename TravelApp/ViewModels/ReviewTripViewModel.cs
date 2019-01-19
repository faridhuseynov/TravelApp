using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class ReviewTripViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        public ReviewTripViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;           
        }

        private RelayCommand checkListViewCommand;
        public RelayCommand CheckListViewCommand
        {
            get => checkListViewCommand ?? (checkListViewCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<TripTasksViewModel>();
                }
                ));            
        }

        private RelayCommand citiesViewCommand;
        public RelayCommand CitiesViewCommand
        {
            get => citiesViewCommand ?? (citiesViewCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<DestinationsViewModel>();
                }
                ));
        }

    }
}
