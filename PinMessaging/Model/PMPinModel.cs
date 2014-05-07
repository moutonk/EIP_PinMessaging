using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
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
            PrivateCourseLastStep
        }

        [JsonProperty] [DefaultValue(null)] public string PinTitle { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Id { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Type { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Description { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Url { get; set; }
        [JsonProperty] [DefaultValue(null)] public string Lang { get; set; }
        [JsonProperty] [DefaultValue(null)] public string CreationTime { get; set; }
        [JsonProperty] public Dictionary<string, string> Location { get; set; }

        [JsonProperty] [DefaultValue(PinsType.PublicMessage)] public PinsType PinTypeEnum { get; set; }
                       [DefaultValue(null)] public Image PinImg { get; set; }
        [JsonProperty] [DefaultValue(null)] public GeoCoordinate GeoCoord { get; set; }
        
        /*public PMPinModel(PinsType type, GeoCoordinate pos)
        {
            PinImg = new Image();
            GeoCoord = pos;
            PinTypeEnum = type;
        }

        public void CompleteInitialization(string name, string content)
        {
            PinTitle = name;
            Description = content;
        }*/

        public void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logs.Output.ShowOutput("Pin tapped!");
            PMMapPinController.OnPinTapped(this);
        }

        public void ShowPinContent()
        {
            try
            {
                Logs.Output.ShowOutput("id:" + Id + " type:" + Type + " typeEnum:" + PinTypeEnum.ToString() + " description:" + Description + " url:" + Url + " lang:" + Lang + " creationTime:" + CreationTime + " location:");
                foreach (var item in Location)
                {
                    Logs.Output.ShowOutput("\t" + item.Key + ":" + item.Value);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("Pin content is incorrectly formatted", Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }
}
