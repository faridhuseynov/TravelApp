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

        private ICollection<City> cityView = new ObservableCollection<City>();
        public ICollection<City> CityView { get => cityView; set => Set(ref cityView, value); }

        private ICollection<DestinationList> destinations= new ObservableCollection<DestinationList>();
        public ICollection<DestinationList> Destinations { get => destinations; set => Set(ref destinations, value); }

        public AddDestinationsViewModel(INavigationService navigation, AppDbContext db, IApiService apiService)
        {
            this.navigation = navigation;
            this.db = db;
            this.apiService = apiService;

            Messenger.Default.Register<NewTripAddedMessage>(this, msg =>
            {
                Destinations.Clear();
                CityView.Clear();
            });
        }
        
        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        var NewCity = db.Cities.FirstOrDefault(x => x.CityName == CityName);
                        if (NewCity == null)
                        {
                            NewCity = apiService.GetCity(CityName);
                            if (NewCity.CityName==null)
                            {
                                MessageBox.Show("Check city name and try again");
                                return;
                            }
                            db.Cities.Add(NewCity);
                            db.SaveChanges();
                        }
                            LatLon = new Pushpin();
                            LatLon.Location=new Location();
                            double lat= double.Parse(NewCity.Latitude);
                            double lon= double.Parse(NewCity.Longitude);
                            LatLon.Location.Latitude = lat;
                            LatLon.Location.Longitude = lon;
                            CityView.Add(NewCity);
                        Destinations.Add(new DestinationList
                        {
                            CityId = db.Cities.First(x => x.CityName == CityName).Id,
                            CityName = db.Cities.First(x => x.CityName == CityName).CityName,
                            Currency = db.Cities.First(x => x.CityName == CityName).Currency,
                            ImagePath = db.Cities.First(x => x.CityName == CityName).ImagePath,
                            Latitude = lat.ToString(),
                            Longitude = lon.ToString()
                        });
                            CityName = "";                                             
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
                    Messenger.Default.Send(new DestinationListAddedMessage
                    {
                        NewCityList = new ObservableCollection<DestinationList>(Destinations)
                    });                   
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
                    Destinations.Clear();
                    CityView.Clear();
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }


        private RelayCommand<City> deleteDestinationCommand;
        public RelayCommand<City> DeleteDestinationCommand
        {
            get => deleteDestinationCommand ?? (deleteDestinationCommand = new RelayCommand<City>(
                param =>
                {
                    CityView.Remove(param);
                    var dest = Destinations.First(x => x.CityId == param.Id);
                    Destinations.Remove(Destinations.First(x => x.CityId == param.Id));
                }
            ));
        }
    }
}
