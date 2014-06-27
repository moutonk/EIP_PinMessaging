using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Storage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using PinMessaging.Other;
using PinMessaging.Utils;

namespace PinMessaging.View
{
    public partial class PMCurrentUserProfilView : PhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();

        public PMCurrentUserProfilView()
        {
            InitializeComponent();

            _photoChooserTask.Completed += photoChooserTask_Completed;
        }

        /*TO FINISH*/
        private void ChangeProfilPictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            _photoChooserTask.PixelWidth = 200;
            _photoChooserTask.PixelHeight = 200;
            _photoChooserTask.Show();
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class Tof
        {
            [JsonProperty]
            public int userId;

            [JsonProperty]
            public byte[] bfield;

            public Stream pictureStream;

            [OnSerialized]
            private void CreateStream(StreamingContext context)
            {
                pictureStream = new MemoryStream(bfield);
            }
        }

        private async void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var bmp = new BitmapImage();
            
                var t = new Tof();
                t.userId = 2;
                t.bfield = new byte[e.ChosenPhoto.Length];

                try
                {
                    Logs.Output.ShowOutput("Can write: " + e.ChosenPhoto.CanRead.ToString());
                    await e.ChosenPhoto.ReadAsync(t.bfield, 0, t.bfield.Length);
                }
                catch (Exception exp)
                {
                    Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
                }

                PMData.ProfilPicturesList.Add(t);

                await PMData.SaveData(PMData.StoredDataType.Pictures);
                PMData.ProfilPicturesList.Clear();
                PMData.LoadProfilPictures();
                
                //   Logs.Output.ShowOutput(e.ChosenPhoto.Length.ToString());
                
               // string fileName = Path.GetFileName(e.OriginalFileName);

                // persist data into isolated storage
                /*StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (Stream current = await file.OpenStreamForWriteAsync())
                {
                    await photoStream.CopyToAsync(current);
                }

                WriteableBitmap d;
                d.SaveJpeg;
                */

                bmp.SetSource(t.pictureStream);
                PMData.UserProfilPicture.Source = bmp;
                UserProfilImage.Source = PMData.UserProfilPicture.Source;
              //  UserProfilImage.
            }
        }
    }
}