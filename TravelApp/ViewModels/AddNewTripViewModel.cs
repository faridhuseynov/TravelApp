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
    class AddNewTripViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        //private Trip newTrip=new Trip();
        //public Trip NewTrip { get => newTrip; set => Set(ref newTrip, value); }

        private string tripName;
        public string TripName { get => tripName; set => Set(ref tripName, value); }

        private DateTime departure = DateTime.Now;
        public DateTime Departure { get => departure; set => Set(ref departure, value); }

        private DateTime arrival = DateTime.Now;
        public DateTime Arrival { get => arrival; set => Set(ref arrival, value); }

        private ICollection<DestinationList> cities;
        public ICollection<DestinationList> Cities { get => cities; set => Set(ref cities, value); }

        private ICollection<TaskList> tasks;
        public ICollection<TaskList> Tasks { get => tasks; set => Set(ref tasks, value); }

        public AddNewTripViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<UserLoggedInOrOutOrRegistered>(this, msg =>
            {
                db.LoggedInUser = msg.UserId;
                db.SaveChanges();
            });
            Messenger.Default.Register<DestinationListAddedMessage>(this, msg =>
            {
                Cities = msg.NewCityList;
            },true);

            Messenger.Default.Register<TaskListAddedMessage>(this, msg =>
            {
                Tasks = msg.NewTaskList;
                MessageBox.Show("Added");
            }, true);
        }


        void ClearData(Trip NewTrip)
        {
            NewTrip.TripName = "";
            NewTrip.Arrival = NewTrip.Departure = DateTime.Now ;
        }
        private RelayCommand cancelNewTripCommand;
        public RelayCommand CancelNewTripCommand
        {
            get => cancelNewTripCommand ?? (cancelNewTripCommand = new RelayCommand(
                () =>
                {
                    TripName = "";
                    Cities.Clear();
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get => okCommand ?? (okCommand = new RelayCommand(
                () =>
                {
                    var NewTrip = new Trip();
                    NewTrip.UserId = db.LoggedInUser;
                    NewTrip.Arrival = Arrival;
                    NewTrip.Departure = Departure;
                    NewTrip.TripName = TripName;
                    TripName = "";
                    //new ICollection<City>
                    NewTrip.Destinations = Cities;
                    NewTrip.TaskList = Tasks;
                    db.Trips.Add(NewTrip);
                    db.SaveChanges();
                    Messenger.Default.Send(new NewTripAddedMessage
                    {
                        Item = new Trip()
                        {
                            TripName = NewTrip.TripName,
                            TaskList = NewTrip.TaskList,
                            Arrival = NewTrip.Arrival,
                            Departure = NewTrip.Departure,
                            Destinations = NewTrip.Destinations,
                            Id = NewTrip.Id,
                            UserId = NewTrip.UserId
                        }
                    });
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }
        private RelayCommand addCityCommand;
        public RelayCommand AddCityCommand
        {
            get => addCityCommand ?? (addCityCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<AddDestinationsViewModel>();
                }
            ));
        }

        private RelayCommand addTaskCommand;
        public RelayCommand AddTaskCommand
        {
            get => addTaskCommand ?? (addTaskCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<AddNewTripTaskViewModel>();
                }
           ));
        }
    }

}
