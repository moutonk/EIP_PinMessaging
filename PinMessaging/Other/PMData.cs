using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Phone.Maps.Controls;
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
        [DefaultValue(false)] public static bool WasFavoriteAddedSuccess { get; set; }
        [DefaultValue(false)] public static bool WasFavoriteRemovedSuccess { get; set; }

        //offline mode / normal mode
        public enum ApplicationMode { Normal, Offline }
        [DefaultValue(ApplicationMode.Normal)] public static ApplicationMode AppMode { get; set; }
 
        [DefaultValue(false)]
        public static bool NetworkProblem { get; set; }

        //pins to add to the MapLayerContainer
        [DefaultValue(null)] public static List<PMPinModel> PinsList { get; set; }
        [DefaultValue(null)] public static List<PMPinModel> PinsListToAdd { get; set; }

        //contains all the favorites
        [DefaultValue(null)] public static List<PMUserModel> UserList { get; set; }

        //contain all the pins
        [DefaultValue(null)] public static MapLayer MapLayerContainer { get; set; }

        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsList { get; set; }
        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsListTmp { get; set; }

        [DefaultValue(null)] public static PMUserModel User { get; set; }

        //contains all the pins known (serialized)
        private const string DataPinsFile = "pinsStorage.dat";

        //contains all the contacts (serialized)
        private const string DataContactsFile = "contactsStorage.dat";

        static PMData()
        {
            PinsList = new List<PMPinModel>();
            PinsListToAdd = new List<PMPinModel>();
            PinsCommentsList = new List<PMPinCommentModel>();
            PinsCommentsListTmp = new List<PMPinCommentModel>();
            UserList = new List<PMUserModel>();
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
        public static void AddToQueueUserList(PMUserModel user)
        {
            UserList.Add(user);
        }
        public static void RemoveToUserList(PMUserModel user)
        {
            try
            {
                var value = UserList.Find(userInList => userInList.Id == user.Id);

                if (value != null)
                {

                    Logs.Output.ShowOutput(UserList.Remove(value).ToString());
                    Logs.Output.ShowOutput("^^^^^^^^^^^^^^^^^^^^^^^^^^^: " + UserList.Count.ToString());
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }
      
        public enum StoredDataType
        {
            Pins,
            Favorites
        }

        public async static void LoadPins()
        {
            Logs.Output.ShowOutput("------------------------LOAD PINS BEGIN------------------");

            var serializer = new JsonSerializer();

            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + DataPinsFile) == true)
            {
                // Get the app data folder and create or replace the file we are storing the JSON in.            
                var textFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DataPinsFile);

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

        public async static Task<bool> SaveData(StoredDataType dataType)
        {

            string dataFilePath = null;
            Object list = null;

            switch (dataType)
            {
                case StoredDataType.Pins:
                    Logs.Output.ShowOutput("------------------------SAVE PINS BEGIN------------------");
                    dataFilePath = DataPinsFile;
                    list = PinsList;
                    break;
                case StoredDataType.Favorites:
                    Logs.Output.ShowOutput("------------------------SAVE FAVORITES BEGIN------------------");
                    dataFilePath = DataContactsFile;
                    list = UserList;
                    break;
            }

            var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFile textFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(dataFilePath, CreationCollisionOption.ReplaceExisting);

            // write the JSON string!
            using (var sw = new StreamWriter(textFile.Path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, list);
            }
            Logs.Output.ShowOutput("------------------------SAVE END------------------");

            return true;
        }

        public async static void LoadFavorites()
        {
            Logs.Output.ShowOutput("------------------------LOAD FAVORITES BEGIN------------------");

            var serializer = new JsonSerializer();

            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + DataContactsFile) == true)
            {
                // Get the app data folder and create or replace the file we are storing the JSON in.            
                var textFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DataContactsFile);

                // read the JSON string!
                using (var sw = new StreamReader(textFile.Path))
                using (JsonReader reader = new JsonTextReader(sw))
                {
                    var list = serializer.Deserialize<List<PMUserModel>>(reader);

                    if (list != null)
                    {
                        Logs.Output.ShowOutput("Deserialized " + list.Count.ToString() + " favorites");

                        foreach (var pmUserModel in list)
                        {
                            PMMapContactController.AddNewFavoris(pmUserModel);
                        }
                    }
                }
            }
            else
            {
                Logs.Output.ShowOutput("Storage file does not exist");
            }
            Logs.Output.ShowOutput("------------------------LOAD FAVORITES END------------------");
        }
    }
}
