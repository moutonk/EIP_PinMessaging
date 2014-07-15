using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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

            LanguageListPicker.SelectionChanged += LanguageListPicker_OnSelectionChanged;
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
            if (Utils.Utils.IsEmailValid(NewEmailTextBox.Text) == true)
            {
                var sc = new PMSettingsController(RequestType.ChangeEmail);
                sc.ChangeEmail(NewEmailTextBox.Text);
            }
        }

        private void PivotPins_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void OldPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            OldPwdTextBlock.Visibility = (OldPwdPasswordBox.Password.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void NewPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewPwdTextBlock.Visibility = (NewPwdPasswordBox.Password.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void NewEmailTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewEmailTextBlock.Visibility = (NewEmailTextBox.Text.Length != 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        private void LanguageListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((((LanguageListPicker).SelectedItem) as Language).Type)
            {
                case PMFlagModel.FlagsType.EN:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                    App.RootFrame.Language = XmlLanguage.GetLanguage("en-US");
                    FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                    App.RootFrame.FlowDirection = flow;
                    App.Current.RootVisual.UpdateLayout();
                    App.RootFrame.UpdateLayout();
                    var ReloadUri =( App.RootFrame.Content as PhoneApplicationPage).NavigationService.CurrentSource;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(ReloadUri + "?no-cache=" + Guid.NewGuid(), UriKind.Relative));
                    
                    NavigationService.RemoveBackEntry();
                    NavigationService.RemoveBackEntry();
                    break;

                case PMFlagModel.FlagsType.FR:
                    //Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

                    break;
            }
            Logs.Output.ShowOutput(Thread.CurrentThread.CurrentUICulture.DisplayName);
            Logs.Output.ShowOutput(Thread.CurrentThread.CurrentUICulture.EnglishName);
            Logs.Output.ShowOutput(Thread.CurrentThread.CurrentUICulture.Name);
            Logs.Output.ShowOutput(Thread.CurrentThread.CurrentUICulture.NativeName);

        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            PMData.PinsList.Clear();
            NavigationService.Navigate(Paths.MapView);
            e.Cancel = false;
        }
    }
}