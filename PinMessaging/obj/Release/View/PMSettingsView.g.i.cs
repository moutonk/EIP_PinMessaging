﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMSettingsView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EBC42ADFE1F4E03A4FB237414E4D734D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18449
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PinMessaging.View {
    
    
    public partial class PMSettings : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock ProfilTitleTextBlock;
        
        internal Microsoft.Phone.Controls.Pivot SettingsPivot;
        
        internal Microsoft.Phone.Controls.PivotItem ApplicationPivotItem;
        
        internal System.Windows.Controls.TextBlock OldPwdTextBlock;
        
        internal System.Windows.Controls.PasswordBox OldPwdPasswordBox;
        
        internal System.Windows.Controls.TextBlock NewPwdTextBlock;
        
        internal System.Windows.Controls.PasswordBox NewPwdPasswordBox;
        
        internal System.Windows.Controls.TextBlock NewPwdConfirmTextBlock;
        
        internal System.Windows.Controls.PasswordBox NewPwdConfirmPasswordBox;
        
        internal System.Windows.Controls.Button ModifyPwdButton;
        
        internal System.Windows.Controls.TextBlock CurrentEmailTextBlock;
        
        internal System.Windows.Controls.TextBlock NewEmailTextBlock;
        
        internal System.Windows.Controls.TextBox NewEmailTextBox;
        
        internal System.Windows.Controls.Button ModifyEmailButton;
        
        internal System.Windows.Controls.TextBlock LanguageTextBoxWarning;
        
        internal Microsoft.Phone.Controls.ListPicker LanguageListPicker;
        
        internal System.Windows.Controls.CheckBox LocationServicesCheckBox;
        
        internal System.Windows.Controls.TextBlock FeedbackTipTextBlock;
        
        internal Microsoft.Phone.Controls.ListPicker FeedbackTypeListPicker;
        
        internal System.Windows.Controls.TextBox FeedbackTipTextBox;
        
        internal System.Windows.Controls.Button FeedbackSendButton;
        
        internal Microsoft.Phone.Controls.PivotItem LegalPivotItem;
        
        internal Microsoft.Phone.Controls.PivotItem AboutPivotItem;
        
        internal System.Windows.Controls.TextBlock PinMessagingTextBlock;
        
        internal System.Windows.Controls.TextBlock PinMessagingInfosTextBlock;
        
        internal System.Windows.Controls.TextBlock PinMessagingVersionTextBlock;
        
        internal System.Windows.Controls.TextBlock PinMessagingCopyrightTextBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PinMessaging;component/View/PMSettingsView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ProfilTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ProfilTitleTextBlock")));
            this.SettingsPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("SettingsPivot")));
            this.ApplicationPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("ApplicationPivotItem")));
            this.OldPwdTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("OldPwdTextBlock")));
            this.OldPwdPasswordBox = ((System.Windows.Controls.PasswordBox)(this.FindName("OldPwdPasswordBox")));
            this.NewPwdTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NewPwdTextBlock")));
            this.NewPwdPasswordBox = ((System.Windows.Controls.PasswordBox)(this.FindName("NewPwdPasswordBox")));
            this.NewPwdConfirmTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NewPwdConfirmTextBlock")));
            this.NewPwdConfirmPasswordBox = ((System.Windows.Controls.PasswordBox)(this.FindName("NewPwdConfirmPasswordBox")));
            this.ModifyPwdButton = ((System.Windows.Controls.Button)(this.FindName("ModifyPwdButton")));
            this.CurrentEmailTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("CurrentEmailTextBlock")));
            this.NewEmailTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NewEmailTextBlock")));
            this.NewEmailTextBox = ((System.Windows.Controls.TextBox)(this.FindName("NewEmailTextBox")));
            this.ModifyEmailButton = ((System.Windows.Controls.Button)(this.FindName("ModifyEmailButton")));
            this.LanguageTextBoxWarning = ((System.Windows.Controls.TextBlock)(this.FindName("LanguageTextBoxWarning")));
            this.LanguageListPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("LanguageListPicker")));
            this.LocationServicesCheckBox = ((System.Windows.Controls.CheckBox)(this.FindName("LocationServicesCheckBox")));
            this.FeedbackTipTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("FeedbackTipTextBlock")));
            this.FeedbackTypeListPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("FeedbackTypeListPicker")));
            this.FeedbackTipTextBox = ((System.Windows.Controls.TextBox)(this.FindName("FeedbackTipTextBox")));
            this.FeedbackSendButton = ((System.Windows.Controls.Button)(this.FindName("FeedbackSendButton")));
            this.LegalPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("LegalPivotItem")));
            this.AboutPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("AboutPivotItem")));
            this.PinMessagingTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinMessagingTextBlock")));
            this.PinMessagingInfosTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinMessagingInfosTextBlock")));
            this.PinMessagingVersionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinMessagingVersionTextBlock")));
            this.PinMessagingCopyrightTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinMessagingCopyrightTextBlock")));
        }
    }
}

