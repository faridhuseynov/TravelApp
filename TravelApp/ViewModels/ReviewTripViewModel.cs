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
        private readonly IMessageService message;

        private Trip selectedTrip=new Trip();
        public Trip SelectedTrip { get => selectedTrip; set => Set(ref selectedTrip, value); }

        public ReviewTripViewModel(INavigationService navigation, AppDbContext db, IMessageService message)
        {
            this.navigation = navigation;
            this.db = db;
            this.message = message;
            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
            {
                SelectedTrip = db.Trips.FirstOrDefault(x => x.Id == msg.TripId);
            },true);
        }

        private RelayCommand checkListViewCommand;
        public RelayCommand CheckListViewCommand
        {
            get => checkListViewCommand ?? (checkListViewCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new TaskListReviewMessage { TripId = SelectedTrip.Id });
                    navigation.Navigate<TripTasksViewModel>();
                }
                ));            
        }

        private RelayCommand ticketsViewCommand;
        public RelayCommand TicketsViewCommand
        {
            get => ticketsViewCommand ?? (ticketsViewCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new TicketsReviewMessage  { TripId = SelectedTrip.Id });
                    navigation.Navigate<TicketsViewModel>();
                }
                ));
        }

        private RelayCommand citiesViewCommand;
        public RelayCommand CitiesViewCommand
        {
            get => citiesViewCommand ?? (citiesViewCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new DestinationsReviewMessage { TripId = SelectedTrip.Id });
                    navigation.Navigate<DestinationsViewModel>();
                }
                ));
        }

        private RelayCommand bookingsViewCommand;
        public RelayCommand BookingsViewCommand
        {
            get => bookingsViewCommand ?? (bookingsViewCommand = new RelayCommand(
                () =>
                {
                    message.ShowYesNo("Had no desire to do this one, will you give me low grade because of this? 😊");
                }
                ));
        }

        private RelayCommand routeMapReviewCommand;
        public RelayCommand RouteMapReviewCommand
        {
            get => routeMapReviewCommand ?? (routeMapReviewCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new MapReviewMessage() { TripId=SelectedTrip.Id });
                    navigation.Navigate<RouteMapViewModel>();
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

    }
}
