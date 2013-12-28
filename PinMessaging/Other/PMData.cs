
using Microsoft.Phone.Maps.Controls;

namespace PinMessaging.Other
{
    public static class PMData
    {
        //sign in / sign up
        public static bool IsSignInSuccess { get; set; }
        public static bool IsSignUpSuccess { get; set; }
        public static bool IsEmailDispo { get; set; }
        public static bool NetworkProblem { get; set; }

        //contain all the pins
        public static MapLayer MapLayerContainer { get; set; }

        static PMData()
        {
            IsSignInSuccess = false;
            IsEmailDispo = false;
            IsSignUpSuccess = false;
            NetworkProblem = false;
            MapLayerContainer = null;
        }
    }
}
