using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace PinMessaging.View
{
    public partial class PMFilterView : PhoneApplicationPage
    {
        public PMFilterView()
        {
            InitializeComponent();

            try
            {
                foreach (var item in PMData.HiddenTypesList)
                {
                    Logs.Output.ShowOutput("Hidden:" + item);
                    switch (item)
                    {
                        case PMPinModel.PinsType.Message:
                            ItemOnTap(PublicMsgStackPanel, null);
                            break;

                        case PMPinModel.PinsType.Event:
                            ItemOnTap(PublicEventStackPanel, null);
                            break;

                        case PMPinModel.PinsType.View:
                            ItemOnTap(PublicViewStackPanel, null);
                            break;

                        case PMPinModel.PinsType.PrivateMessage:
                            ItemOnTap(PrivateMsgStackPanel, null);
                            break;

                        case PMPinModel.PinsType.PrivateEvent:
                            ItemOnTap(PrivateEventStackPanel, null);
                            break;

                        case PMPinModel.PinsType.PrivateView:
                            ItemOnTap(PrivateViewStackPanel, null);
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                Logs.Output.ShowOutput(exp.Message);
            }
        
        }

        private static void HideOrNotItemsInMap(string typeToMove, bool hideStatus, bool init)
        {
            PMPinModel.PinsType typeToModify;

            if (Enum.TryParse(typeToMove, true, out typeToModify) == true)
            {
                if (init == false)
                    PMMapPinController.HideOrNotPinType(typeToModify, hideStatus);
    
                if (hideStatus == true)
                    PMData.HiddenTypesList.Add(typeToModify);
                else
                    PMData.HiddenTypesList.Remove(typeToModify);
            }
            else
                Logs.Error.ShowError("HideOrNotItemsInMap: could not convert pinString to pinType", Logs.Error.ErrorsPriority.NotCritical);
        }

        private static void SwitchItems(StackPanel item, StackPanel orig, StackPanel dest)
        {
            var tmpSp = orig.Children[orig.Children.IndexOf(item)];

            orig.Children.Remove(tmpSp);
            dest.Children.Add(tmpSp);   
        }

        private void ItemOnTap(object sender, GestureEventArgs gestureEventArgs)
        {
            var sp = sender as StackPanel;

            if (sp == null)
            {
                Logs.Error.ShowError("ItemOnTap: sp is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            if (PinDisplayedStackPanel.Children.Contains(sp))
            {
                SwitchItems(sender as StackPanel, PinDisplayedStackPanel, PinNotDisplayedStackPanel);
                HideOrNotItemsInMap(sp.Tag.ToString(), true, gestureEventArgs == null ? true : false);
            }
            else
            {
                SwitchItems(sender as StackPanel, PinNotDisplayedStackPanel, PinDisplayedStackPanel);
                HideOrNotItemsInMap(sp.Tag.ToString(), false, gestureEventArgs == null ? true : false);
            }

            EmptyPinsTextBlock2.Visibility = PinNotDisplayedStackPanel.Children.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
            EmptyPinsTextBlock1.Visibility = PinDisplayedStackPanel.Children.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}