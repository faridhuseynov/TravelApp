﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelApp.Services
{
    class MessageService : IMessageService
    {
        public bool ShowYesNo(string text, string title = "Your choice")
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void ShowError(string text, string title = "Error")
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
