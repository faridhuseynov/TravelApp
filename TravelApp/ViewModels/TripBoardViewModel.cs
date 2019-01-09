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
    class TripBoardViewModel : ViewModelBase
    {
        private ObservableCollection<Trip> trips;
        public ObservableCollection<Trip> Trips { get => trips; set => Set(ref trips, value); }

        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        public TripBoardViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<UserLoggedInOrRegisteredMessage>(this, msg =>
            {
                db.LoggedInUser = msg.UserId;
                db.SaveChanges();
                MessageBox.Show("Worked!");
                Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.UserId == db.LoggedInUser));
            });
            Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.UserId == db.LoggedInUser));
            Messenger.Default.Register<NewTripAddedMessage>(this, msg =>
            {
                //Trips.Add(msg.Item);
                Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.UserId == db.LoggedInUser));
                MessageBox.Show("Adding trip Worked!");
            });
        }

        private RelayCommand logOutCommand;
        public RelayCommand LogOutCommand
        {
            get => logOutCommand ?? (logOutCommand = new RelayCommand(
                () =>
                {
                    db.LoggedInUser = 0;
                    db.SaveChanges();
                    Messenger.Default.Send(new UserLoggedInOrRegisteredMessage { UserId = db.LoggedInUser });
                    navigation.Navigate<StartPageViewModel>();
                }
            ));
        }

        private RelayCommand addNewTripCommand;
        public RelayCommand AddNewTripCommand
        {
            get => addNewTripCommand ?? (addNewTripCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
