using System;
using System.Diagnostics;

namespace PinMessaging.Utils
{
    public static class Logs
    {
        public static class Error
        {
            public enum ErrorsPriority { Critical, NotCritical };

            public static void ShowError(Exception exp, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + Environment.NewLine + exp.StackTrace + ": " + exp.Message + Environment.NewLine);
            }

            public static void ShowError(string msg, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + ": " + msg);
            }

            public static void ShowError(string msg, Exception exp, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + ": " + msg + ":" + Environment.NewLine + exp.Message);
            }
        }

        public static class Output
        {
            [Conditional("DEBUG")]
            public static void ShowOutput(string output)
            {
                Debug.WriteLine(output);
            }
        }
    }
}
