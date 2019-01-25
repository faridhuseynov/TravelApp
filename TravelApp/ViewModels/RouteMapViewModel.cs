using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class RouteMapViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private ICollection<Location> locations;
        public ICollection<Location> Locations { get => locations; set => Set(ref locations, value); }


        public RouteMapViewModel(INavigationService navigation,AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<MapReviewMessage>(this,msg=>
            {
                foreach (var item in msg.Destinations)
                {
                    Locations.Add(new Location
                    {
                        Latitude = double.Parse(item.Latitude),
                        Longitude = double.Parse(item.Longitude)
                    });
                }
            });
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {                    
                    navigation.Navigate<ReviewTripViewModel>();
                }
            ));
        }
    }
}
