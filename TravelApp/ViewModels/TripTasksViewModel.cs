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
            Messenger.Default.Register<TaskListReviewMessage>(this, msg =>
            {
                TaskListView = new ObservableCollection<TaskList>();
                SelectedTrip = db.Trips.FirstOrDefault(x => x.Id == msg.TripId);
                if (SelectedTrip.TaskList != null)
                {
                    foreach (var item in SelectedTrip.TaskList)
                    {
                        TaskListView.Add(new TaskList { TaskName = item.TaskName, Status = item.Status });
                    }
                }
            }, true);
        }

        private RelayCommand taskOkCommand;
        public RelayCommand TaskOkCommand
        {
            get => taskOkCommand ?? (taskOkCommand = new RelayCommand(
            () =>
            {
                SelectedTrip.TaskList = new ObservableCollection<TaskList>();
                foreach (var item in TaskListView)
                {
                    SelectedTrip.TaskList.Add(new TaskList
                    {
                        TaskName = item.TaskName,
                        Status = item.Status
                    });
                }
                db.SaveChanges();
                TaskListView.Clear();
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
