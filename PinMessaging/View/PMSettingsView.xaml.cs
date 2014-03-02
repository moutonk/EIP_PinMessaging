using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Controller;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMSettings : PhoneApplicationPage
    {
        public PMSettings()
        {
            InitializeComponent();
        }

        private void ModifyPwdButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Utils.Utils.PasswordSyntaxCheck(NewPwdPasswordBox.Password) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangePassword);
                sc.ChangePassword(OldPwdPasswordBox.Password, NewPwdPasswordBox.Password);       
            }
        }

        private void ModifyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmailChecker.IsEmailValid(NewEmailTextBox.Text) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangeEmail);
                sc.ChangeEmail(NewEmailTextBox.Text);
            }
        }
    }
}