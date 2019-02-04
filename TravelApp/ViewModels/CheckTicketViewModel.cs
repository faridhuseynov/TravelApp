using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Messages;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    public class CheckTicketViewModel:ViewModelBase
    {
        private readonly INavigationService navigation;

        private Uri ticketSource;
        public Uri TicketSource { get => ticketSource; set => Set(ref ticketSource, value); }


        public CheckTicketViewModel(INavigationService navigation)
        {
            
            this.navigation = navigation;
            Messenger.Default.Register<CheckTicketMessage>(this, msg =>
             {
                 TicketSource = new Uri(msg.TicketSource);                 
             },true);
        }

        private RelayCommand reviewTicketOkCommand;
        public RelayCommand ReviewTicketOkCommand
        {
            get => reviewTicketOkCommand ?? (reviewTicketOkCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<TicketsViewModel>();
                }
                ));
        }


    }

    //taken from StackOverflow
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                browser.Source = !String.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
        }

    }
}
