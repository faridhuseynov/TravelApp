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
    class TripBoardViewModel:ViewModelBase
    {
        private int loggedInUser;
        public int LoggedInUser { get => loggedInUser; set => Set(ref loggedInUser, value); }

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
               loggedInUser= msg.UserId;
            });
            Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.UserId == LoggedInUser));
            Messenger.Default.Register<NewTripAddedMessage>(this, msg => {db.Trips.Add(msg.Item);});
        }

        private RelayCommand logOutCommand;
        public RelayCommand LogOutCommand
        {
            get => logOutCommand ?? (logOutCommand = new RelayCommand(
                () =>
                {
                    LoggedInUser = 0;
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
