﻿#pragma checksum "C:\Users\kevin_000\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMSplashView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "85BD76AE21EABD8466D9D89AFA9CF778"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18408
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
    
    
    public partial class PMSplashView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid GridLogo;
        
        internal System.Windows.Media.Animation.Storyboard MoveLogo;
        
        internal System.Windows.Controls.ProgressBar progressBar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PinMessaging;component/View/PMSplashView.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.GridLogo = ((System.Windows.Controls.Grid)(this.FindName("GridLogo")));
            this.MoveLogo = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveLogo")));
            this.progressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("progressBar")));
        }
    }
}

