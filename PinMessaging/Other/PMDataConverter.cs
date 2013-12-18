using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Other
{
    class PMDataConverter
    {
        public void ParseJson(string json, RequestType currentRequestType)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (currentRequestType == RequestType.SignIn)
                    PMData.IsSignInSuccess = Boolean.Parse((string)item[0]);

                if (currentRequestType == RequestType.CheckEmail)
                    PMData.IsEmailDispo = Boolean.Parse((string)item[0]);

                if (currentRequestType == RequestType.SignUp)
                    PMData.IsSignUpSuccess = Boolean.Parse((string)item[0]);
            }
            catch (Exception e)
            {
                ErrorsManager.ShowError(e, ErrorsPriority.NotCritical);
            }
            
        }
    }   
}
