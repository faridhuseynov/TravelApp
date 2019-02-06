using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class DestinationsViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;
        private readonly IApiService apiService;

        private string cityName;
        public string CityName { get => cityName; set => Set(ref cityName, value); }

        private Trip selectedTrip;
        public Trip SelectedTrip { get => selectedTrip; set => Set(ref selectedTrip, value); }

        private ICollection<City> cityView = new ObservableCollection<City>();
        public ICollection<City> CityView { get => cityView; set => Set(ref cityView, value); }

        private ICollection<DestinationList> destinations=new ObservableCollection<DestinationList>();
        public ICollection<DestinationList> Destinations { get => destinations; set => Set(ref destinations, value); }

        public DestinationsViewModel(INavigationService navigation, AppDbContext db, IApiService apiService)
        {
            this.navigation = navigation;
            this.db = db;
            this.apiService = apiService;
            Messenger.Default.Register<DestinationsReviewMessage>(this, msg =>
             {
                 SelectedTrip = db.Trips.FirstOrDefault(x => x.Id == msg.TripId);
                 if (SelectedTrip.Destinations != null)
                 {
                     foreach (var item in SelectedTrip.Destinations)
                     {
                         CityView.Add(new City { CityName = item.CityName, ImagePath = item.ImagePath });
                         Destinations.Add(new DestinationList()
                         {
                             CityId = item.CityId,
                             CityName = item.CityName,
                             Currency = item.Currency,
                             ImagePath = item.ImagePath,
                             Latitude = item.Latitude,
                             Longitude = item.Longitude
                         });
                     }
                 }
             }, true);
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
                            if (NewCity.CityName == null)
                            {
                                MessageBox.Show("Check city name and try again");
                                return;
                            }
                            db.Cities.Add(NewCity);
                            db.SaveChanges();
                        }

                        CityView.Add(new City()
                        {
                            CityName = NewCity.CityName, Country = NewCity.Country, Currency = NewCity.Currency,
                            ImagePath = NewCity.ImagePath, Latitude = NewCity.Latitude, Longitude = NewCity.Longitude
                        });
                        Destinations.Add(new DestinationList()
                        {
                            CityId = db.Cities.FirstOrDefault(x => x.CityName == CityName).Id,
                            CityName = db.Cities.FirstOrDefault(x => x.CityName == CityName).CityName,
                            Currency = db.Cities.FirstOrDefault(x => x.CityName == CityName).Currency,
                            ImagePath = db.Cities.FirstOrDefault(x => x.CityName == CityName).ImagePath,
                            Latitude = db.Cities.FirstOrDefault(x => x.CityName == CityName).Latitude,
                            Longitude = db.Cities.FirstOrDefault(x => x.CityName == CityName).Longitude
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
                    SelectedTrip.Destinations = new ObservableCollection<DestinationList>();
                    foreach (var item in Destinations)
                    {
                        var dest = new DestinationList
                        {
                            CityId = item.CityId,
                            CityName = item.CityName,
                            Currency = item.Currency,
                            ImagePath = item.ImagePath,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude
                        };
                        SelectedTrip.Destinations.Add(dest
                        );                       
                    }
                    db.SaveChanges();
                    Destinations.Clear();
                    CityView.Clear();
                    navigation.Navigate<ReviewTripViewModel>();
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
                    navigation.Navigate<ReviewTripViewModel>();
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
                    var dest = Destinations.First(x => x.CityName == param.CityName);
                    Destinations.Remove(dest);
                }
            ));
        }
    }
}
