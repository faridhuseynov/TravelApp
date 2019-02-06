using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Extensions;
using TravelApp.Messages;
using TravelApp.Models;
using TravelApp.Services;

namespace TravelApp.ViewModels
{
    class SignUpViewModel : ViewModelBase,IDataErrorInfo
    {
        private readonly INavigationService navigation;
        private readonly AppDbContext db;
        private readonly IMessageService message;
        private User newUser = new User();
        public User NewUser { get => newUser; set => Set(ref newUser, value); }

        void UserDataClear()
        {
            NewUser.Email = NewUser.Name = NewUser.PhotoLink = NewUser.Surname = NewUser.UserName = "";
        }

        public SignUpViewModel(INavigationService navigation, AppDbContext db, IMessageService message)
        {
            this.navigation = navigation;
            this.db = db;
            this.message = message;
        }

        private RelayCommand<PasswordBox> registerCommand;
        public RelayCommand<PasswordBox> RegisterCommand
        {
            get => registerCommand ?? (registerCommand = new RelayCommand<PasswordBox>(
                param =>
                {
                    if (Regex.IsMatch(NewUser.Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")==true && !String.IsNullOrEmpty(NewUser.Name) && !String.IsNullOrEmpty(NewUser.Surname)&& !String.IsNullOrEmpty(NewUser.UserName)
                && !String.IsNullOrEmpty(param.Password))
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
                        db.SaveChanges();
                        Messenger.Default.Send(new UserLoggedInOrOutOrRegistered { UserId = NewUser.Id });
                        UserDataClear();
                        navigation.Navigate<TripBoardViewModel>();
                    }
                    else
                    {
                        message.ShowError("Please check validation errors");
                    }
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

        public string Error => throw new NotImplementedException();
        public string this[string columnName] => this.Validate(columnName) ;
    }
}
