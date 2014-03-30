using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Utils;

namespace PinMessaging.Other
{
    public static class PMData
    {
        //sign in / sign up
        [DefaultValue(false)] public static bool IsSignInSuccess { get; set; }
        [DefaultValue(false)] public static bool IsSignUpSuccess { get; set; }
        [DefaultValue(false)] public static bool IsEmailDispo { get; set; }
        [DefaultValue(false)] public static bool IsChangePwdSuccess { get; set; }
        [DefaultValue(false)] public static bool IsChangeEmailSuccess { get; set; }

        //offline mode / normal mode
        public enum ApplicationMode { Normal, Offline }
        [DefaultValue(ApplicationMode.Normal)] public static ApplicationMode AppMode { get; set; }
 
        [DefaultValue(false)]
        public static bool NetworkProblem { get; set; }

        //pins to add to the MapLayerContainer
        [DefaultValue(null)] public static List<PMPinModel> PinsList { get; set; }
        [DefaultValue(null)] public static List<PMPinModel> PinsListToAdd { get; set; }

        //contain all the pins
        [DefaultValue(null)] public static MapLayer MapLayerContainer { get; set; }

        static PMData()
        {
            PinsList = new List<PMPinModel>();
            PinsListToAdd = new List<PMPinModel>();
        }

        public static void AddToQueuePinsList(List<PMPinModel> list)
        {
            PinsListToAdd.AddRange(list);
        }
        public static void AddToQueuePinsList(PMPinModel pin)
        {
            PinsListToAdd.Add(pin);
        }

        public async static void LoadPins()
        {
            JsonSerializer serializer = new JsonSerializer();

            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.GetFileAsync("test.dat");

            // read the JSON string!
            using (StreamReader sw = new StreamReader(textFile.Path))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                PinsList = serializer.Deserialize<List<PMPinModel>>(reader);
                PMPinController pc = new PMPinController();

                foreach (var pmPinModel in PinsList)
                {
                    Logs.Output.ShowOutput("------------------------LOAD------------------");
                    pc.CompleteDataMember(pmPinModel);
                    pmPinModel.ShowPinContent();
                    PMMapPinController.AddPinToMap(pmPinModel);
                }
            }
        }

        private class Product
        {
            public string Name { get; set; }
            public Dictionary<string, string> Location { get; set; }
            public string NullTest { get; set; }
            public GeoCoordinate geo { get; set; }
            public Image img { get; set; }
        }

        public async static Task<bool> SavePins()
        {
            //List<Product> list = new List<Product>();

            //Product product = new Product();
            //product.Name = "Apple";
            //product.NullTest = null;
            //product.geo = new GeoCoordinate() {Longitude = 12.000, Latitude = 45.22222};
            //product.img = new Image() {Source = new BitmapImage(Paths.PinEvent)};
            //product.Location = new Dictionary<string, string>()
            //{
            //    {"Test", "mdr"},
            //    {"Ligne2", "plouf"}
            //};
            //Product product1 = new Product();
            //product1.Name = "Pomme";
            //product1.NullTest = null;
            //product1.Location = new Dictionary<string, string>()
            //{
            //    {"qsdsqd", "aaaaa"},
            //    {"sqs", "dsqdsqd"}
            //};

            //list.Add(product);
            //list.Add(product1);


            //string output = JsonConvert.SerializeObject(list);

            //Logs.Output.ShowOutput(output);
            Logs.Output.ShowOutput("------------------------SAVEPINS------------------");
            var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};

            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFile textFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("test.dat", CreationCollisionOption.ReplaceExisting);

            // write the JSON string!
            using (var sw = new StreamWriter(textFile.Path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                Logs.Output.ShowOutput("------------------------SAVE BEFORE------------------");
                serializer.Serialize(writer, PinsList);
                //Logs.Output.ShowOutput(JsonConvert.SerializeObject(PinsList));
                Logs.Output.ShowOutput("------------------------SAVE AFTER------------------");
            }
            return true;
        }
    }
}
