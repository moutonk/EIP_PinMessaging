using System;
using System.Collections.Generic;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMUserController : PMWebServiceEndDetector
    {
        private readonly Action _updateUiMethod;

        public PMUserController(RequestType currentRequestType, Action updateUi)
        {
            CurrentRequestType = currentRequestType;
            _updateUiMethod = updateUi;
        }

        public void GetUserInfos(string userId)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"id", userId},
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.User, SyncType.Async, dictionary, null);

            StartTimer();
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                //when we are here it means that the webservice call is over and the data are parsed and stocked in PMData
                StopTimer();

                switch (CurrentRequestType)
                {
                    case RequestType.User:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                }
            }
        }
    }
}
