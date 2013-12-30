using System;
using System.Windows.Threading;
using PinMessaging.Model;
using PinMessaging.Utils.WebService;
using System.Windows;

namespace PinMessaging.Utils
{
    public class PMWebServiceEndDetector
    {
        protected DispatcherTimer WaitAnswerTimer;
        protected Func<RequestType, PMLogInCreateStructureModel.ActionType, bool, bool> UpdateUi;
        protected Func<bool> ChangeView;
        protected RequestType CurrentRequestType;
        protected PMLogInCreateStructureModel.ActionType ParentRequestType;

        protected PMWebServiceEndDetector()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                WaitAnswerTimer = new DispatcherTimer();
                WaitAnswerTimer.Tick += waitEnd_Tick;
                WaitAnswerTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            });

            UpdateUi = null;
            ChangeView = null;
         }

        protected void StartTimer()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => WaitAnswerTimer.Start());
        }

        protected void StopTimer()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => WaitAnswerTimer.Stop());
        }

        //function called peridodicaly (and so the overridden function inherited)
        protected virtual void waitEnd_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
