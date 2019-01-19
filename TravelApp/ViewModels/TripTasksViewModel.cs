using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class TripTasksViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private Trip selectedTrip;
        public Trip SelectedTrip { get => selectedTrip; set => Set(ref selectedTrip, value); }

        private ICollection<TaskList> taskListView=new ObservableCollection<TaskList>();
        public ICollection<TaskList> TaskListView { get=> taskListView; set=>Set(ref taskListView, value); }

        public TripTasksViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
            {
                SelectedTrip = (db.Trips.FirstOrDefault(x => x.Id == msg.Trip.Id));
                TaskListView = new ObservableCollection<TaskList>(SelectedTrip.TaskList);
            },true);
        }

        private RelayCommand taskOkCommand;
        public RelayCommand TaskOkCommand
        {
            get => taskOkCommand ?? (taskOkCommand = new RelayCommand(
            () =>
            {
                SelectedTrip.TaskList =new ObservableCollection<TaskList>(TaskListView);
                db.SaveChanges();
                navigation.Navigate<ReviewTripViewModel>();
            }
            ));
        }

        private RelayCommand taskCancelCommand;
        public RelayCommand TaskCancelCommand
        {
            get => taskCancelCommand ?? (taskCancelCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<ReviewTripViewModel>();
                }
                ));
        }
    }
}
