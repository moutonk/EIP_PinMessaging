﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMSettings : PhoneApplicationPage
    {
        private class Language
        {
            public string Name { get; set; }
            public BitmapImage Image { get; set; }
            public PMFlagModel.FlagsType Type { get; set; }
        }

        public PMSettings()
        {
            InitializeComponent();

            if (RememberConnection.GetLoginPwd() != null)
                CurrentEmailTextBlock.Text = RememberConnection.GetLoginPwd().Email;

            LanguageListPicker.Items.Add(new  Language { Name = "Français", Image = new BitmapImage(Paths.FlagFR), Type = PMFlagModel.FlagsType.FR });
            LanguageListPicker.Items.Add(new Language { Name = "English", Image = new BitmapImage(Paths.FlagEN), Type = PMFlagModel.FlagsType.EN });
          
            //select the correct language regardings the telephone language
            LanguageListPicker.SelectedIndex = Thread.CurrentThread.CurrentUICulture.Name.Equals("fr-FR") ? 0 : 1;
        }

        private void ModifyPwdButton_OnClick(object sender, RoutedEventArgs e)
        {
            //AppResources.pa
            if (Utils.Utils.PasswordSyntaxCheck(NewPwdPasswordBox.Password) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangePassword);
                sc.ChangePassword(OldPwdPasswordBox.Password, NewPwdPasswordBox.Password);       
            }
        }

        private void ModifyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Utils.IsEmailValid(NewEmailTextBox.Text) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangeEmail);
                sc.ChangeEmail(NewEmailTextBox.Text);
            }
        }

        private void PivotPins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OldPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            OldPwdTextBlock.Visibility = (OldPwdPasswordBox.Password.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void NewPwdConfirmPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewPwdConfirmTextBlock.Visibility = (NewPwdConfirmPasswordBox.Password.Length != 0 ? Visibility.Collapsed : Visibility.Visible);

        }

        private void NewPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewPwdTextBlock.Visibility = (NewPwdPasswordBox.Password.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void NewEmailTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewEmailTextBlock.Visibility = (NewEmailTextBox.Text.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void ChangeLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);

            App.RootFrame.Language = XmlLanguage.GetLanguage(lang);
            App.RootFrame.FlowDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
            App.Current.RootVisual.UpdateLayout();
            App.RootFrame.UpdateLayout();

            //reload UI
            var reloadUri = (App.RootFrame.Content as PhoneApplicationPage).NavigationService.CurrentSource;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(reloadUri + "?no-cache=" + Guid.NewGuid(), UriKind.Relative));

            //remove settings and map
            NavigationService.RemoveBackEntry();
            NavigationService.RemoveBackEntry();
        }

        private void LanguageListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((((LanguageListPicker).SelectedItem) as Language).Type)
            {
                case PMFlagModel.FlagsType.EN:
                    ChangeLanguage("en-US");
                    break;

                case PMFlagModel.FlagsType.FR:
                    ChangeLanguage("fr-FR");
                    break;
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            PMData.PinsList.Clear();
            NavigationService.Navigate(Paths.MapView);
            e.Cancel = false;
        }

        private void LanguageListPicker_OnLoaded(object sender, RoutedEventArgs e)
        {
            LanguageListPicker.SelectionChanged += LanguageListPicker_OnSelectionChanged;
        }

        private void ModifyPasswordBox_OnTextInput(object sender, RoutedEventArgs routedEventArgs)
        {
            if (NewPwdPasswordBox.Password.Length == 0 || OldPwdPasswordBox.Password.Length == 0 || NewPwdConfirmPasswordBox.Password.Length == 0)
                ModifyPwdButton.IsEnabled = false;
            else
                ModifyPwdButton.IsEnabled = true;
        }

        private void NewEmailTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewEmailTextBox.Text.Length == 0)
                ModifyEmailButton.IsEnabled = false;
            else
                ModifyEmailButton.IsEnabled = true;
        }
    }
}