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
            if (json != null)
            {
                try
                {
                    var item = JsonConvert.DeserializeObject<JArray>(json);

                    switch (currentRequestType)
                    {
                        case RequestType.SignIn:
                            PMData.IsSignInSuccess = Boolean.Parse((string) item[0]);
                            break;

                        case RequestType.CheckEmail:
                            PMData.IsEmailDispo = Boolean.Parse((string) item[0]);
                            break;

                        case RequestType.SignUp:
                            PMData.IsSignUpSuccess = Boolean.Parse((string) item[0]);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logs.Error.ShowError(e, ErrorsPriority.NotCritical);
                }
            }
        }
    }   
}
