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

        private Trip selectedTrip=new Trip();
        public Trip SelectedTrip { get => selectedTrip; set => Set(ref selectedTrip, value); }

        public ReviewTripViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
            {
                SelectedTrip = db.Trips.First(x=>x.Id==msg.Trip.Id);
            });
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

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<TripBoardViewModel>();
                }
                ));
        }
        private RelayCommand routeMapReviewCommand;
        public RelayCommand RouteMapReviewCommand
        {
            get => routeMapReviewCommand ?? (routeMapReviewCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new MapReviewMessage() { Destinations = SelectedTrip.Destinations });
                    navigation.Navigate<RouteMapViewModel>();
                }
                ));
        }
        
    }
}
