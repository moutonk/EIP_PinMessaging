﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMMapView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4740F81C9F71BAB96D57832448DE248D"
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
using Microsoft.Phone.Maps.Controls;
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
    
    
    public partial class PMMapView : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Canvas CentralCanvas;
        
        internal System.Windows.Media.Animation.Storyboard moveAnimation;
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.Grid LeftMenuGrid;
        
        internal System.Windows.Controls.Image ImgMap;
        
        internal System.Windows.Controls.Button ButtonMap;
        
        internal System.Windows.Controls.Image ImgProfil;
        
        internal System.Windows.Controls.Button ButtonProfil;
        
        internal System.Windows.Controls.Image ImgPins;
        
        internal System.Windows.Controls.Button ButtonPins;
        
        internal System.Windows.Controls.Image ImgFilters;
        
        internal System.Windows.Controls.Button ButtonFilters;
        
        internal System.Windows.Controls.Image ImgSettings;
        
        internal System.Windows.Controls.Button ButtonSettings;
        
        internal System.Windows.Controls.Image ImgReward;
        
        internal System.Windows.Controls.Button ButtonReward;
        
        internal System.Windows.Controls.Image ImgAbout;
        
        internal System.Windows.Controls.Button ButtonAbout;
        
        internal System.Windows.Controls.Image ImgLogout;
        
        internal System.Windows.Controls.Button ButtonLogout;
        
        internal System.Windows.Controls.Border RightMenuGrid;
        
        internal System.Windows.Controls.Grid MainVerticalGrid;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationUp;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationDown;
        
        internal System.Windows.Controls.Grid MainGridMap;
        
        internal System.Windows.Controls.Grid GridMap;
        
        internal Microsoft.Phone.Maps.Controls.Map Map;
        
        internal System.Windows.Controls.Grid GridMapMenu;
        
        internal System.Windows.Controls.Button MenuButton;
        
        internal System.Windows.Controls.Button NotificationButton;
        
        internal System.Windows.Controls.Button ContactsButton;
        
        internal System.Windows.Controls.Button PinsButton;
        
        internal System.Windows.Controls.Grid UnderMenuGrid;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PinMessaging;component/View/PMMapView.xaml", System.UriKind.Relative));
            this.CentralCanvas = ((System.Windows.Controls.Canvas)(this.FindName("CentralCanvas")));
            this.moveAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("moveAnimation")));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.LeftMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("LeftMenuGrid")));
            this.ImgMap = ((System.Windows.Controls.Image)(this.FindName("ImgMap")));
            this.ButtonMap = ((System.Windows.Controls.Button)(this.FindName("ButtonMap")));
            this.ImgProfil = ((System.Windows.Controls.Image)(this.FindName("ImgProfil")));
            this.ButtonProfil = ((System.Windows.Controls.Button)(this.FindName("ButtonProfil")));
            this.ImgPins = ((System.Windows.Controls.Image)(this.FindName("ImgPins")));
            this.ButtonPins = ((System.Windows.Controls.Button)(this.FindName("ButtonPins")));
            this.ImgFilters = ((System.Windows.Controls.Image)(this.FindName("ImgFilters")));
            this.ButtonFilters = ((System.Windows.Controls.Button)(this.FindName("ButtonFilters")));
            this.ImgSettings = ((System.Windows.Controls.Image)(this.FindName("ImgSettings")));
            this.ButtonSettings = ((System.Windows.Controls.Button)(this.FindName("ButtonSettings")));
            this.ImgReward = ((System.Windows.Controls.Image)(this.FindName("ImgReward")));
            this.ButtonReward = ((System.Windows.Controls.Button)(this.FindName("ButtonReward")));
            this.ImgAbout = ((System.Windows.Controls.Image)(this.FindName("ImgAbout")));
            this.ButtonAbout = ((System.Windows.Controls.Button)(this.FindName("ButtonAbout")));
            this.ImgLogout = ((System.Windows.Controls.Image)(this.FindName("ImgLogout")));
            this.ButtonLogout = ((System.Windows.Controls.Button)(this.FindName("ButtonLogout")));
            this.RightMenuGrid = ((System.Windows.Controls.Border)(this.FindName("RightMenuGrid")));
            this.MainVerticalGrid = ((System.Windows.Controls.Grid)(this.FindName("MainVerticalGrid")));
            this.MoveAnimationUp = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationUp")));
            this.MoveAnimationDown = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationDown")));
            this.MainGridMap = ((System.Windows.Controls.Grid)(this.FindName("MainGridMap")));
            this.GridMap = ((System.Windows.Controls.Grid)(this.FindName("GridMap")));
            this.Map = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("Map")));
            this.GridMapMenu = ((System.Windows.Controls.Grid)(this.FindName("GridMapMenu")));
            this.MenuButton = ((System.Windows.Controls.Button)(this.FindName("MenuButton")));
            this.NotificationButton = ((System.Windows.Controls.Button)(this.FindName("NotificationButton")));
            this.ContactsButton = ((System.Windows.Controls.Button)(this.FindName("ContactsButton")));
            this.PinsButton = ((System.Windows.Controls.Button)(this.FindName("PinsButton")));
            this.UnderMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuGrid")));
        }
    }
}

