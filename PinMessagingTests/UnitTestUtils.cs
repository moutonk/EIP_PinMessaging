using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PinMessaging;
using PinMessaging.Controller;
using PinMessaging.Utils;
using System.Diagnostics;
using System.Collections.Generic;
using PinMessaging.Utils.WebService;
using PinMessaging.Model;
using System.Threading;
using System.Windows;
using PinMessaging.Other;

namespace PinMessagingTests
{
    [TestClass]
    public class UnitTestUtils
    {
        [TestClass]
        public class UnitTestUtilsEmail
        {
            [TestMethod]
            public void TestEmailCheckerCorrect()
            {
                Assert.IsTrue(Utils.IsEmailValid("test@keke.fr"));
                Assert.IsTrue(Utils.IsEmailValid("test@keke.eu.fr"));
                Assert.IsTrue(Utils.IsEmailValid("t@k.fr"));
                Assert.IsTrue(Utils.IsEmailValid("test.kevin@keke.fr"));
                Assert.IsTrue(Utils.IsEmailValid("0@keke.fr"));
            }

            [TestMethod]
            public void TestEmailCheckerIncorrect()
            {
                Assert.IsFalse(Utils.IsEmailValid("@keke.fr"));
                Assert.IsFalse(Utils.IsEmailValid("test@.eu.fr"));
                Assert.IsFalse(Utils.IsEmailValid("t@.f"));
                Assert.IsFalse(Utils.IsEmailValid("test.kevin@keke..fr"));
                Assert.IsFalse(Utils.IsEmailValid("@."));
            }
        }

        [TestClass]
        public class UnitTestUtilsEncrypt
        {
            [TestMethod]
            public void TestEncyptSHA1()
            {
                Assert.AreEqual(PinMessaging.Utils.Encrypt.SHA1Core.ConvertToSHA1("coucou"), "5ed25af7b1ed23fb00122e13d7f74c4d8262acd8");
            }

            [TestMethod]
            public void TestEncyptMD5()
            {
               Assert.AreEqual(PinMessaging.Utils.Encrypt.MD5Core.ConvertToMD5("coucou"), "721a9b52bfceacc503c056e3b9b93cfa");
            }
        }

        [TestClass]
        public class UnitTestUtilsWebservice : PMWebServiceEndDetector
        {
           bool _webServiceReponse;

           [TestMethod]
           public void TestWebServiceAnswer()
           {
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "kevin.mouton@epitech.eu"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");
           }

           [TestMethod]
           public void TestWebServiceCheckEmailExist()
           {
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "k.k@k.kk"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsFalse(PMData.IsEmailDispo);
           }

           [TestMethod]
           public void TestWebServiceCheckEmailNotExist()
           {
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "dsdqsqdsqddsq@fqdfqdfqfq.dqsdhjsqd"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.IsEmailDispo);
           }

