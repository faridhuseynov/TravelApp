using GalaSoft.MvvmLight;
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
    class TicketsViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private ICollection<Ticket> ticketList = new ObservableCollection<Ticket>();
        public ICollection<Ticket> TicketList { get => ticketList; set => Set(ref ticketList, value); }
        

        public TicketsViewModel(INavigationService navigation,AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;


        }
    }
}
