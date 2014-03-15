using System.ComponentModel;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Utils
{
    namespace WebService
    {
        public enum HttpRequestType
        {
            [Description("Get requests")]
            Get,
            [Description("Post requests")]
            Post
        };

        public enum SyncType
        {
            [Description("Synchrone requests")]
            Sync,
            [Description("Asynchrone requests")]
            Async
        };

        public enum RequestType
        {
            [Description("Used for the connection")]
            SignIn,
            [Description("Used for the inscription")]
            SignUp,
            [Description("Used the check if the email is already taken")]
            CheckEmail,
            [Description("Used to change the password of an existing user")]
            ChangePassword,
            [Description("Used to change the email of an existing account")]
            ChangeEmail,
            [Description("Used to add a user as the current user's favorite")]
            AddFavoriteUser,
            [Description("Used to remove a user as the current user's favorite")]
            RemoveFavoriteUser,
            [Description("Used to add a location as the current user's favorite")]
            AddFavoriteLocation,
            [Description("Used to remove a user as the current user's favorite")]
            RemoveFavoriteLocation,
            [Description("Used to retrieve pins")]
            GetPins,
            [Description("Used to create a pin")]
            CreatePin
        }
    }

    public static class PMWebService
    {
        private static readonly PMDataConverter DataConverter = new PMDataConverter();
        public static bool OnGoingRequest { get; private set; }
        private static bool _firstRequest = true;
      
        private static CookieCollection _cookieColl = new CookieCollection();
        private static readonly CookieContainer CookieContainer = new CookieContainer();

        private static string RequestTypeToUrlString(RequestType reqType)
        {
            switch (reqType)
            {
                case RequestType.CheckEmail:
                    return "check-email";
                case RequestType.SignUp:
                    return "create-user";
                case RequestType.SignIn:
                    return "connect";
                case RequestType.AddFavoriteLocation:
                    return "addFavoriteLocation";
                case RequestType.AddFavoriteUser:
                    return "addFavoriteUser";
                case RequestType.RemoveFavoriteLocation:
                    return "removeFavoriteLocation";
                case RequestType.RemoveFavoriteUser:
                    return "removeFavoriteUser";
                case RequestType.ChangeEmail:
                    return "changeEmail";
                case RequestType.ChangePassword:
                    return "changePass";
                case RequestType.GetPins:
                    return "get-pins";
                case RequestType.CreatePin:
                    return "create-pin";
                default:
                    return reqType.ToString();
            }
        }

        public static void SendRequest(HttpRequestType httpReqType, RequestType reqType, SyncType syncType, Dictionary<string, string> args, Dictionary<string, string> header)
        {
            OnGoingRequest = true;
            PMData.NetworkProblem = false;

            //Debug output
            Logs.Output.ShowOutput(Environment.NewLine + "SendRequest: " + httpReqType.ToString() + " " + reqType.ToString() + " " + syncType.ToString() + " " + args.Aggregate("",(current, keyValuePair) =>current + ("[" + keyValuePair.Key + " " + keyValuePair.Value + "]")));

            //convert the dictionnary to a string
            var dicoToString = FormateDictionnaryToString(args);
            Logs.Output.ShowOutput("DicoToString request: " + dicoToString);

            //create the request with the correct URL
            var url = Paths.ServerAddress + RequestTypeToUrlString(reqType) + ".json";

            switch (httpReqType)
            {
                case HttpRequestType.Post:
                    PostRequest(ref url, ref dicoToString, reqType);
                    break;

                case HttpRequestType.Get:
                    GetRequest(ref url, ref dicoToString, reqType);
                    break;
            }
        }

        private static void ShowCookiesInfos(HttpWebResponse response)
        {
            foreach (Cookie cook in response.Cookies)
            {
                Logs.Output.ShowOutput("Cookie:");
                Logs.Output.ShowOutput(cook.Name + "=" + cook.Value);
                Logs.Output.ShowOutput("Domain: " + cook.Domain);
                Logs.Output.ShowOutput("Path: " + cook.Path);
                Logs.Output.ShowOutput("Port: " + cook.Port);
                Logs.Output.ShowOutput("Secure: " + cook.Secure);

                Logs.Output.ShowOutput("When issued:" + cook.TimeStamp);
                Logs.Output.ShowOutput("Expires: (expired?)" + cook.Expires + " " + cook.Expired);
                Logs.Output.ShowOutput("Don't save:" + cook.Discard);
                Logs.Output.ShowOutput("Comment: " + cook.Comment);
                Logs.Output.ShowOutput("Uri for comments: " + cook.CommentUri);
                Logs.Output.ShowOutput("Version: RFC " + (cook.Version == 1 ? "2109" : "2965"));

                // Show the string representation of the cookie.
                Logs.Output.ShowOutput("String: " + cook.ToString());
            }
        }

        private static void ManageResponse(IAsyncResult ar)
        {
            Logs.Output.ShowOutput("Waiting answer BEGIN...");

            //get the tuple object contained in ar
            var tuple = (Tuple<HttpWebRequest, byte[], RequestType>)ar.AsyncState;

            try
            {
                using (var response = (HttpWebResponse) tuple.Item1.EndGetResponse(ar))
                using (var streamResponse = response.GetResponseStream())
                using (var streamRead = new StreamReader(streamResponse))
                {
                    var responseString = streamRead.ReadToEnd();

                    if (_firstRequest == true)
                    {
                        _cookieColl = response.Cookies;
                        _firstRequest = false;
                    }

                    // Print the properties of each cookie. 
                    ShowCookiesInfos(response);

                    Logs.Output.ShowOutput("Answer: " + responseString);

                    DataConverter.ParseJson(responseString, (RequestType) tuple.Item3);
                }
            }
            catch (WebException e)
            {
                ManageResponseExplicitError(e);

                PMData.NetworkProblem = true;

                Utils.CustomMessageBox(new[] { "Ok" }, "Oops !", AppResources.NetworkProblem);
            }

            OnGoingRequest = false;

            Logs.Output.ShowOutput("Waiting answer END...");         
        }

        private static void ManageResponseExplicitError(WebException e)
        {
            Logs.Output.ShowOutput("GetResponseCallback: " + e.Message + ": " + e.InnerException.Message);
            var aResp = e.Response as HttpWebResponse;

            if (aResp != null)
            {
                Logs.Output.ShowOutput("statusCode: " + (int)aResp.StatusCode);
            }

            using (var reader = new StreamReader(e.Response.GetResponseStream()))
            {
                Logs.Output.ShowOutput(reader.ReadToEnd());
            }
        }

        private static void PostRequest(ref string url, ref string parameters, RequestType reqType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            Logs.Output.ShowOutput(url);

            if (_firstRequest == true)
                CookieContainer.Add(new Uri(Paths.ServerAddress), _cookieColl);

            request.CookieContainer = CookieContainer;
            request.Method = HttpRequestType.Post.ToString();

            //convert the dictionnary with the argument to an array of bytes
            byte[] requestParams = Encoding.UTF8.GetBytes(parameters);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestParams.Length;

            // start the asynchronous operation
            request.BeginGetRequestStream(new AsyncCallback(WriteParamsInStreamCallBack), Tuple.Create(request, requestParams, reqType));
        }

        private static void WriteParamsInStreamCallBack(IAsyncResult ar)
        {
            Logs.Output.ShowOutput("Writing request BEGIN...");

            //get the tuple object contained in ar
            var tuple = (Tuple<HttpWebRequest, byte[], RequestType>)ar.AsyncState;
   
            // End the operation
            using (var postStream = tuple.Item1.EndGetRequestStream(ar))
            {
                //write the params in the request
                postStream.Write(tuple.Item2, 0, tuple.Item2.Length);
                postStream.Flush();  
            }

            Logs.Output.ShowOutput("Writing request END...");

            // Start the asynchronous operation to get the response
            tuple.Item1.BeginGetResponse(new AsyncCallback(ManageResponse), ar.AsyncState);
        }

        private static void GetRequest(ref string url, ref string parameters, RequestType reqType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + "?" + parameters);

            if (_firstRequest == true)
                CookieContainer.Add(new Uri(Paths.ServerAddress), _cookieColl);

            request.CookieContainer = CookieContainer;

            Logs.Output.ShowOutput(url + "?" + parameters);

            request.Method = HttpRequestType.Get.ToString();

            // start the asynchronous operation
            request.BeginGetResponse(new AsyncCallback(ManageResponse), Tuple.Create(request, new byte[1], reqType));
        }

        private static string FormateDictionnaryToString(IReadOnlyCollection<KeyValuePair<string, string>> dict)
        {
            var builder = new StringBuilder();

            Logs.Output.ShowOutput("-------------------------------------------");

            for (var dicoLine = 0; dicoLine < dict.Count; dicoLine++)
            {
                builder.Append(dict.ElementAt(dicoLine).Key + "=" + dict.ElementAt(dicoLine).Value);
                if (dicoLine + 1 < dict.Count)
                    builder.Append("&");
            }
            
            Logs.Output.ShowOutput("-------------------------------------------");
            return builder.ToString();
        }
    }
}