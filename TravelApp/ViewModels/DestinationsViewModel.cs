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
    class DestinationsViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private ICollection<DestinationList> cityListView=new ObservableCollection<DestinationList>();
        public ICollection<DestinationList> CityListView { get => cityListView; set => Set(ref cityListView, value); }

        private ICollection<DestinationList> selectedTripDestinations=new ObservableCollection<DestinationList>();
        public ICollection<DestinationList> SelectedTripDestinations { get => selectedTripDestinations; set => Set(ref selectedTripDestinations, value); }

        public DestinationsViewModel(INavigationService navigation, AppDbContext db )
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
             {
                 SelectedTripDestinations = db.Trips.FirstOrDefault(x=>x.Id==msg.Trip.Id).Destinations;
                 CityListView = new ObservableCollection<DestinationList>(SelectedTripDestinations);
             },true);
        }

        private RelayCommand<DestinationList> deleteCityCommand;
        public RelayCommand<DestinationList> DeleteCityCommand
        {
            get => deleteCityCommand ?? (deleteCityCommand = new RelayCommand<DestinationList>(
                param =>
                {
                    CityListView.Remove(param);
                    SelectedTripDestinations.Remove(param);
                    db.SaveChanges();
                }
            ));
        }
    }
}
