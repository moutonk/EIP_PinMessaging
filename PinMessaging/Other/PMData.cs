
namespace PinMessaging.Other
{
    public static class PMData
    {
        public static bool IsSignInSuccess { get; set; }
        public static bool IsSignUpSuccess { get; set; }
        public static bool IsEmailDispo { get; set; }
        public static bool NetworkProblem { get; set; }

        static PMData()
        {
            IsSignInSuccess = false;
            IsEmailDispo = false;
            IsSignUpSuccess = false;
            NetworkProblem = false;
        }
    }
}
