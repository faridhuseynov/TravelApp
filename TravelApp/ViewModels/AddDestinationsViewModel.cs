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

        private string cityName;
        public string CityName { get => cityName; set => Set(ref cityName, value); }

        private City tempCity;
        public City TempCity { get => tempCity; set => Set(ref tempCity, value); }

        private Pushpin pushpin;
        public Pushpin Pushpin { get => pushpin; set => Set(ref pushpin, value); }
        
        private ObservableCollection<City> destionations=new ObservableCollection<City>();
        public ObservableCollection<City> Destionations { get => destionations; set => Set(ref destionations, value); }

        public AddDestinationsViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            
            Destionations = new ObservableCollection<City>();
        }
        
        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        TempCity = new City();
                        using (WebClient webClient = new WebClient())
                        {
                            var query = webClient.DownloadString($"http://open.mapquestapi.com/geocoding/v1/address?key=dp0ZzMx1Za1461WOtG1KE8emvuSexkvL&location={CityName}");
                            var result = JsonConvert.DeserializeObject(query) as JObject;
                            TempCity.Country = result["results"][0]["locations"][0]["adminArea1"].ToString();
                            TempCity.CityName = CityName;
                            TempCity.Coordinates.Latitude = double.Parse(result["results"][0]["locations"][0]["latLng"]["lat"].ToString());
                            TempCity.Coordinates.Longitude = double.Parse(result["results"][0]["locations"][0]["latLng"]["lng"].ToString());
                            Pushpin = new Pushpin();
                            Pushpin.Location = TempCity.Coordinates;
                        }
                        //CityName = "";
                        Destionations.Add(TempCity);
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
                    Messenger.Default.Send(new CityAddedMessage { NewCity = TempCity });
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
