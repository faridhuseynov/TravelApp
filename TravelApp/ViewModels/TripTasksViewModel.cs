using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class TripTasksViewModel:ViewModelBase
    {
        private ICollection<TaskList> selectedTripTasks;
        public ICollection<TaskList> SelectedTripTasks{ get => selectedTripTasks; set => Set(ref selectedTripTasks, value); }

        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        public TripTasksViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<TripSelectedMessage>(this, msg =>
            {
                SelectedTripTasks = new ObservableCollection<TaskList>(msg.Trip.TaskList);
            });
        }
    }
}
