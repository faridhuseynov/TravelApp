using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class TripBoardViewModel:ViewModelBase
    {
        private User loggedInUser;
        public User LoggedInUser { get => loggedInUser; set => Set(ref loggedInUser, value); }

        private ObservableCollection<Trip> trips;
        public ObservableCollection<Trip> Trips { get => trips; set => Set(ref trips, value); }

        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        public TripBoardViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.User == LoggedInUser));
        }
    }
}
