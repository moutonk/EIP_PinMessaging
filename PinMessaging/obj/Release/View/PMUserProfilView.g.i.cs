﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMUserProfilView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "363CCCF26631F1D3903D2FCD3E312E15"
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
    
    
    public partial class PMUserProfil : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid UseProfilMainGrid;
        
        internal System.Windows.Controls.TextBlock ProfilTitleTextBlock;
        
        internal System.Windows.Controls.ProgressBar UserProfilProgressBar;
        
        internal Microsoft.Phone.Controls.Pivot PivotProfil;
        
        internal Microsoft.Phone.Controls.PivotItem ProfilPivotItem;
        
        internal System.Windows.Controls.Image ProfilPictureImage;
        
        internal System.Windows.Controls.TextBlock LoginTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock LoginTextBlock;
        
        internal System.Windows.Controls.TextBlock GradeTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock GradeTextBlock;
        
        internal System.Windows.Controls.TextBlock PointsTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock PointsTextBlock;
        
        internal System.Windows.Controls.TextBlock PinsCreatedTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock PinsCreatedTextBlock;
        
        internal System.Windows.Controls.TextBlock CommentsTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock CommentsTextBlock;
        
        internal Microsoft.Phone.Controls.PivotItem StatsPivotItem;
        
        internal System.Windows.Controls.Grid UserHistoryGrid;
        
        internal System.Windows.Controls.StackPanel HistoryItemsStackPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PinMessaging;component/View/PMUserProfilView.xaml", System.UriKind.Relative));
            this.UseProfilMainGrid = ((System.Windows.Controls.Grid)(this.FindName("UseProfilMainGrid")));
            this.ProfilTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ProfilTitleTextBlock")));
            this.UserProfilProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("UserProfilProgressBar")));
            this.PivotProfil = ((Microsoft.Phone.Controls.Pivot)(this.FindName("PivotProfil")));
            this.ProfilPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("ProfilPivotItem")));
            this.ProfilPictureImage = ((System.Windows.Controls.Image)(this.FindName("ProfilPictureImage")));
            this.LoginTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("LoginTitleTextBlock")));
            this.LoginTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("LoginTextBlock")));
            this.GradeTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("GradeTitleTextBlock")));
            this.GradeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("GradeTextBlock")));
            this.PointsTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PointsTitleTextBlock")));
            this.PointsTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PointsTextBlock")));
            this.PinsCreatedTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinsCreatedTitleTextBlock")));
            this.PinsCreatedTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinsCreatedTextBlock")));
            this.CommentsTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("CommentsTitleTextBlock")));
            this.CommentsTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("CommentsTextBlock")));
            this.StatsPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("StatsPivotItem")));
            this.UserHistoryGrid = ((System.Windows.Controls.Grid)(this.FindName("UserHistoryGrid")));
            this.HistoryItemsStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("HistoryItemsStackPanel")));
        }
    }
}

