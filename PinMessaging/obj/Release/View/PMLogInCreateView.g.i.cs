﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMLogInCreateView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B2B1D0BDE6446285D8CF2E6ED08F9BC6"
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
    
    
    public partial class PMSignInCreateStructure : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock PageSubTitle;
        
        internal System.Windows.Controls.Grid GridEmailError;
        
        internal System.Windows.Media.Animation.Storyboard MoveTextBoxEmailUp;
        
        internal System.Windows.Media.Animation.Storyboard MoveTextBoxEmailDown;
        
        internal System.Windows.Controls.TextBlock TextBlockError;
        
        internal System.Windows.Controls.TextBox TextBoxEmail;
        
        internal System.Windows.Controls.PasswordBox TextBoxPassword;
        
        internal System.Windows.Controls.Grid GridLogo;
        
        internal System.Windows.Controls.Grid GridPageTitle;
        
        internal System.Windows.Media.Animation.Storyboard MovePageTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Button ButtonPrevious;
        
        internal System.Windows.Controls.Button ButtonValidate;
        
        internal System.Windows.Controls.Grid GridProgress;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignInPart1;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignInPart1Reverse;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignInPart2;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignUpPart1;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignUpPart1Reverse;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignUpPart2;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignUpPart2Reverse;
        
        internal System.Windows.Media.Animation.Storyboard MoveProgressBarSignUpPart3;
        
        internal System.Windows.Controls.ProgressBar ProgressBarLoading;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PinMessaging;component/View/PMLogInCreateView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.PageSubTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageSubTitle")));
            this.GridEmailError = ((System.Windows.Controls.Grid)(this.FindName("GridEmailError")));
            this.MoveTextBoxEmailUp = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveTextBoxEmailUp")));
            this.MoveTextBoxEmailDown = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveTextBoxEmailDown")));
            this.TextBlockError = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlockError")));
            this.TextBoxEmail = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxEmail")));
            this.TextBoxPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("TextBoxPassword")));
            this.GridLogo = ((System.Windows.Controls.Grid)(this.FindName("GridLogo")));
            this.GridPageTitle = ((System.Windows.Controls.Grid)(this.FindName("GridPageTitle")));
            this.MovePageTitle = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MovePageTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ButtonPrevious = ((System.Windows.Controls.Button)(this.FindName("ButtonPrevious")));
            this.ButtonValidate = ((System.Windows.Controls.Button)(this.FindName("ButtonValidate")));
            this.GridProgress = ((System.Windows.Controls.Grid)(this.FindName("GridProgress")));
            this.MoveProgressBarSignInPart1 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignInPart1")));
            this.MoveProgressBarSignInPart1Reverse = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignInPart1Reverse")));
            this.MoveProgressBarSignInPart2 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignInPart2")));
            this.MoveProgressBarSignUpPart1 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignUpPart1")));
            this.MoveProgressBarSignUpPart1Reverse = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignUpPart1Reverse")));
            this.MoveProgressBarSignUpPart2 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignUpPart2")));
            this.MoveProgressBarSignUpPart2Reverse = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignUpPart2Reverse")));
            this.MoveProgressBarSignUpPart3 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveProgressBarSignUpPart3")));
            this.ProgressBarLoading = ((System.Windows.Controls.ProgressBar)(this.FindName("ProgressBarLoading")));
        }
    }
}

