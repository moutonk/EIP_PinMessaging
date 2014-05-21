using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Other;
using PinMessaging.Utils;

namespace PinMessaging.View
{
    public partial class PMUserProfil : PhoneApplicationPage
    {
        public PMUserProfil()
        {
            InitializeComponent();

            if (PMData.User != null)
            {
                PointsTextBlock.Text = PMData.User.Points;
                NbrMsgTextBlock.Text = PMData.User.NbrMessage;
                NbrPinTextBlock.Text = PMData.User.NbrPin;
                LoginTextBlock.Text = PMData.User.Login;
                GradeTextBlock.Text = PMData.User.Grade;
            }
        }

        private void AddAsFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            Logs.Output.ShowOutput("ADD FAV");
        }
    }   
}