using GalaSoft.MvvmLight;
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
    class DestinationsViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private ICollection<DestinationList> selectedTripDestinations;
        public ICollection<DestinationList> SelectedTripDestinations { get => selectedTripDestinations; set => Set(ref selectedTripDestinations, value); }

        public DestinationsViewModel(INavigationService navigation, AppDbContext db )
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
             {
                 SelectedTripDestinations = new ObservableCollection<DestinationList>(msg.Trip.Destinations);
             });
        }
    }
}
