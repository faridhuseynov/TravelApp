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
    class TripBoardViewModel : ViewModelBase
    {
        private ObservableCollection<Trip> trips= new ObservableCollection<Trip>();
        public ObservableCollection<Trip> Trips { get => trips; set => Set(ref trips, value); }

        private readonly INavigationService navigation;
        private readonly AppDbContext db;
        private readonly IMessageService messageService;

        public TripBoardViewModel(INavigationService navigation, AppDbContext db,IMessageService messageService)
        {
            this.navigation = navigation;
            this.db = db;
            this.messageService = messageService;

            Messenger.Default.Register<UserLoggedInOrOutOrRegistered>(this, msg =>
            {
                db.LoggedInUser = msg.UserId;
                db.SaveChanges();
                Trips = new ObservableCollection<Trip>(db.Trips.Where(x => x.UserId == db.LoggedInUser));
            });

            Messenger.Default.Register<NewTripAddedMessage>(this, msg =>
            {
                var newtrip =db.Trips.FirstOrDefault(x => x.Id == msg.Item.Id);
                Trips.Add(newtrip);
            },true);
        }

        private RelayCommand logOutCommand;
        public RelayCommand LogOutCommand
        {
            get => logOutCommand ?? (logOutCommand = new RelayCommand(
                () =>
                {
                    //if (db.Users.FirstOrDefault(x => x.Id == db.LoggedInUser).Trips != null)
                    //{
                    //    db.LoggedInUser = 0;
                    //    db.SaveChanges();
                    //}
                    //Messenger.Default.Send(new UserLoggedInOrOutOrRegistered { UserId = db.LoggedInUser });
                    navigation.Navigate<StartPageViewModel>();
                }
            ));
        }

        private RelayCommand addNewTripCommand;
        public RelayCommand AddNewTripCommand
        {
            get => addNewTripCommand ?? (addNewTripCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }

        private RelayCommand<Trip> reviewTripCommand;
        public RelayCommand<Trip> ReviewTripCommand
        {
            get => reviewTripCommand ?? (reviewTripCommand = new RelayCommand<Trip>(
                param =>
                {
                    Messenger.Default.Send(new TripSelectedMessage(){ TripId = param.Id });
                    navigation.Navigate<ReviewTripViewModel>();
                }
            ));
        }

        private RelayCommand<Trip> deleteTripCommand;
        public RelayCommand<Trip> DeleteTripCommand
        {
            get => deleteTripCommand ?? (deleteTripCommand = new RelayCommand<Trip>(
                param =>
                {
                    if (messageService.ShowYesNo($"Are you sure you want to delete trip {param.TripName}?"))
                    {
                        Trips.Remove(param);
                        var item = db.Trips.Where(x => x.Id == param.Id).FirstOrDefault();
                        item.Destinations.Clear();
                        item.Tickets.Clear();
                        item.TaskList.Clear();
                        db.SaveChanges();
                        db.Trips.Remove(param);
                        db.SaveChanges();
                    }
                }
            ));
        }
    }
}
