using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Shell;

namespace PinMessaging.Utils
{
    public static class Logs
    {
        public static class Error
        {
            public enum ErrorsPriority { Critical, NotCritical };

            private static void CreateToast(string title, string content)
            {
                try
                {
                    //for phone
                    MessageBox.Show(content);
                }
                catch (Exception exp)
                {
                }
            }

            public static void ShowError(Exception exp, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + Environment.NewLine + exp.StackTrace + ": " + exp.Message + Environment.NewLine);
                CreateToast("", exp.Message);
            }

            public static void ShowError(string msg, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + ": " + msg);
                CreateToast("", msg);
            }

            public static void ShowError(string msg, Exception exp, ErrorsPriority prio)
            {
                Output.ShowOutput(Environment.NewLine + "Priority: " + prio + ": " + msg + ":" + Environment.NewLine + exp.Message);
                CreateToast("", msg);
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
