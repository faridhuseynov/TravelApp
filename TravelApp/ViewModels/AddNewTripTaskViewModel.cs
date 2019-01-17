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

        private ICollection<Task> taskListView=new ObservableCollection<Task>();
        public ICollection<Task> TaskListView { get => taskListView; set => Set(ref taskListView, value); }

        private ICollection<TaskList> taskList=new ObservableCollection<TaskList>();
        public ICollection<TaskList> TaskList { get => taskList; set => Set(ref taskList, value); }

        public AddNewTripTaskViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
            Messenger.Default.Register<NewTripAddedMessage>(this, msg =>
            {
                TaskList.Clear();
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
                        
                        var NewTask= db.Tasks.FirstOrDefault(x => x.TaskName == NewTaskName);
                        if (NewTask == null)
                        {
                            db.Tasks.Add(new Task { TaskName = NewTaskName });
                            db.SaveChanges();
                            NewTask= db.Tasks.FirstOrDefault(x => x.TaskName == NewTaskName);
                        }                      
                        TaskListView.Add(NewTask);
                        TaskList.Add(new TaskList {TaskId = db.Tasks.First(x => x.TaskName == NewTaskName).Id, TaskName=NewTaskName });
                        NewTaskName = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            ));
        }

        private RelayCommand<Task> deleteTaskCommand;
        public RelayCommand<Task> DeleteTaskCommand
        {
            get => deleteTaskCommand ?? (deleteTaskCommand = new RelayCommand<Task>(
                param =>
                {
                    TaskListView.Remove(param);
                    var deleteTask = TaskList.First(x => x.TaskId == param.Id);
                    TaskList.Remove(deleteTask);
                }
            ));
        }

        private RelayCommand taskOkCommand;
        public RelayCommand TaskOkCommand
        {
            get => taskOkCommand ?? (taskOkCommand = new RelayCommand(
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
                    NewTaskName = "";
                    TaskList.Clear();
                    TaskListView.Clear();
                    navigation.Navigate<AddNewTripViewModel>();
                }
            ));
        }
    }
}
