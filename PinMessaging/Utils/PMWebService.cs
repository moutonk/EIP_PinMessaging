﻿using System.ComponentModel;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using PinMessaging.Other;
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
            CheckEmail
        }
    }

    class PMWebService
    {
        private static readonly PMDataConverter DataConverter = new PMDataConverter();
        public static bool OnGoingRequest { get; set; }

        private static string RequestTypeToUrlString(RequestType reqType)
        {
            switch (reqType)
            {
                case RequestType.CheckEmail:
                    return "check-email";
                case RequestType.SignUp:
                    return "new-user";
                case RequestType.SignIn:
                    return "connect";
                default:
                    return reqType.ToString();
            }
        }

        public static void SendRequest(HttpRequestType httpReqType, RequestType reqType, SyncType syncType, Dictionary<string, string> args, Dictionary<string, string> header)
        {
            OnGoingRequest = true;

            //Debug output
            Debug.WriteLine(Environment.NewLine + "SendRequest: " + httpReqType.ToString() + " " + reqType.ToString() + " " + syncType.ToString() + " " + args.Aggregate("",(current, keyValuePair) =>current + ("[" + keyValuePair.Key + " " + keyValuePair.Value + "]")));

            //convert the dictionnary to a string
            string dicoToString = FormateDictionnaryToString(args);

            //create the request with the correct URL
            string url = Paths.ServerAddress + RequestTypeToUrlString(reqType) + ".json";
            Debug.WriteLine(url + "?" + dicoToString);

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

        private static void ManageResponse(IAsyncResult ar)
        {
            Debug.WriteLine("Waiting answer BEGIN...");

            //get the tuple object contained in ar
            var tuple = (Tuple<HttpWebRequest, byte[], RequestType>)ar.AsyncState;

            HttpWebResponse response = null;
            Stream streamResponse = null;
            StreamReader streamRead = null;
            string responseString = null;

            try
            {
                response = (HttpWebResponse)tuple.Item1.EndGetResponse(ar);
                streamResponse = response.GetResponseStream();
                streamRead = new StreamReader(streamResponse);

                responseString = streamRead.ReadToEnd();

                Debug.WriteLine("Answer: " + responseString);

                CloseStream(streamResponse);
                CloseStream(streamRead);
                CloseStream(response);
            }
            catch (WebException e)
            {
                ManageResponseExplicitError(e);

                CloseStream(streamResponse);
                CloseStream(streamRead);
                CloseStream(response);
            }

            DataConverter.ParseJson(responseString, (RequestType)tuple.Item3);

            OnGoingRequest = false;

            Debug.WriteLine("Waiting answer END...");
        }

        private static void ManageResponseExplicitError(WebException e)
        {
            Debug.WriteLine("GetResponseCallback: " + e.Message + ": " + e.InnerException.Message);
            var aResp = e.Response as HttpWebResponse;

            if (aResp != null)
            {
                Debug.WriteLine("statusCode: " + (int)aResp.StatusCode);
            }

            using (var reader = new StreamReader(e.Response.GetResponseStream()))
            {
                Debug.WriteLine(reader.ReadToEnd());
            }
        }

        private static void PostRequest(ref string url, ref string parameters, RequestType reqType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";

            //convert the dictionnary with the argument to an array of bytes
            byte[] requestParams = Encoding.UTF8.GetBytes(parameters);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestParams.Length;

            // start the asynchronous operation
            request.BeginGetRequestStream(new AsyncCallback(WriteParamsInStreamCallBack), Tuple.Create(request, requestParams, reqType));
        }

        private static void WriteParamsInStreamCallBack(IAsyncResult ar)
        {
            Debug.WriteLine("Writing request BEGIN...");

            //get the tuple object contained in ar
            var tuple = (Tuple<HttpWebRequest, byte[], RequestType>)ar.AsyncState;
   
            // End the operation
            Stream postStream = tuple.Item1.EndGetRequestStream(ar);

            //write the params in the request
            postStream.Write(tuple.Item2, 0, tuple.Item2.Length);
            postStream.Flush();
            CloseStream(postStream);

            Debug.WriteLine("Writing request END...");

            // Start the asynchronous operation to get the response
            tuple.Item1.BeginGetResponse(new AsyncCallback(ManageResponse), ar.AsyncState);
        }

        private static void GetRequest(ref string url, ref string parameters, RequestType reqType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + "?" + parameters);

            request.Method = "GET";

            // start the asynchronous operation
            request.BeginGetResponse(new AsyncCallback(ManageResponse), Tuple.Create(request, (object)null, reqType));
        }

        private static string FormateDictionnaryToString(Dictionary<string, string> dict)
        {
            var builder = new StringBuilder();

            Debug.WriteLine("-------------------------------------------");
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                builder.Append(kvp.Key + "=" + kvp.Value + "&");
                Debug.WriteLine(kvp.Key + " " + kvp.Value);
            }
            Debug.WriteLine("-------------------------------------------");
            return builder.ToString();
        }

        private static void CloseStream(Stream s) 
        {
            if (s != null)
                s.Close();
        }

        private static void CloseStream(HttpWebResponse s)
        {
            if (s != null)
                s.Close();
        }

        private static void CloseStream(StreamReader s)
        {
            if (s != null)
                s.Close();
        }
    }
}