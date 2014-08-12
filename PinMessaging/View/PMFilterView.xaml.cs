using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Utils;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace PinMessaging.View
{
    public partial class PMFilterView : PhoneApplicationPage
    {
        public PMFilterView()
        {
            InitializeComponent();
        }

        private static void HideOrNotItemsInMap(string typeToMove, bool hideStatus)
        {
            PMPinModel.PinsType typeToModify;

            if (Enum.TryParse(typeToMove, true, out typeToModify) == true)
                PMMapPinController.HideOrNotPinType(typeToModify, hideStatus);
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
            StackPanel sp;
    
            try
            {
                sp = sender as StackPanel;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ItemOnTap:" + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            if (PinDisplayedStackPanel.Children.Contains(sp))
            {
                SwitchItems(sender as StackPanel, PinDisplayedStackPanel, PinNotDisplayedStackPanel);
                HideOrNotItemsInMap(sp.Tag.ToString(), true);
            }
            else
            {
                SwitchItems(sender as StackPanel, PinNotDisplayedStackPanel, PinDisplayedStackPanel);
                HideOrNotItemsInMap(sp.Tag.ToString(), false);
            }

            EmptyPinsTextBlock2.Visibility = PinNotDisplayedStackPanel.Children.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
            EmptyPinsTextBlock1.Visibility = PinDisplayedStackPanel.Children.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}