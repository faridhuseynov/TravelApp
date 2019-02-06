using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
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
    class MapLocation
    {
        public Location Coordinates { get; set; }
    }

    class RouteMapViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        public ObservableCollection<MapLocation> Locations { get; set; } = new ObservableCollection<MapLocation>();

        public RouteMapViewModel(INavigationService navigation,AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<MapReviewMessage>(this,msg=>
            {
                var TripDestinations = db.Trips.FirstOrDefault(x => x.Id == msg.TripId).Destinations;
                if (TripDestinations != null)
                {
                    foreach (var item in TripDestinations)
                    {
                        Locations.Add(new MapLocation
                        {
                            Coordinates = new Location
                            {
                                Latitude = double.Parse(item.Latitude),
                                Longitude = double.Parse(item.Longitude)
                            }
                        }
                        );
                    }
                }
            },true);
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    Locations.Clear();
                    navigation.Navigate<ReviewTripViewModel>();
                }
            ));
        }
    }
}
