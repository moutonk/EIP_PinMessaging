using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PinMessaging.Controller;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PMPinModel
    {
        public enum PinsType
        {
            PublicMessage = 0,
            PrivateMessage,

            Event,
            PrivateEvent,
            
            View,
            PrivateView,
            
            CourseStart,
            PrivateCourseStart,
            
            CourseNextStep,
            PrivateCourseNextStep,
            
            CourseLastStep,
            PrivateCourseLastStep,

            Default
        }

        public enum PinsContentType
        {
            Text = 0,
            Image,
            Video
        }

        [JsonProperty] [DefaultValue(null)] public string Title { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Author { get; set; }
        [JsonProperty] [DefaultValue(null)] public string AuthorId { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Id { get; set; }
        [JsonProperty] [DefaultValue(null)] public string PinType { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Content { get; set; }
        [JsonProperty] [DefaultValue(null)] public string ContentType { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Url { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Lang { get; set; }
        [JsonProperty] [DefaultValue(null)] public string CreationTime { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Latitude { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Longitude { get; set; }
        [JsonProperty] [DefaultValue(null)] public string LocationName { get; set; }
        [JsonProperty] [DefaultValue(PinsType.PublicMessage)] public PinsType PinTypeEnum { get; set; }
                       [DefaultValue(null)] public Image PinImg { get; set; }
        [JsonProperty] [DefaultValue(null)] public GeoCoordinate GeoCoord { get; set; }
        //[JsonProperty] public Dictionary<string, string> Location { get; set; }

        
        private void ConvertTypeToEnum()
        {
            if (PinType != null)
            {
                if (PinType.Equals("Public_msg"))
                    PinTypeEnum = PinsType.PublicMessage;
                else if (PinType.Equals("Private_msg"))
                    PinTypeEnum = PinsType.PrivateMessage;
                else if (PinType.Equals("Event"))
                    PinTypeEnum = PinsType.Event;
                else if (PinType.Equals("View_point"))
                    PinTypeEnum = PinsType.View;
                else
                    PinTypeEnum = PinsType.PrivateMessage;
            }
        }

        private void ConvertGeoPosToInteger()
        {
            try
            {
                if (Latitude != null && Longitude != null)
                    GeoCoord = new GeoCoordinate(Double.Parse(Latitude, CultureInfo.InvariantCulture),
                                                 Double.Parse(Longitude, CultureInfo.InvariantCulture));
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
                GeoCoord = new GeoCoordinate(0, 0);
            }
        }

        private void ConnectImg()
        {
            PinImg = new Image { Source = Paths.PinsMapImg[PinTypeEnum] };
            PinImg.Tap += img_Tap;
        }

        [OnDeserialized]
        private void CompleteDataMember(StreamingContext context)
        {
            ConvertGeoPosToInteger();
            ConvertTypeToEnum();

           // if (Location.ContainsKey("name") == true)
            //   Title = Location["name"];

            //if it's the main thread
            if (Deployment.Current.Dispatcher.CheckAccess() == false)
                //we invoke the UI thread
                Deployment.Current.Dispatcher.BeginInvoke(ConnectImg);
            else //otherwise it's already in the UI thread
                ConnectImg();
        }

        public void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logs.Output.ShowOutput("Pin tapped!");
            PMMapPinController.OnPinTapped(this);
        }

        public void ShowPinContent()
        {
            try
            {
                Logs.Output.ShowOutput("id:" + Id + " type:" + PinType + " typeEnum:" + PinTypeEnum.ToString() +
                                       " description:" + Content +
                                       " url:" + Url + " lang:" + Lang + " creationTime:" + CreationTime + " author:" +
                                       AuthorId +
                                       " authorId: " + AuthorId + " locationName: " + LocationName);
                if (GeoCoord != null)
                    Logs.Output.ShowOutput(" geoPos Lat:" + GeoCoord.Latitude + " geoPos Long:" + GeoCoord.Longitude );

               /* foreach (var item in Location)
                {
                    Logs.Output.ShowOutput("\t" + item.Key + ":" + item.Value);
                }*/
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("Pin content is incorrectly formatted", Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }
}
