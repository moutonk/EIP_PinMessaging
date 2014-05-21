using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Phone.Maps.Controls;
using Newtonsoft.Json;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Utils;
using PinMessaging.View;

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

        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsList { get; set; }
        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsListTmp { get; set; }

        [DefaultValue(null)] public static PMUserModel User { get; set; }

        //contains all the pins known (serialized)
        private const string DataFile = "pinsStorage.dat";

        static PMData()
        {
            PinsList = new List<PMPinModel>();
            PinsListToAdd = new List<PMPinModel>();
            PinsCommentsList = new List<PMPinCommentModel>();
            PinsCommentsListTmp = new List<PMPinCommentModel>();
        }

        public static void AddToQueuePinsList(List<PMPinModel> list)
        {
            PinsListToAdd.AddRange(list);
        }
        public static void AddToQueuePinsList(PMPinModel pin)
        {
            PinsListToAdd.Add(pin);
        }
        public static void AddToQueuePinComments(List<PMPinCommentModel> comments)
        {
            PinsCommentsList.AddRange(comments);
        }
        public static void AddToQueuePinCommentsTmp(List<PMPinCommentModel> comments)
        {
            PinsCommentsListTmp.AddRange(comments);
        }
        public static void AddToQueuePinCommentsTmp(PMPinCommentModel comment)
        {
            PinsCommentsListTmp.Add(comment);
        }

        public async static void LoadPins()
        {
            Logs.Output.ShowOutput("------------------------LOAD PINS BEGIN------------------");

            var serializer = new JsonSerializer();

            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + DataFile) == true)
            {
                // Get the app data folder and create or replace the file we are storing the JSON in.            
                var textFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DataFile);

                // read the JSON string!
                using (var sw = new StreamReader(textFile.Path))
                using (JsonReader reader = new JsonTextReader(sw))
                {
                    var list = serializer.Deserialize<List<PMPinModel>>(reader);
                    Logs.Output.ShowOutput("Deserialized " + list.Count.ToString() + " pins");                

                    var pc = new PMPinController();

                    foreach (var pmPinModel in list)
                    {
                        if (PMMapPinController.IsPinUnique(pmPinModel) == true)
                        {
                            //pc.CompleteDataMember(pmPinModel);
                            pmPinModel.ShowPinContent();
                            PMMapPinController.AddPinToMap(pmPinModel);
                            PinsList.Add(pmPinModel);
                        }
                    }
                }
            }
            else
            {
                Logs.Output.ShowOutput("Storage file does not exist");
            }
            Logs.Output.ShowOutput("------------------------LOAD PINS END------------------");
        }

        public async static Task<bool> SavePins()
        {
            Logs.Output.ShowOutput("------------------------SAVE PINS BEGIN------------------");

            var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFile textFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(DataFile, CreationCollisionOption.ReplaceExisting);

            // write the JSON string!
            using (var sw = new StreamWriter(textFile.Path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, PinsList);
            }
            Logs.Output.ShowOutput("------------------------SAVE PINS END------------------");

            return true;
        }
    }
}
