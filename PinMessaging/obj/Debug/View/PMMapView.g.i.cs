﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMMapView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8409C3FCA2C7DF43B348156EDAB44344"
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
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage @this;
        
        internal System.Windows.Controls.Canvas CentralCanvas;
        
        internal System.Windows.Media.Animation.Storyboard moveAnimation;
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.Grid LeftMenuGrid;
        
        internal System.Windows.Controls.Image ImgMap;
        
        internal System.Windows.Controls.TextBlock ButtonMap;
        
        internal System.Windows.Controls.Image ImgFilters;
        
        internal System.Windows.Controls.TextBlock ButtonFilters;
        
        internal System.Windows.Controls.Canvas MyAccountCanvasLeft;
        
        internal System.Windows.Controls.Canvas MyAccountCanvasRight;
        
        internal System.Windows.Controls.TextBlock MyAccountTextBlock;
        
        internal System.Windows.Controls.Image ImgProfil;
        
        internal System.Windows.Controls.TextBlock ButtonProfil;
        
        internal System.Windows.Controls.Image ImgPins;
        
        internal System.Windows.Controls.TextBlock ButtonPins;
        
        internal System.Windows.Controls.Image ImgSettings;
        
        internal System.Windows.Controls.TextBlock ButtonSettings;
        
        internal System.Windows.Controls.Image ImgReward;
        
        internal System.Windows.Controls.TextBlock ButtonReward;
        
        internal System.Windows.Controls.Image ImgAbout;
        
        internal System.Windows.Controls.TextBlock ButtonAbout;
        
        internal System.Windows.Controls.Image ImgLogout;
        
        internal System.Windows.Controls.TextBlock ButtonLogout;
        
        internal System.Windows.Controls.Grid RightMenuGrid;
        
        internal System.Windows.Controls.ProgressBar PostPinProgressBar;
        
        internal Microsoft.Phone.Controls.Pivot pivot;
        
        internal Microsoft.Phone.Controls.PivotItem NewPinPivotItem;
        
        internal System.Windows.Controls.Grid GridNewPinPivotItem;
        
        internal Microsoft.Phone.Controls.ListPicker ListPickerPinType;
        
        internal Microsoft.Phone.Controls.DatePicker DateEvent;
        
        internal Microsoft.Phone.Controls.TimePicker TimeEvent;
        
        internal System.Windows.Controls.TextBox PinReceiver;
        
        internal System.Windows.Controls.TextBox PinTitle;
        
        internal System.Windows.Controls.TextBox PinContent;
        
        internal System.Windows.Controls.Image PinImage;
        
        internal System.Windows.Controls.Button PostPinButton;
        
        internal System.Windows.Controls.Image image2;
        
        internal System.Windows.Controls.Image image3;
        
        internal System.Windows.Controls.Grid MainVerticalGrid;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationUp;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationDown;
        
        internal System.Windows.Controls.Grid MainGridMap;
        
        internal System.Windows.Controls.ProgressBar ProgressBarMap;
        
        internal System.Windows.Controls.Grid GridMap;
        
        internal Microsoft.Phone.Maps.Controls.Map Map;
        
        internal System.Windows.Media.ImageBrush ImgTarget;
        
        internal System.Windows.Controls.Grid GridMapMenu;
        
        internal System.Windows.Controls.Button MenuButton;
        
        internal System.Windows.Media.ImageBrush ImgMenuButton;
        
        internal System.Windows.Controls.Button NotificationButton;
        
        internal System.Windows.Media.ImageBrush ImgNotificationButton;
        
        internal System.Windows.Controls.Button ContactsButton;
        
        internal System.Windows.Media.ImageBrush ImgContactsButton;
        
        internal System.Windows.Controls.Button PinsButton;
        
        internal System.Windows.Media.ImageBrush ImgPinsButton;
        
        internal System.Windows.Controls.Grid UnderMenuGrid;
        
        internal System.Windows.Controls.TextBlock DownMenuTitle;
        
        internal System.Windows.Controls.Button CloseMenuDownButton;
        
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
            this.@this = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("this")));
            this.CentralCanvas = ((System.Windows.Controls.Canvas)(this.FindName("CentralCanvas")));
            this.moveAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("moveAnimation")));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.LeftMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("LeftMenuGrid")));
            this.ImgMap = ((System.Windows.Controls.Image)(this.FindName("ImgMap")));
            this.ButtonMap = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonMap")));
            this.ImgFilters = ((System.Windows.Controls.Image)(this.FindName("ImgFilters")));
            this.ButtonFilters = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonFilters")));
            this.MyAccountCanvasLeft = ((System.Windows.Controls.Canvas)(this.FindName("MyAccountCanvasLeft")));
            this.MyAccountCanvasRight = ((System.Windows.Controls.Canvas)(this.FindName("MyAccountCanvasRight")));
            this.MyAccountTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("MyAccountTextBlock")));
            this.ImgProfil = ((System.Windows.Controls.Image)(this.FindName("ImgProfil")));
            this.ButtonProfil = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonProfil")));
            this.ImgPins = ((System.Windows.Controls.Image)(this.FindName("ImgPins")));
            this.ButtonPins = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonPins")));
            this.ImgSettings = ((System.Windows.Controls.Image)(this.FindName("ImgSettings")));
            this.ButtonSettings = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonSettings")));
            this.ImgReward = ((System.Windows.Controls.Image)(this.FindName("ImgReward")));
            this.ButtonReward = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonReward")));
            this.ImgAbout = ((System.Windows.Controls.Image)(this.FindName("ImgAbout")));
            this.ButtonAbout = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonAbout")));
            this.ImgLogout = ((System.Windows.Controls.Image)(this.FindName("ImgLogout")));
            this.ButtonLogout = ((System.Windows.Controls.TextBlock)(this.FindName("ButtonLogout")));
            this.RightMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("RightMenuGrid")));
            this.PostPinProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("PostPinProgressBar")));
            this.pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivot")));
            this.NewPinPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("NewPinPivotItem")));
            this.GridNewPinPivotItem = ((System.Windows.Controls.Grid)(this.FindName("GridNewPinPivotItem")));
            this.ListPickerPinType = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListPickerPinType")));
            this.DateEvent = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DateEvent")));
            this.TimeEvent = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("TimeEvent")));
            this.PinReceiver = ((System.Windows.Controls.TextBox)(this.FindName("PinReceiver")));
            this.PinTitle = ((System.Windows.Controls.TextBox)(this.FindName("PinTitle")));
            this.PinContent = ((System.Windows.Controls.TextBox)(this.FindName("PinContent")));
            this.PinImage = ((System.Windows.Controls.Image)(this.FindName("PinImage")));
            this.PostPinButton = ((System.Windows.Controls.Button)(this.FindName("PostPinButton")));
            this.image2 = ((System.Windows.Controls.Image)(this.FindName("image2")));
            this.image3 = ((System.Windows.Controls.Image)(this.FindName("image3")));
            this.MainVerticalGrid = ((System.Windows.Controls.Grid)(this.FindName("MainVerticalGrid")));
            this.MoveAnimationUp = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationUp")));
            this.MoveAnimationDown = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationDown")));
            this.MainGridMap = ((System.Windows.Controls.Grid)(this.FindName("MainGridMap")));
            this.ProgressBarMap = ((System.Windows.Controls.ProgressBar)(this.FindName("ProgressBarMap")));
            this.GridMap = ((System.Windows.Controls.Grid)(this.FindName("GridMap")));
            this.Map = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("Map")));
            this.ImgTarget = ((System.Windows.Media.ImageBrush)(this.FindName("ImgTarget")));
            this.GridMapMenu = ((System.Windows.Controls.Grid)(this.FindName("GridMapMenu")));
            this.MenuButton = ((System.Windows.Controls.Button)(this.FindName("MenuButton")));
            this.ImgMenuButton = ((System.Windows.Media.ImageBrush)(this.FindName("ImgMenuButton")));
            this.NotificationButton = ((System.Windows.Controls.Button)(this.FindName("NotificationButton")));
            this.ImgNotificationButton = ((System.Windows.Media.ImageBrush)(this.FindName("ImgNotificationButton")));
            this.ContactsButton = ((System.Windows.Controls.Button)(this.FindName("ContactsButton")));
            this.ImgContactsButton = ((System.Windows.Media.ImageBrush)(this.FindName("ImgContactsButton")));
            this.PinsButton = ((System.Windows.Controls.Button)(this.FindName("PinsButton")));
            this.ImgPinsButton = ((System.Windows.Media.ImageBrush)(this.FindName("ImgPinsButton")));
            this.UnderMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuGrid")));
            this.DownMenuTitle = ((System.Windows.Controls.TextBlock)(this.FindName("DownMenuTitle")));
            this.CloseMenuDownButton = ((System.Windows.Controls.Button)(this.FindName("CloseMenuDownButton")));
        }
    }
}

