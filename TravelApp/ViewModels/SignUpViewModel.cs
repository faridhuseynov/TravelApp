using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class SignUpViewModel : ViewModelBase
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;

        private User newUser = new User();
        public User NewUser { get => newUser; set => Set(ref newUser, value); }

        void UserDataClear()
        {
            NewUser.Email = NewUser.Name = NewUser.PhotoLink = NewUser.Surname = NewUser.UserName = "";
        }

        public SignUpViewModel(INavigationService navigation, AppDbContext db)
        {
            this.navigation = navigation;
            this.db = db;
        }

        private RelayCommand<PasswordBox> registerCommand;
        public RelayCommand<PasswordBox> RegisterCommand
        {
            get => registerCommand ?? (registerCommand = new RelayCommand<PasswordBox>(
                param =>
                {
                    RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                    byte[] salt = new byte[32];
                    csprng.GetBytes(salt);
                    // Get the salt value
                    NewUser.SaltValue = Convert.ToBase64String(salt);
                    // Salt the password
                    byte[] saltedPassword = Encoding.UTF8.GetBytes(NewUser.SaltValue + param.Password);
                    // Hash the salted password using SHA256
                    SHA256Managed hashstring = new SHA256Managed();
                    byte[] hash = hashstring.ComputeHash(saltedPassword);
                    // Save both the salt and the hash in the user's database record.
                    NewUser.SaltValue = Convert.ToBase64String(salt);
                    NewUser.HashValue = Convert.ToBase64String(hash);
                    db.Users.Add(NewUser);
                    db.LoggedInUser = NewUser.Id;
                    db.SaveChanges();
                    Messenger.Default.Send(new UserLoggedInOrOutOrRegistered { UserId = NewUser.Id });
                    UserDataClear();
                    navigation.Navigate<TripBoardViewModel>();
                }
            ));
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new RelayCommand(
                () =>
                {
                    UserDataClear();
                    navigation.Navigate<StartPageViewModel>();
                }
            ));
        }
    }
}
