using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Windows.Controls;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    public class PMPinModel
    {
        public enum PinsType
        {
            CulturalMessage,
            Event,
            Message,
            PublicMessage,
            PrivateMessage,
            CourseLastStep,
            Eye,
            PointOfInterest
        }

        [DefaultValue(null)] private string PinTitle { get; set; }
        [DefaultValue(null)] public string Id { get; set; }
        [DefaultValue(null)] public string Type { get; set; }
        [DefaultValue(null)] public string Description { get; set; }
        [DefaultValue(null)] public string Url { get; set; }
        [DefaultValue(null)] public string Lang { get; set; }
        [DefaultValue(null)] public string CreationTime { get; set; }
        public Dictionary<string, string> Location { get; set; }       

        [DefaultValue(PinsType.PublicMessage)] public PinsType PinTypeEnum { get; set; }
        [DefaultValue(null)] public Image PinImg { get; set; }
        [DefaultValue(null)] public GeoCoordinate GeoCoord { get; set; }
        
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
        }

        public void ShowPinContent()
        {
            Logs.Output.ShowOutput("id:" + Id + " type:" + Type + " typeEnum:" + PinTypeEnum.ToString() + " description:" + Description + " url:" + Url + " lang:" + Lang + " creationTime:" + CreationTime + " location:");
            Logs.Output.ShowOutput("\tid:" + Location["id"] + " latitude:" + GeoCoord.Latitude.ToString() + " longitude:" + GeoCoord.Longitude.ToString() + " name:" + Location["name"]);
            foreach (var item in Location)
            {
                Logs.Output.ShowOutput("\t" + item.Key + ":" + item.Value);
            }
        }
    }
}
