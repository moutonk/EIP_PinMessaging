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
            if (PasswordSyntaxCheck() == true)
            {
                var sc = new PMSettingsController(RequestType.ChangePassword);
                sc.ChangePassword(OldPwdPasswordBox.Password, NewPwdPasswordBox.Password);       
            }
        }

        //to factorize with PMLogInCreate....
        private bool PasswordSyntaxCheck()
        {
            if (NewPwdPasswordBox.Password.Length < 6 || NewPwdPasswordBox.Password.Length > 20)
            {
                MessageBox.Show("wrong pwd");
                return false;
            }
            return true;
        }
    }
}