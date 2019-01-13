using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class AddDestinationsViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;
        private readonly IApiService apiService;

        private string cityName;
        public string CityName { get => cityName; set => Set(ref cityName, value); }

        private Pushpin latLon;
        public Pushpin LatLon { get => latLon; set => Set(ref latLon, value); }
        
        private ObservableCollection<City> destinations;
        public ObservableCollection<City> Destinations { get => destinations; set => Set(ref destinations, value); }

        public AddDestinationsViewModel(INavigationService navigation, AppDbContext db, IApiService apiService)
        {
            this.navigation = navigation;
            this.db = db;
            this.apiService = apiService;
            Destinations = new ObservableCollection<City>();
        }
        
        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        var NewCity = apiService.GetCity(CityName);
                        if (NewCity!=null)
                        {
                            LatLon = new Pushpin();
                            LatLon.Location=new Location();
                            double lat= double.Parse(NewCity.Latitude);
                            double lon= double.Parse(NewCity.Longitude);
                            LatLon.Location.Latitude = lat;
                            LatLon.Location.Longitude = lon;
                            db.Cities.Add(NewCity);
                            db.SaveChanges();
                            Destinations.Add(NewCity);
                        }
                        
                        //CityName = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new CityListAddedMessage { NewCityList = Destinations });
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new RelayCommand(
                () =>
                {
                    CityName = "";
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
