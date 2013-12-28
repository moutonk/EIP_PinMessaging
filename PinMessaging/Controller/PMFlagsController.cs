using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using PinMessaging.Model;
using PinMessaging.Utils;

namespace PinMessaging.Controller
{
    public static class PMFlagsController
    {
        private static Dictionary<PMFlagModel.FlagsType, BitmapImage> FlagsMap { get; set; }

        public static void Initialization()
        {
            FlagsMap = new Dictionary<PMFlagModel.FlagsType, BitmapImage>();

            FlagsMap[PMFlagModel.FlagsType.AR] = Design.CreateImage(new Uri(Paths.FlagAR.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.CN] = Design.CreateImage(new Uri(Paths.FlagCN.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.DE] = Design.CreateImage(new Uri(Paths.FlagDE.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.EN] = Design.CreateImage(new Uri(Paths.FlagEN.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.ES] = Design.CreateImage(new Uri(Paths.FlagES.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.FI] = Design.CreateImage(new Uri(Paths.FlagFI.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.FR] = Design.CreateImage(new Uri(Paths.FlagFR.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.IN] = Design.CreateImage(new Uri(Paths.FlagIN.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.IT] = Design.CreateImage(new Uri(Paths.FlagIT.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.JP] = Design.CreateImage(new Uri(Paths.FlagJP.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.KR] = Design.CreateImage(new Uri(Paths.FlagKR.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.NO] = Design.CreateImage(new Uri(Paths.FlagNO.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.OT] = Design.CreateImage(new Uri(Paths.FlagOT.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.PT] = Design.CreateImage(new Uri(Paths.FlagPT.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.RU] = Design.CreateImage(new Uri(Paths.FlagRU.ToString(), UriKind.Relative));
            FlagsMap[PMFlagModel.FlagsType.SE] = Design.CreateImage(new Uri(Paths.FlagSE.ToString(), UriKind.Relative));
        }
    }
}
