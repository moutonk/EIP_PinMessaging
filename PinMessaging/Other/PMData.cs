
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;

namespace PinMessaging.Other
{
    public static class PMData
    {
        //sign in / sign up
        [DefaultValue(false)] public static bool IsSignInSuccess { get; set; }
        [DefaultValue(false)] public static bool IsSignUpSuccess { get; set; }
        [DefaultValue(false)] public static bool IsEmailDispo { get; set; }
        [DefaultValue(false)] public static bool NetworkProblem { get; set; }

        //pins to add to the MapLayerContainer
        [DefaultValue(null)] public static List<PMPinModel> PinsList { get; set; }

        //contain all the pins
        [DefaultValue(null)] public static MapLayer MapLayerContainer { get; set; }

        static PMData()
        {
            PinsList = new List<PMPinModel>();
        }

        public static void AddToQueuePinsList(List<PMPinModel> list)
        {
            PinsList.AddRange(list);
        }
        public static void AddToQueuePinsList(PMPinModel pin)
        {
            PinsList.Add(pin);
        }
    }
}
