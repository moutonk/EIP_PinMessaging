using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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
        //[DefaultValue(null)] public static Image UserProfilPicture { get; set; }
        [DefaultValue(null)] public static string UserId { get; set; }
        [DefaultValue(null)] public static string CurrentUserId { get; set; }
        [DefaultValue(null)] public static string AuthId { get; set; }
        [DefaultValue(false)] public static bool IsSignInSuccess { get; set; }
        [DefaultValue(false)] public static bool IsSignUpSuccess { get; set; }
        [DefaultValue(false)] public static bool IsEmailDispo { get; set; }
        [DefaultValue(false)] public static bool IsChangePwdSuccess { get; set; }
        [DefaultValue(false)] public static bool IsChangeEmailSuccess { get; set; }
        [DefaultValue(false)] public static bool WasFavoriteAddedSuccess { get; set; }
        [DefaultValue(false)] public static bool WasFavoriteRemovedSuccess { get; set; }
        [DefaultValue(false)] public static bool WasPinDeletedSuccess { get; set; }

        //offline mode / normal mode
        public enum ApplicationMode { Normal, Offline }
        [DefaultValue(ApplicationMode.Normal)] public static ApplicationMode AppMode { get; set; }
 
        [DefaultValue(false)]
        public static bool NetworkProblem { get; set; }

        public static string CurrentLanguge = Thread.CurrentThread.CurrentUICulture.Name;

        //neutral profil pic
        [DefaultValue(null)] public static BitmapImage NeutralProfilPic { get; set; }

        //pins to add to the MapLayerContainer
        [DefaultValue(null)] public static List<PMPinModel> PinsList { get; set; }
        [DefaultValue(null)] public static List<PMPinModel> PinsListToAdd { get; set; }

        //contains all the favorites
        [DefaultValue(null)] public static List<PMUserModel> UserList { get; set; }

        //contains all the searched users
        [DefaultValue(null)] public static List<PMUserModel> SearchUserList { get; set; }

        //contains all the users profil pictures
        [DefaultValue(null)] public static List<PMPhotoModel> ProfilPicturesList { get; set; }

        //contain all the pins
        [DefaultValue(null)] public static MapLayer MapLayerContainer { get; set; }

        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsList { get; set; }
        [DefaultValue(null)] public static List<PMPinCommentModel> PinsCommentsListTmp { get; set; }

        [DefaultValue(null)] public static PMUserModel User { get; set; }

        //contains all the user history
        [DefaultValue(null)] public static List<PMHistoryModel> UserHistoryList { get; set; }

        //contains the users profil picture
        [DefaultValue(null)] public static byte[] UserProfilPicture { get; set; }

        //contains all the pins known (serialized)
        private const string DataPinsFile = "pinsStorage.dat";

        //contains all the contacts (serialized)
        private const string DataContactsFile = "contactsStorage.dat";

        //contains all the pictures (serialized)
        private const string DataPicturesFile = "picturesStorage.dat";

        static PMData()
        {
            PinsList = new List<PMPinModel>();
            PinsListToAdd = new List<PMPinModel>();
            PinsCommentsList = new List<PMPinCommentModel>();
            PinsCommentsListTmp = new List<PMPinCommentModel>();
            UserList = new List<PMUserModel>();
            SearchUserList = new List<PMUserModel>();
            //UserProfilPicture = new Image();
            ProfilPicturesList = new List<PMPhotoModel>();
            //UserHistoryList = new List<PMHistoryModel>();
            NeutralProfilPic = new BitmapImage(new Uri("/Images/Icons/neutral_profil.jpg", UriKind.Relative));

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
                    UserList.Remove(value);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }
        public static PMPhotoModel GetUserProfilPicture(string id)
        {
            return ProfilPicturesList.Any(pics => pics.UserId.Equals(id) == true) == true
                ? ProfilPicturesList.Find(pics => pics.UserId.Equals(id) == true)
                : null;
        }

        public enum StoredDataType
        {
            Pins,
            Favorites,
            Pictures
        }

        public async static void LoadPins()
        {
            Logs.Output.ShowOutput("------------------------LOAD PINS BEGIN------------------");

            var serializer = new JsonSerializer();

            try
            {
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

                        foreach (var pmPinModel in list.Where(pmPinModel => PMMapPinController.IsPinUnique(pmPinModel) == true))
                        {
                            //pmPinModel.ShowPinContent();
                            PMMapPinController.AddPinToMap(pmPinModel);
                            PinsList.Add(pmPinModel);
                        }
                    }
                }
                else
                {
                    Logs.Output.ShowOutput("Storage file does not exist");
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("LoadPins", exp, Logs.Error.ErrorsPriority.NotCritical);
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
                case StoredDataType.Pictures:
                    Logs.Output.ShowOutput("------------------------SAVE PICTURES BEGIN------------------");
                    dataFilePath = DataPicturesFile;
                    list = ProfilPicturesList;
                    break;
            }

            var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFile textFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(dataFilePath, CreationCollisionOption.ReplaceExisting);

            // write the JSON string!
            using (var sw = new StreamWriter(textFile.Path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                try
                {
                    serializer.Serialize(writer, list);
                }
                catch (JsonException exp)
                {
                    Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
            Logs.Output.ShowOutput("------------------------SAVE END------------------");

            return true;
        }

        //REFACTORING + universal profil picture

        public async static void LoadFavorites()
        {
            Logs.Output.ShowOutput("------------------------LOAD FAVORITES BEGIN------------------");

            var serializer = new JsonSerializer();

            try
            {
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
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("LoadFavorites", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            Logs.Output.ShowOutput("------------------------LOAD FAVORITES END------------------");
        }

        public async static void LoadProfilPictures()
        {
            Logs.Output.ShowOutput("------------------------LOAD PICTURES BEGIN------------------");

            var serializer = new JsonSerializer();

            try
            {
                if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + DataPicturesFile) == true)
                {
                    // Get the app data folder and create or replace the file we are storing the JSON in.            
                    var textFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DataPicturesFile);

                    // read the JSON string!
                    using (var sw = new StreamReader(textFile.Path))
                    using (JsonReader reader = new JsonTextReader(sw))
                    {
                        var list = serializer.Deserialize<List<PMPhotoModel>>(reader);

                        if (list != null)
                        {
                            Logs.Output.ShowOutput("Deserialized " + list.Count.ToString() + " pictures");
                            ProfilPicturesList = list;
                        }
                    }
                }
                else
                {
                    Logs.Output.ShowOutput("Storage file does not exist");
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("LoadProfilPictures", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            Logs.Output.ShowOutput("------------------------LOAD PICTURES END------------------");
        }
    }
}