           [TestMethod]
           public void TestWebServiceSignInCorrect()
           {
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                    {"email", "k.k@k.kk"},
                    {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("kkkkkk"))}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignIn, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.IsSignInSuccess);
           }

            [TestMethod]
           public void TestWebServiceSignInIncorrect()
           {
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                    {"email", "a@a.aa"},
                    {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("wrongpassword!"))}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignIn, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsFalse(PMData.IsSignInSuccess);
           }

           [TestMethod]
           public void TestWebServiceSignUpCorrect()
           {
               _webServiceReponse = false;
               string randomEmail = CreateRandomEmail();

               var dictionary = new Dictionary<string, string>
               {
                    {"email", randomEmail},
                    {"login", randomEmail},
                    {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("gggggg"))},
                    {"simId", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("gggggg"))}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignUp, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.IsSignUpSuccess);
           }

           [TestMethod]
           public void TestWebServiceGetPinsCorrect()
           {
               // fail if 0 pin exists
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;
               var numberPinsBefore = PMData.PinsListToAdd.Count;

               var dictionary = new Dictionary<string, string>
               {
                    {"longitude", "-118.0001"},
                    {"latitude", "33.98"},
                    {"radius", "1"}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPins, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberPinsAfter = PMData.PinsListToAdd.Count;

               Assert.IsTrue(numberPinsBefore < numberPinsAfter);
           }

           [TestMethod]
           public void TestWebServiceGetPinsEmptyResult()
           {
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;
               var numberPinsBefore = PMData.PinsListToAdd.Count;

               var dictionary = new Dictionary<string, string>
               {
                {"longitude", "0"},
                {"latitude", "0"},
                {"radius", "0"}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPins, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberPinsAfter = PMData.PinsListToAdd.Count;

               Assert.IsTrue(numberPinsBefore == numberPinsAfter);
           }

           [TestMethod]
           public void TestWebServiceCreatePin()
           {
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;
               var numberPinsBefore = PMData.PinsListToAdd.Count;

               var dictionary = new Dictionary<string, string>
                {
                    {"longitude", "25.0001"},
                    {"latitude", "28.000001"},
                    {"title", "LOL"},
                    {"contentType", "0"},
                    {"content", "YOLO"},
                    {"pinType", "0"},
                    {"private", "false"},
                    {"authorisedUsersId", null}
                };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePin, SyncType.Async, dictionary, null);
                
               StartTimer();
    
               Thread.Sleep(3000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberPinsAfter = PMData.PinsListToAdd.Count;

               Assert.IsTrue(numberPinsBefore < numberPinsAfter);
           }

           [TestMethod]
           public void TestWebServiceChangeEmail()
           {
            //   webServiceReponse = false;

            //   var dictionary = new Dictionary<string, string>
            //    {
            //    {"newEmail", "newemail@email.com"}
            //};

            //   PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangeEmail, SyncType.Async, dictionary, null);

            //   StartTimer();

            //   Thread.Sleep(1000);

            //   Assert.IsTrue(PMData.IsChangeEmailSuccess);
               Assert.IsFalse(false);
           }

           [TestMethod]
           public void TestWebServiceChangePasswordCorrect()
           {
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
            {
                {"oldPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("kkkkkk"))},
                {"newPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("kkkkkk"))}
            };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangePassword, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.IsChangePwdSuccess);
           }

           [TestMethod]
           public void TestWebServiceChangePasswordIncorrect()
           {
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
            {
                {"oldPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("kkkkk"))},
                {"newPassword", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("kkkkkk"))}
            };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangePassword, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsFalse(PMData.IsChangePwdSuccess);
           }

           [TestMethod]
           public void TestWebServiceRemoveFavoriteIncorrect()
           {
               TestWebServiceSignInCorrect();

               _webServiceReponse = false;
               PMData.WasFavoriteRemovedSuccess = false;
               
               var dictionary = new Dictionary<string, string>
               {
                   {"favoriteId", "-1"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.RemoveFavoriteUser, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.WasFavoriteRemovedSuccess == false);
           }

           [TestMethod]
           public void TestWebServiceAddFavoriteIncorrect()
           {
               TestWebServiceSignInCorrect();

               PMData.WasFavoriteAddedSuccess = false;
               _webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"favoriteId", "-1"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.AddFavoriteUser, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               Assert.IsTrue(PMData.WasFavoriteAddedSuccess == false);
           }

           [TestMethod]
           public void TestWebServiceCreatePinMsgCorrect()
           {
               TestWebServiceCreatePin();

               _webServiceReponse = false;
               var numberComBefore = PMData.PinsCommentsListTmp.Count;

               var dictionary = new Dictionary<string, string>
               {
                    {"pinId", PMData.PinsListToAdd[PMData.PinsListToAdd.Count - 1].Id},
                    {"type", "0"},
                    {"content", "test msg"}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePinMessage, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberComAfter = PMData.PinsCommentsListTmp.Count;

               Assert.IsTrue(numberComBefore < numberComAfter);
           }

           [TestMethod]
           public void TestWebServiceCreatePinMsgIncorrect()
           {
               _webServiceReponse = false;
               var numberComBefore = PMData.PinsCommentsListTmp.Count;

               var dictionary = new Dictionary<string, string>
               {
                    {"pinId", "-1"},
                    {"type", "0"},
                    {"content", "test msg"}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePinMessage, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberComAfter = PMData.PinsCommentsListTmp.Count;

               Assert.IsFalse(numberComBefore < numberComAfter);
           }

           [TestMethod]
           public void TestWebServiceGetPinMessagesCorrect()
           {
               TestWebServiceCreatePinMsgCorrect();

               _webServiceReponse = false;
               var numberMsgBefore = PMData.PinsCommentsListTmp.Count;

               var dictionary = new Dictionary<string, string>
               {
                    {"pinId", PMData.PinsListToAdd[PMData.PinsListToAdd.Count - 1].Id},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPinMessages, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberMsgAfter = PMData.PinsCommentsListTmp.Count;

               Assert.IsTrue(numberMsgBefore < numberMsgAfter);
           }

           [TestMethod]
           public void TestWebServiceGetPinMessagesIncorrect()
           {
               TestWebServiceCreatePinMsgCorrect();

               _webServiceReponse = false;
               var numberMsgBefore = PMData.PinsCommentsListTmp.Count;

               var dictionary = new Dictionary<string, string>
               {
                    {"pinId", "-1"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPinMessages, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(1000);

               Assert.IsTrue(_webServiceReponse, "No webservice response");
               Assert.IsFalse(PMData.NetworkProblem, "No webservice response");

               var numberMsgAfter = PMData.PinsCommentsListTmp.Count;

               Assert.IsTrue(numberMsgBefore == numberMsgAfter);
           }

           private string CreateRandomEmail()
           {
               Random random = new Random();
               string email = string.Empty;

               for (int num = 0; num < 5; num++)
               {
                   email += random.Next(0, 9).ToString();
                   Thread.Sleep(10);
               }
               email += "@test.test";
                
               return email;
           }

           protected override void waitEnd_Tick(object sender, EventArgs e)
           {
               if (PMWebService.OnGoingRequest == false)
               {
                   StopTimer();
                   _webServiceReponse = true;
               }
           }
        }
    }
}
