using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class AddDestinationsViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private City city;
        public City City { get => city; set => Set(ref city, value); }

        private ObservableCollection<City> destionations;
        public ObservableCollection<City> Destionations { get => destionations; set => Set(ref destionations, value); }

        public AddDestinationsViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            //Destionations = new ObservableCollection<City>(db.Destionations);
        }

        private RelayCommand addDestionationCommand;
        public RelayCommand AddDestionationCommand
        {
            get => addDestionationCommand ?? (addDestionationCommand = new RelayCommand(
                () =>
                {

                }
            ));
        }
    }
}
