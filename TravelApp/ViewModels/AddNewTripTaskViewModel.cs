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

        private string taskName;
        public string TaskName { get => taskName; set => Set(ref taskName, value); }

        private ICollection<Task> taskListView=new ObservableCollection<Task>();
        public ICollection<Task> TaskListView { get => taskListView; set => Set(ref taskListView, value); }

        private ICollection<TaskList> taskList=new ObservableCollection<TaskList>();
        public ICollection<TaskList> TaskList { get => taskList; set => Set(ref taskList, value); }

        public AddNewTripTaskViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
        }

        private RelayCommand addTaskCommand;
        public RelayCommand AddTaskCommand
        {
            get => addTaskCommand ?? (addTaskCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        
                        var NewTask= db.Tasks.FirstOrDefault(x => x.TaskName == TaskName);
                        if (NewTask.TaskName == null)
                        {
                            db.Tasks.Add(new Task { TaskName = TaskName });
                            db.SaveChanges();
                            NewTask= db.Tasks.FirstOrDefault(x => x.TaskName == TaskName);
                        }                      
                        TaskListView.Add(NewTask);
                        TaskList.Add(new TaskList {TaskId = db.Tasks.First(x => x.TaskName == TaskName).Id });
                        TaskName = "";
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
                    Messenger.Default.Send(new TaskListAddedMessage { NewTaskList = TaskList });
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
                    TaskName = "";
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
