using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PinMessaging;
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
                Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test@keke.fr"));
                Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test@keke.eu.fr"));
                Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("t@k.fr"));
                Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test.kevin@keke.fr"));
                Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("0@keke.fr"));
            }

            [TestMethod]
            public void TestEmailCheckerIncorrect()
            {
                Assert.IsFalse(PinMessaging.Utils.EmailChecker.IsEmailValid("@keke.fr"));
                Assert.IsFalse(PinMessaging.Utils.EmailChecker.IsEmailValid("test@.eu.fr"));
                Assert.IsFalse(PinMessaging.Utils.EmailChecker.IsEmailValid("t@.f"));
                Assert.IsFalse(PinMessaging.Utils.EmailChecker.IsEmailValid("test.kevin@keke..fr"));
                Assert.IsFalse(PinMessaging.Utils.EmailChecker.IsEmailValid("@."));
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
           bool webServiceReponse;

           [TestMethod]
           public void TestWebServiceAnswer()
           {
               webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "kevin.mouton@epitech.eu"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);
               Assert.IsTrue(webServiceReponse);
           }

           [TestMethod]
           public void TestWebServiceCheckEmailExist()
           {
               webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "mk@epi.eu"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);
               Assert.IsFalse(PMData.IsEmailDispo);
           }

           [TestMethod]
           public void TestWebServiceCheckEmailNotExist()
           {
               webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                   {"email", "dsdqsqdsqddsq@fqdfqdfqfq.dqsdhjsqd"},
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.CheckEmail, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);
               Assert.IsTrue(PMData.IsEmailDispo);
           }

           [TestMethod]
           public void TestWebServiceSignInCorrect()
           {
               webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                    {"email", "a@a.aa"},
                    {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("aaaaaa"))}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignIn, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);
               Assert.IsTrue(PMData.IsSignInSuccess);
           }

           [TestMethod]
           public void TestWebServiceSignInIncorrect()
           {
               webServiceReponse = false;

               var dictionary = new Dictionary<string, string>
               {
                    {"email", "a@a.aa"},
                    {"password", Encrypt.MD5Core.ConvertToMD5(Encrypt.SHA1Core.ConvertToSHA1("wrongpassword!"))}
               };

               PMWebService.SendRequest(HttpRequestType.Post, RequestType.SignIn, SyncType.Async, dictionary, null);

               StartTimer();

               Thread.Sleep(2000);
               Assert.IsFalse(PMData.IsSignInSuccess);
           }

           [TestMethod]
           public void TestWebServiceSignUpCorrect()
           {
               webServiceReponse = false;
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
               Assert.IsTrue(PMData.IsSignUpSuccess);
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
               StopTimer();
               webServiceReponse = true;
           }
        }
    }
}
