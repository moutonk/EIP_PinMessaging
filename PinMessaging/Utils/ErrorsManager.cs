using System;
using System.Diagnostics;

namespace PinMessaging.Utils
{
    public enum ErrorsPriority { Critical, NotCritical };

    static class ErrorsManager
    {
        public static void ShowError(Exception exp, ErrorsPriority prio)
        {
            Debug.WriteLine(Environment.NewLine + "Priority: " + prio + Environment.NewLine + exp.StackTrace + ": " + exp.Message + Environment.NewLine);
        }

        public static void ShowError(string error, ErrorsPriority prio)
        {
            Debug.WriteLine("Priority: " + prio + Environment.NewLine + error);
        }
    }
}
