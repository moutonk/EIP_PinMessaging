using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using System;
using System.Collections.Generic;

namespace PinMessaging.Controller
{
    class PMEmailController : PMWebServiceEndDetector
    {
        public PMEmailController(Func<RequestType, PMLogInCreateStructureModel.ActionType, bool, bool> changeProgressBarState, RequestType currentRequestType, PMLogInCreateStructureModel.ActionType parentRequestType)
        {
            UpdateUi = changeProgressBarState;
            CurrentRequestType = currentRequestType;
            ParentRequestType = parentRequestType;
        }

        public void CheckEmailExists(PMLogInModel logInModel)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"email", logInModel.Email},
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

            StartTimer();
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                StopTimer();

                //For request with a change of UI
                if (UpdateUi != null)
                {
                    switch (CurrentRequestType)
                    {
                            case RequestType.CheckEmail:
                                UpdateUi(CurrentRequestType, ParentRequestType, PMData.IsEmailDispo);
                                break;
                            case RequestType.SignUp:
                                UpdateUi(CurrentRequestType, ParentRequestType, PMData.IsSignUpSuccess);
                                break;
                            case RequestType.SignIn:
                                UpdateUi(CurrentRequestType, ParentRequestType, PMData.IsSignInSuccess);
                                break;
                    }
                    UpdateUi = null;

                    //if (PMData.NetworkProblem == true)
                    //{
                    //    Utils.Utils.CustomMessageBox(new[] { "Ok" }, "Oops !", AppResources.NetworkProblem);
                    //    UpdateUi = null;
                    //}
               }
               //For request without UI change
               else if (ChangeView != null)
               {
                    switch (CurrentRequestType)
                    {
                        case RequestType.SignIn:
                            ChangeView();
                            break;
                    }
                }
            }
        }
    }
}
