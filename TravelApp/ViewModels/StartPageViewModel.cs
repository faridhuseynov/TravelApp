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
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private string photoPath = @"C:\Users\Farid\Desktop\Structure.jpg";
        public string PhotoPath { get => photoPath; set => Set(ref photoPath, value); }

        private string checkUsername;
        public string CheckUsername { get => checkUsername; set => Set(ref checkUsername, value); }

        //private string checkPassword;
        //public string CheckPassword { get => checkPassword; set => Set(ref checkPassword, value); }

        public StartPageViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
        }

        private RelayCommand signUpCommand;
        public RelayCommand SignUpCommand
        {
            get => signUpCommand ?? (signUpCommand = new RelayCommand(
                () =>
                {
                    navigation.Navigate<SignUpViewModel>();
                }
            ));
        }

        private RelayCommand<PasswordBox> loginCommand;
        public RelayCommand<PasswordBox> LoginCommand
        {
            get => loginCommand ?? (loginCommand = new RelayCommand<PasswordBox>(
                param =>
                {
                    var check = db.Users.FirstOrDefault(x => x.UserName == checkUsername);
                    if (check != null)
                    {
                        PhotoPath = check.PhotoLink;
                        if (check.Password == param.Password)
                        {
                            db.LoggedInUser = 1;
                            db.SaveChanges();
                            Messenger.Default.Send(new UserLoggedInOrRegisteredMessage { UserId = check.Id });
                            navigation.Navigate<TripBoardViewModel>();
                        }
                        else
                            MessageBox.Show("Password is incorrect!");
                    }
                    else
                        MessageBox.Show("Username is incorrect!");
                }
            ));
        }
    }
}
