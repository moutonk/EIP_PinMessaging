
using PinMessaging.Model;
using System.Collections.Generic;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using System;

namespace PinMessaging.Controller
{
    class PMSignInController : PMEmailController
    {
        public PMSignInController(Func<RequestType, PMLogInCreateStructureModel.ActionType, bool, bool> changeProgressBarState, Func<bool> changeView, RequestType currentRequest, PMLogInCreateStructureModel.ActionType parentRequest) :
            base(changeProgressBarState, currentRequest, parentRequest)
        {
            UpdateUi = changeProgressBarState;
            ChangeView = changeView;
            CurrentRequestType = currentRequest;
            ParentRequestType = parentRequest;
        }

        public void LogIn(PMLogInModel logInModel)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"login", logInModel.Email},
                {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1(logInModel.Password))}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignIn, SyncType.Async, dictionary, null);

            StartTimer();
        }
    }
}
