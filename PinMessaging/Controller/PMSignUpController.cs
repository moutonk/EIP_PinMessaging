using System;
using System.Collections.Generic;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using PinMessaging.Model;
using PinMessaging.Other;

namespace PinMessaging.Controller
{
    class PMSignUpController : PMEmailController
    {
        public PMSignUpController(Func<RequestType, PMLogInCreateStructureModel.ActionType, bool, bool> changeProgressBarState, RequestType currentRequestType, PMLogInCreateStructureModel.ActionType parentRequestType) :
            base(changeProgressBarState, currentRequestType, parentRequestType)
        {
            UpdateUi = changeProgressBarState;
            CurrentRequestType = currentRequestType;
            ParentRequestType = parentRequestType;
        }

        public bool SignUp(PMLogInModel logInModel)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"email", logInModel.Email},
                {"login", logInModel.Email},
                {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1(logInModel.Password))},
                {"simId", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1(logInModel.PhoneSimId))}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignUp, SyncType.Async, dictionary, null);

            StartTimer();

            return true;
        }
    }
}
