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

        private class FeedBack
        {
            public string Name { get; set; }
            public FeedbackType Type { get; set; }
        }

        public enum FeedbackType
        {
            Bug,
            Idea,
            Question,
        }

        public string FromFeedbackTypeToString(FeedbackType type)
        {
            switch (type)
            {
                case FeedbackType.Bug:
                    return "B";
                case FeedbackType.Idea:
                    return "I";
                case FeedbackType.Question:
                    return "Q";
                default:
                    return "B";
            }
        }

        public PMSettings()
        {
            InitializeComponent();

            if (RememberConnection.GetLoginPwd() != null)
                CurrentEmailTextBlock.Text = RememberConnection.GetLoginPwd().Email;

            LanguageListPicker.Items.Add(new  Language { Name = "Français", Image = new BitmapImage(Paths.FlagFR), Type = PMFlagModel.FlagsType.FR });
            LanguageListPicker.Items.Add(new Language { Name = "English", Image = new BitmapImage(Paths.FlagEN), Type = PMFlagModel.FlagsType.EN });

            FeedbackTypeListPicker.Items.Add(new FeedBack {Name = "Bug", Type = FeedbackType.Bug});
            FeedbackTypeListPicker.Items.Add(new FeedBack { Name = "Idea", Type = FeedbackType.Idea });
            FeedbackTypeListPicker.Items.Add(new FeedBack { Name = "Question", Type = FeedbackType.Question });

            try
            {
                //select the correct language regardings the telephone language
                LanguageListPicker.SelectedIndex = Thread.CurrentThread.CurrentUICulture.Name.Equals("fr-FR") ? 0 : 1;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("PMSettings:" + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
                LanguageListPicker.SelectedIndex = 0;
            }

            LocationServicesCheckBox.IsChecked = RememberConnection.GetAccessLocation() ?? true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var pivot = "";

            if (NavigationContext.QueryString.TryGetValue("open", out pivot))
            {
                switch (int.Parse(pivot))
                {
                    case 0:
                        ShowPivotNumber(0);
                        break;

                    case 1:
                        ShowPivotNumber(1);
                        break;

                    case 2:
                        ShowPivotNumber(2);
                        break;
                }
            }
        }

        private void ShowPivotNumber(int num)
        {
            if (num >= 0 && num < SettingsPivot.Items.Count)
                SettingsPivot.SelectedIndex = num;
        }

        private void ModifyPwdButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewPwdPasswordBox.Password.Equals(NewPwdConfirmPasswordBox.Password) == false)
            {
                Utils.Utils.CustomMessageBox(new[] { AppResources.Ok }, AppResources.ChangePwd, AppResources.PMPasswordsNotSame);
            }   
            else if (Utils.Utils.PasswordSyntaxCheck(NewPwdPasswordBox.Password) == false)
            {
                Utils.Utils.CustomMessageBox(new[] { AppResources.Ok }, AppResources.ChangePwd, AppResources.PMWrongPasswordSyntax);
            }
            else
            {
                var sc = new PMSettingsController(RequestType.ChangePassword, UpdateUiModifyPwdError, UpdateUiModifyPwdSuccess);
                sc.ChangePassword(OldPwdPasswordBox.Password, NewPwdPasswordBox.Password);
            }
        }

        private void UpdateUiModifyPwdError()
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show("ChangePwdStatus: " + PMData.IsChangePwdSuccess + " " + AppResources.ChangePwdError));
        }

        private void UpdateUiModifyPwdSuccess()
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.ChangePwdSuccess));
            OldPwdPasswordBox.Password = string.Empty;
            NewPwdPasswordBox.Password = string.Empty;
            NewPwdConfirmPasswordBox.Password = string.Empty;
            ModifyPwdButton.IsEnabled = false;
        }

        private void ModifyEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Utils.IsEmailValid(NewEmailTextBox.Text) == false)
            {
                Utils.Utils.CustomMessageBox(new[] { AppResources.Ok }, AppResources.ChangeEmail, AppResources.PMWrongEmailSyntax);
            }   
            else
            {
                var sc = new PMSettingsController(RequestType.ChangeEmail, UpdateUiModifyEmailError, UpdateUiModifyEmailSuccess);
                sc.ChangeEmail(NewEmailTextBox.Text);
            }
        }

        private void UpdateUiModifyEmailError()
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show("ChangeEmailStatus: " + PMData.IsChangeEmailSuccess + " " + AppResources.ChangePwdError));
        }

        private void UpdateUiModifyEmailSuccess()
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.ChangePwdSuccess));
            NewEmailTextBox.Text = string.Empty;
            ModifyEmailButton.IsEnabled = false;
        }

        private void PivotPins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OldPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            OldPwdTextBlock.Opacity = (OldPwdPasswordBox.Password.Length != 0 ? 0 : 1);
        }

        private void NewPwdConfirmPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewPwdConfirmTextBlock.Opacity = (NewPwdConfirmPasswordBox.Password.Length != 0 ? 0 : 1);
        }

        private void NewPwdPasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewPwdTextBlock.Opacity = (NewPwdPasswordBox.Password.Length != 0 ? 0 : 1);
        }

        private void NewEmailTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            NewEmailTextBlock.Opacity = (NewEmailTextBox.Text.Length != 0 ? 0 : 1);
        }

        private void ChangeLanguage(string lang)
        {
            try
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
            catch (Exception exp)
            {
                Logs.Error.ShowError("ChangeLanguage:" + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void LanguageListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lang = (LanguageListPicker).SelectedItem as Language;

            if (lang == null)
            {
                Logs.Error.ShowError("LanguageListPicker_OnSelectionChanged: lang is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            switch (lang.Type)
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
            try
            {
                //if the language was changed
                if (PMData.CurrentLanguge.Equals(Thread.CurrentThread.CurrentUICulture.Name) == false)
                {
                    PMData.CurrentLanguge = Thread.CurrentThread.CurrentUICulture.Name;
                    PMData.PinsList.Clear();
                    NavigationService.Navigate(Paths.MapView);
                }
                e.Cancel = false;
            }
            catch (Exception exp) 
            {
                Logs.Error.ShowError("OnBackKeyPress: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
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
            ModifyEmailButton.IsEnabled = NewEmailTextBox.Text.Length != 0;
        }

        private void LocationServicesCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            PMData.AppMode = PMData.ApplicationMode.Normal;
            RememberConnection.SaveAccessLocation(true);
        }

        private void LocationServicesCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            PMData.AppMode = PMData.ApplicationMode.Offline;
            RememberConnection.SaveAccessLocation(false);
        }

        private void UpdateUiFeedBack()
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(AppResources.ThanksFeedback);
                FeedbackSendButton.IsEnabled = true;
            });
        }

        private void FeedbackSendButton_OnClick(object sender, RoutedEventArgs e)
        {
            var feed = (FeedbackTypeListPicker).SelectedItem as FeedBack;

            if (feed == null)
            {
                Logs.Error.ShowError("FeedbackSendButton_OnClick: feeed is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var sc = new PMSettingsController(RequestType.Feedback, UpdateUiFeedBack, UpdateUiFeedBack);

            sc.PostFeedback(FromFeedbackTypeToString(feed.Type), FeedbackTipTextBox.Text, "");
            FeedbackSendButton.IsEnabled = false;
        }

        private void FeedbackTipTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FeedbackSendButton.IsEnabled = FeedbackTipTextBox.Text.Length != 0;
        }
    }
}