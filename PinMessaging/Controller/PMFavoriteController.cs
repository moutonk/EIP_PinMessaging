using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMFavoriteController : PMWebServiceEndDetector
    {
        private  Action _updateUiMethod;

        public PMFavoriteController(RequestType currentRequestType, Action updateUi)
        {
            CurrentRequestType = currentRequestType;
            _updateUiMethod = updateUi;
        }

        public void AddFavoriteUser(string userId)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"favoriteId", userId},
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.AddFavoriteUser, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void RemoveFavoriteUser(string userId)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"favoriteId", userId},
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.RemoveFavoriteUser, SyncType.Async, dictionary, null);

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
                    case RequestType.AddFavoriteUser:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                    case RequestType.RemoveFavoriteUser:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                            _updateUiMethod = null;
                        break;
                }
            }
        }
    }
}
