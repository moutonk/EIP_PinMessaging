﻿#pragma checksum "C:\Users\mouton_k\Documents\GitHub\EIP_PinMessaging\PinMessaging\View\PMMapView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9CC0F08327EC803E36F4EAD728B99983"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34014
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Coding4Fun.Toolkit.Controls;
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
        
        internal Microsoft.Phone.Controls.Pivot PivotPins;
        
        internal Microsoft.Phone.Controls.PivotItem MyPinsPivotItem;
        
        internal System.Windows.Controls.StackPanel MyPinsStackPanel;
        
        internal System.Windows.Controls.Grid MainVerticalGrid;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationUp;
        
        internal System.Windows.Media.Animation.Storyboard MoveAnimationDown;
        
        internal System.Windows.Controls.Grid MainGridMap;
        
        internal System.Windows.Controls.ProgressBar ProgressBarMap;
        
        internal System.Windows.Controls.Grid GridMap;
        
        internal Microsoft.Phone.Maps.Controls.Map Map;
        
        internal System.Windows.Media.ImageBrush ImgTarget;
        
        internal System.Windows.Controls.Grid NotificationGrid;
        
        internal System.Windows.Controls.Button RefreshPinButton;
        
        internal System.Windows.Controls.Button CreatePinButton;
        
        internal System.Windows.Controls.Button HelpButton;
        
        internal System.Windows.Controls.Grid GridMapMenu;
        
        internal System.Windows.Controls.Grid UnderMenuGrid;
        
        internal System.Windows.Controls.TextBlock DownMenuTitle;
        
        internal System.Windows.Controls.Button CloseMenuDownButton;
        
        internal System.Windows.Controls.ProgressBar UnderMenuProgressBar;
        
        internal Microsoft.Phone.Controls.Pivot PivotContacts;
        
        internal Microsoft.Phone.Controls.PivotItem MyContactsPivotItem;
        
        internal System.Windows.Controls.ScrollViewer UnderMenuContactScrollViewer;
        
        internal System.Windows.Controls.StackPanel UnderMenuContactPanel;
        
        internal System.Windows.Controls.TextBox SearchContactsTextBox;
        
        internal System.Windows.Controls.StackPanel SearchContactStackPanel;
        
        internal System.Windows.Controls.Grid UnderMenuNotificationGrid;
        
        internal System.Windows.Controls.StackPanel NotificationStackPanel;
        
        internal System.Windows.Controls.ScrollViewer UnderMenuPinDescriptionScrollView;
        
        internal System.Windows.Controls.Grid UnderMenuPinDescriptionGrid;
        
        internal System.Windows.Controls.Image PinDescriptionImage;
        
        internal System.Windows.Controls.TextBlock PinTitleTextBlock;
        
        internal System.Windows.Controls.TextBlock PinCreationTimeDescriptionTextBlock;
        
        internal System.Windows.Controls.TextBlock PinMessageDescriptionTextBlock;
        
        internal System.Windows.Controls.StackPanel PinImageStackPanel;
        
        internal System.Windows.Controls.Image PinImage;
        
        internal System.Windows.Controls.Image AuthorPicture;
        
        internal System.Windows.Controls.TextBlock PinAuthortitleDescriptionTextBlock;
        
        internal System.Windows.Controls.TextBlock PinAuthorDescriptionTextBlock;
        
        internal System.Windows.Controls.TextBlock CommentTitleDescriptionTextBlock;
        
        internal Coding4Fun.Toolkit.Controls.ChatBubbleTextBox CommentChatBubble;
        
        internal System.Windows.Controls.TextBlock PinCommentTipContentTextBox;
        
        internal System.Windows.Controls.TextBlock MeTextBlock;
        
        internal System.Windows.Controls.StackPanel CommentStackPanel;
        
        internal System.Windows.Controls.ScrollViewer UnderMenuCreatePinScrollViewer;
        
        internal System.Windows.Controls.Grid UnderMenuCreatePinGrid;
        
        internal System.Windows.Controls.ProgressBar DropPinProgressBar;
        
        internal Microsoft.Phone.Controls.ListPicker PinListPicker;
        
        internal System.Windows.Controls.TextBox PinCreateTitleTextBox;
        
        internal System.Windows.Controls.TextBox PinCreateMessageTextBox;
        
        internal System.Windows.Controls.StackPanel EventStackPanel;
        
        internal Microsoft.Phone.Controls.DatePicker EventDate;
        
        internal Microsoft.Phone.Controls.TimePicker EventTime;
        
        internal System.Windows.Controls.StackPanel TargetStackPanel;
        
        internal System.Windows.Controls.TextBlock NoContactsTextBlock;
        
        internal Microsoft.Phone.Controls.LongListMultiSelector TargetLongListSelector;
        
        internal System.Windows.Controls.Button DropPinButton;
        
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
            this.PivotPins = ((Microsoft.Phone.Controls.Pivot)(this.FindName("PivotPins")));
            this.MyPinsPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("MyPinsPivotItem")));
            this.MyPinsStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("MyPinsStackPanel")));
            this.MainVerticalGrid = ((System.Windows.Controls.Grid)(this.FindName("MainVerticalGrid")));
            this.MoveAnimationUp = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationUp")));
            this.MoveAnimationDown = ((System.Windows.Media.Animation.Storyboard)(this.FindName("MoveAnimationDown")));
            this.MainGridMap = ((System.Windows.Controls.Grid)(this.FindName("MainGridMap")));
            this.ProgressBarMap = ((System.Windows.Controls.ProgressBar)(this.FindName("ProgressBarMap")));
            this.GridMap = ((System.Windows.Controls.Grid)(this.FindName("GridMap")));
            this.Map = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("Map")));
            this.ImgTarget = ((System.Windows.Media.ImageBrush)(this.FindName("ImgTarget")));
            this.NotificationGrid = ((System.Windows.Controls.Grid)(this.FindName("NotificationGrid")));
            this.RefreshPinButton = ((System.Windows.Controls.Button)(this.FindName("RefreshPinButton")));
            this.CreatePinButton = ((System.Windows.Controls.Button)(this.FindName("CreatePinButton")));
            this.HelpButton = ((System.Windows.Controls.Button)(this.FindName("HelpButton")));
            this.GridMapMenu = ((System.Windows.Controls.Grid)(this.FindName("GridMapMenu")));
            this.UnderMenuGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuGrid")));
            this.DownMenuTitle = ((System.Windows.Controls.TextBlock)(this.FindName("DownMenuTitle")));
            this.CloseMenuDownButton = ((System.Windows.Controls.Button)(this.FindName("CloseMenuDownButton")));
            this.UnderMenuProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("UnderMenuProgressBar")));
            this.PivotContacts = ((Microsoft.Phone.Controls.Pivot)(this.FindName("PivotContacts")));
            this.MyContactsPivotItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("MyContactsPivotItem")));
            this.UnderMenuContactScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("UnderMenuContactScrollViewer")));
            this.UnderMenuContactPanel = ((System.Windows.Controls.StackPanel)(this.FindName("UnderMenuContactPanel")));
            this.SearchContactsTextBox = ((System.Windows.Controls.TextBox)(this.FindName("SearchContactsTextBox")));
            this.SearchContactStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("SearchContactStackPanel")));
            this.UnderMenuNotificationGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuNotificationGrid")));
            this.NotificationStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("NotificationStackPanel")));
            this.UnderMenuPinDescriptionScrollView = ((System.Windows.Controls.ScrollViewer)(this.FindName("UnderMenuPinDescriptionScrollView")));
            this.UnderMenuPinDescriptionGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuPinDescriptionGrid")));
            this.PinDescriptionImage = ((System.Windows.Controls.Image)(this.FindName("PinDescriptionImage")));
            this.PinTitleTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinTitleTextBlock")));
            this.PinCreationTimeDescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinCreationTimeDescriptionTextBlock")));
            this.PinMessageDescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinMessageDescriptionTextBlock")));
            this.PinImageStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("PinImageStackPanel")));
            this.PinImage = ((System.Windows.Controls.Image)(this.FindName("PinImage")));
            this.AuthorPicture = ((System.Windows.Controls.Image)(this.FindName("AuthorPicture")));
            this.PinAuthortitleDescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinAuthortitleDescriptionTextBlock")));
            this.PinAuthorDescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("PinAuthorDescriptionTextBlock")));
            this.CommentTitleDescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("CommentTitleDescriptionTextBlock")));
            this.CommentChatBubble = ((Coding4Fun.Toolkit.Controls.ChatBubbleTextBox)(this.FindName("CommentChatBubble")));
            this.PinCommentTipContentTextBox = ((System.Windows.Controls.TextBlock)(this.FindName("PinCommentTipContentTextBox")));
            this.MeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("MeTextBlock")));
            this.CommentStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("CommentStackPanel")));
            this.UnderMenuCreatePinScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("UnderMenuCreatePinScrollViewer")));
            this.UnderMenuCreatePinGrid = ((System.Windows.Controls.Grid)(this.FindName("UnderMenuCreatePinGrid")));
            this.DropPinProgressBar = ((System.Windows.Controls.ProgressBar)(this.FindName("DropPinProgressBar")));
            this.PinListPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("PinListPicker")));
            this.PinCreateTitleTextBox = ((System.Windows.Controls.TextBox)(this.FindName("PinCreateTitleTextBox")));
            this.PinCreateMessageTextBox = ((System.Windows.Controls.TextBox)(this.FindName("PinCreateMessageTextBox")));
            this.EventStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("EventStackPanel")));
            this.EventDate = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("EventDate")));
            this.EventTime = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("EventTime")));
            this.TargetStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("TargetStackPanel")));
            this.NoContactsTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("NoContactsTextBlock")));
            this.TargetLongListSelector = ((Microsoft.Phone.Controls.LongListMultiSelector)(this.FindName("TargetLongListSelector")));
            this.DropPinButton = ((System.Windows.Controls.Button)(this.FindName("DropPinButton")));
        }
    }
}

