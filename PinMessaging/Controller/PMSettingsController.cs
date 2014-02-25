using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMSettingsController : PMWebServiceEndDetector
    {
        public PMSettingsController(RequestType currentRequestType)
        {
            CurrentRequestType = currentRequestType;
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

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                //when we are here it means that the webservice call is over and the data are parsed and stocked in PMData
                StopTimer();

                switch (CurrentRequestType)
                {
                    case RequestType.ChangePassword:
                        MessageBox.Show("ChangePwdStatus: " + PMData.IsChangePwdSuccess);
                        break;
                }
            }
        }
    }
}
