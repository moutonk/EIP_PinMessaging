using System;
using System.Windows.Threading;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils.WebService;
using System.Diagnostics;

namespace PinMessaging.Utils
{
    class PMWebServiceEndDetector
    {
        protected readonly DispatcherTimer WaitAnswerTimer = new DispatcherTimer();
        protected Func<RequestType, PMLogInCreateStructureModel.ActionType, bool, bool> UpdateUi;
        protected Func<bool> ChangeView;
        protected RequestType CurrentRequestType;
        protected PMLogInCreateStructureModel.ActionType ParentRequestType;

        protected PMWebServiceEndDetector()
        {
            UpdateUi = null;
            ChangeView = null;
            WaitAnswerTimer.Tick += waitEnd_Tick;
            WaitAnswerTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        }

        //function called peridodicaly (and so the overridden function inherited)
        protected virtual void waitEnd_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
