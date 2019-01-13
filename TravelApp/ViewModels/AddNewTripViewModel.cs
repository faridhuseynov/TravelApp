using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class AddNewTripViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        //private Trip newTrip=new Trip();
        //public Trip NewTrip { get => newTrip; set => Set(ref newTrip, value); }

        private string tripName;
        public string TripName { get => tripName; set => Set(ref tripName, value); }

        private DateTime departure = DateTime.Now;
        public DateTime Departure { get => departure; set => Set(ref departure, value); }

        private DateTime arrival = DateTime.Now;
        public DateTime Arrival { get => arrival; set => Set(ref arrival, value); }

        private ObservableCollection<City> cities;
        public ObservableCollection<City> Cities { get => cities; set => Set(ref cities, value); }

        public AddNewTripViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<UserLoggedInOrOutOrRegistered>(this, msg =>
            {
                db.LoggedInUser = msg.UserId;
                db.SaveChanges();
            });
            Cities = new ObservableCollection<City>();
            Messenger.Default.Register<CityListAddedMessage>(this, msg =>
            {
                Cities = msg.NewCityList;
            },true);
        }


        void ClearData(Trip NewTrip)
        {
            NewTrip.TripName = "";
            NewTrip.Arrival = NewTrip.Departure = DateTime.Now ;
        }
        private RelayCommand cancelNewTripCommand;
        public RelayCommand CancelNewTripCommand
        {
            get => cancelNewTripCommand ?? (cancelNewTripCommand = new RelayCommand(
                () =>
                {
                    TripName = "";
                    Cities = new ObservableCollection<City>();
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    var NewTrip = new Trip();
                    NewTrip.UserId = db.LoggedInUser;
                    NewTrip.Arrival = Arrival;
                    NewTrip.Departure = Departure;
                    NewTrip.TripName = TripName;
                    TripName = "";
                    NewTrip.Cities = Cities;
                    db.Trips.Add(NewTrip);
                    db.SaveChanges();
                    Messenger.Default.Send(new NewTripAddedMessage { Item = NewTrip });
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }
        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<AddDestinationsViewModel>();
                }
            ));
        }
    }

}
