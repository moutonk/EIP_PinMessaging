﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PinMessaging.Utils
{
    class Design
    {
        public static byte[] FromHexaToARGB(string hexaColor)
        {
            var argb = new byte[4];

            try
            {
                argb[0] = Convert.ToByte(hexaColor.Substring(1, 2), 16);
                argb[1] = Convert.ToByte(hexaColor.Substring(3, 2), 16);
                argb[2] = Convert.ToByte(hexaColor.Substring(5, 2), 16);
                argb[3] = Convert.ToByte(hexaColor.Substring(7, 2), 16);
            }
            catch (Exception e)
            {
                ErrorsManager.ShowError(e, ErrorsPriority.NotCritical);

                argb[0] = 0;
                argb[1] = 0;
                argb[2] = 0;
                argb[3] = 0;
            }
            return argb;
        }

        public static WriteableBitmap ChangeImageColor(Uri imgSrc, SolidColorBrush newColor)
        {
            var bti = new BitmapImage(imgSrc) {CreateOptions = BitmapCreateOptions.None};

            //new image size, otherwise if image too big takes lots of time
            //bti.DecodePixelWidth = 100;
            //bti.DecodePixelHeight = 100;

            //mandatory otherwise exception

            var wbmp = new WriteableBitmap(bti);
            byte[] pixelBytes;
            var argb = FromHexaToARGB(newColor.Color.ToString());
       
            for (var i = 0; i < wbmp.Pixels.Length; i++)
            {
                //get the byte array associated to the pixel
                pixelBytes = BitConverter.GetBytes(wbmp.Pixels[i]);

                pixelBytes[0] = argb[1];
                pixelBytes[1] = argb[2];
                pixelBytes[2] = argb[3];

                //little endian to big endien
                Array.Reverse(pixelBytes, 0, 3);

                //convert byte array into int
                wbmp.Pixels[i] = BitConverter.ToInt32(pixelBytes, 0);
            }

            //validate the change
            wbmp.Invalidate();

            return wbmp;
        }
    }
}
