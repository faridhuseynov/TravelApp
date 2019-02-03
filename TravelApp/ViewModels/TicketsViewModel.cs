using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
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

            //Messenger.Default.Register<>  this to be continued. DialogBox successfully created
        }

        private RelayCommand addTicketCommand;
        public RelayCommand AddTicketCommand
        {
            get => addTicketCommand ?? (addTicketCommand = new RelayCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.DefaultExt = ".pdf";
                    fileDialog.Filter = "PDF documents (.pdf)|*.pdf";
                    fileDialog.InitialDirectory = @"C:\Users\Farid\Desktop";
                    fileDialog.ShowDialog();
                    if (fileDialog!=null)
                    {

                    }

                }
            ));
        }
    }
}
