using System;
using System.Collections.Generic;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMSettingsController : PMWebServiceEndDetector
    {
        private readonly Action _updateUiMethodError;
        private readonly Action _updateUiMethodSuccess;

        public PMSettingsController(RequestType currentRequestType, Action updateUiError, Action updateUiSuccess)
        {
            CurrentRequestType = currentRequestType;
            _updateUiMethodError = updateUiError;
            _updateUiMethodSuccess = updateUiSuccess;
        }

        public void ChangePassword(string oldPwd, string newPwd)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"oldPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1(oldPwd))},
                {"newPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1(newPwd))}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangePassword, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void ChangeEmail(string newEmail)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"newEmail", newEmail}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangeEmail, SyncType.Async, dictionary, null);

            StartTimer();   
        }

        public void PostFeedback(string feedbackType, string comment, string extra)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"id", PMData.CurrentUserId},
                {"plateforme", "W"},
                {"type", feedbackType},
                {"comment", comment},
                {"extra", extra}
            };

            PMWebService.SendRequest(HttpRequestType.Get, RequestType.Feedback, SyncType.Async, dictionary, null);

            StartTimer();
            
        }

        private void DispatchRegarding(bool dispatchingValue)
        {
            if (_updateUiMethodError != null && _updateUiMethodSuccess != null)
            {
                if (dispatchingValue)
                    _updateUiMethodSuccess();
                else
                    _updateUiMethodError();
            }
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                //when we are here it means that the webservice call is over and the data are parsed and stocked in PMData
                StopTimer();

                switch (CurrentRequestType)
                {
                    case RequestType.ChangePassword:
                        DispatchRegarding(PMData.IsChangePwdSuccess);
                        break;
                    case RequestType.ChangeEmail:
                        DispatchRegarding(PMData.IsChangeEmailSuccess);
                        break;
                    case RequestType.Feedback:
                        DispatchRegarding(true);
                        break;
                }
            }
        }
    }
}
