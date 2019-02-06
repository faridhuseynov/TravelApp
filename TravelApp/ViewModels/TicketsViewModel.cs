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
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    public class TicketsViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private ICollection<Ticket> ticketList;
        public ICollection<Ticket> TicketList { get => ticketList; set => Set(ref ticketList, value); }

        private Trip selectedTrip;
        public Trip SelectedTrip { get => selectedTrip; set => Set(ref selectedTrip, value); }

        public TicketsViewModel(INavigationService navigation,AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;

            Messenger.Default.Register<TicketsReviewMessage>(this, msg => 
            {
                SelectedTrip = db.Trips.FirstOrDefault(x => x.Id == msg.TripId);
                TicketList = new ObservableCollection<Ticket>();
                if (SelectedTrip.Tickets != null)
                {
                    foreach (var item in SelectedTrip.Tickets)
                    {
                        TicketList.Add(new Ticket
                        {
                            TicketName = item.TicketName,
                            TicketPath = item.TicketPath
                        });
                    }
                }
            },true);
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
                        TicketList.Add(new Ticket { TicketName = fileDialog.SafeFileName, TicketPath = fileDialog.FileName });
                    }                    
                }
            ));
        }

        private RelayCommand<Ticket> deleteTicketCommand;
        public RelayCommand<Ticket> DeleteTicketCommand
        {
            get => deleteTicketCommand ?? (deleteTicketCommand = new RelayCommand<Ticket>(
                param =>
                {
                    TicketList.Remove(param);
                }
            ));
        }

        private RelayCommand<Ticket> checkTicketCommand;
        public RelayCommand<Ticket> CheckTicketCommand
        {
            get => checkTicketCommand ?? (checkTicketCommand = new RelayCommand<Ticket>(
                param =>
                {
                    Messenger.Default.Send(new CheckTicketMessage { TicketSource = param.TicketPath });
                    navigation.Navigate<CheckTicketViewModel>();
                }
            ));
        }

        private RelayCommand okTicketCommand;
        public RelayCommand OkTicketCommand
        {
            get => okTicketCommand ?? (okTicketCommand = new RelayCommand(
                () =>
                {
                    SelectedTrip.Tickets = new ObservableCollection<Ticket>();
                    foreach (var item in TicketList)
                    {
                        SelectedTrip.Tickets.Add(new Ticket
                        {
                            TicketName=item.TicketName,TicketPath=item.TicketPath
                        });
                    }
                    db.SaveChanges();
                    TicketList.Clear();
                    navigation.Navigate<ReviewTripViewModel>();
                }
            ));
        }
    }
}
