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
    class AddNewTripViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private Trip newTrip=new Trip();
        public Trip NewTrip { get => newTrip; set => Set(ref newTrip, value); }
        
        private ObservableCollection<City> destionations;
        public ObservableCollection<City> Destionations { get => destionations; set => Set(ref destionations, value); }

        public AddNewTripViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<UserLoggedInOrRegisteredMessage>(this, msg =>
            {
                db.LoggedInUser = msg.UserId;
                db.SaveChanges();
            });
            //Destionations = new ObservableCollection<City>(db.Destionations.Where(x => x.TripId == LoggedInUser));
        }

        private RelayCommand addNewDestionationsCommand;
        public RelayCommand AddNewDestionationsCommand
        {
            get => addNewDestionationsCommand ?? (addNewDestionationsCommand = new RelayCommand(
                () =>
                {

                    navigation.Navigate<AddDestinationsViewModel>();
                }
            ));
        }
        void ClearData()
        {
            NewTrip.TripName = "";
            NewTrip.Arrival = NewTrip.Departure= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }
        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    NewTrip.UserId = db.LoggedInUser;
                    db.Trips.Add(NewTrip);
                    db.SaveChanges();
                    Messenger.Default.Send(new NewTripAddedMessage { Item = NewTrip });
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }
    }
}
