using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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

        //private City city;
        //public City City { get => city; set => Set(ref city, value); }

        private string cityName;
        public string CityName { get => cityName; set => Set(ref cityName, value); }

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
                        WebClient webClient = new WebClient();
                        var query = webClient.DownloadString($"http://open.mapquestapi.com/geocoding/v1/address?key=dp0ZzMx1Za1461WOtG1KE8emvuSexkvL&location={CityName}");
                        var result = JsonConvert.DeserializeObject(query) as JObject;
                        var _NewCity = new City();
                        _NewCity.Country=result["results"][0]["locations"][0]["adminArea1"].ToString();
                        _NewCity.CityName = CityName;
                        _NewCity.Coordinates.Latitude = double.Parse(result["results"][0]["locations"][0]["latLng"]["lat"].ToString());
                        _NewCity.Coordinates.Longitude = double.Parse(result["results"][0]["locations"][0]["latLng"]["lng"].ToString());
                        CityName = "";
                        Messenger.Default.Send(new DestinationAddedMessage { NewCity = _NewCity }  );
                        Destionations.Add(_NewCity);
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
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
