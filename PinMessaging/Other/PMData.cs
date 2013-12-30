
using System.ComponentModel;
using Microsoft.Phone.Maps.Controls;

namespace PinMessaging.Other
{
    public static class PMData
    {
        //sign in / sign up
        [DefaultValue(false)] public static bool IsSignInSuccess { get; set; }
        [DefaultValue(false)] public static bool IsSignUpSuccess { get; set; }
        [DefaultValue(false)] public static bool IsEmailDispo { get; set; }
        [DefaultValue(false)] public static bool NetworkProblem { get; set; }

        //contain all the pins
        [DefaultValue(null)] public static MapLayer MapLayerContainer { get; set; }
    }
}
