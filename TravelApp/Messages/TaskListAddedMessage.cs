using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Messages
{
    class TaskListAddedMessage
    {
        public ICollection<TaskList> NewTaskList { get; set; } = new ObservableCollection<TaskList>();
    }
}
