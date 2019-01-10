using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        private ObservableCollection<City> destionations;
        public ObservableCollection<City> Destionations { get => destionations; set => Set(ref destionations, value); }

        public AddDestinationsViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            Destionations = new ObservableCollection<City>(db.Destionations);
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
                        var query = webClient.DownloadString($"http://open.mapquestapi.com/geocoding/v1/address?key=dp0ZzMx1Za1461WOtG1KE8emvuSexkvL&location={cityName}");
                        var result = JsonConvert.DeserializeObject(query) as JObject;
                        var NewCity = new City();
                        NewCity.Country=result["results"]["locations"]["adminArea1"].ToString();
                        NewCity.CityName = CityName;
                        NewCity.Coordinates.Latitude =Double.Parse(result["results"]["locations"]["latLng"]["lat"].ToString());
                        NewCity.Coordinates.Longitude = Double.Parse(result["results"]["locations"]["latLng"]["lng"].ToString());
                        CityName = "";
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            ));
        }
    }
}
