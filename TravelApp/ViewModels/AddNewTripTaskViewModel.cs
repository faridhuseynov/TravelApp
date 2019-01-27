using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class AddNewTripTaskViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private string newTaskName;
        public string NewTaskName { get => newTaskName; set => Set(ref newTaskName, value); }

        private ICollection<TaskList> taskListView=new ObservableCollection<TaskList>();
        public ICollection<TaskList> TaskListView { get => taskListView; set => Set(ref taskListView, value); }

        public AddNewTripTaskViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<NewTripAddedMessage>(this, msg =>
            {
                TaskListView.Clear();
            });
        }

        private RelayCommand addTaskCommand;
        public RelayCommand AddTaskCommand
        {
            get => addTaskCommand ?? (addTaskCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                                           
                        TaskListView.Add(new TaskList { TaskName = NewTaskName });
                        NewTaskName = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            ));
        }

        private RelayCommand<TaskList> deleteTaskCommand;
        public RelayCommand<TaskList> DeleteTaskCommand
        {
            get => deleteTaskCommand ?? (deleteTaskCommand = new RelayCommand<TaskList>(
                param =>
                {
                    TaskListView.Remove(param);                    
                }
            ));
        }

        private RelayCommand taskOkCommand;
        public RelayCommand TaskOkCommand
        {
            get => taskOkCommand ?? (taskOkCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new TaskListAddedMessage { NewTaskList = new ObservableCollection<TaskList>(TaskListView) });
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
                    NewTaskName = "";
                    TaskListView.Clear();
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
