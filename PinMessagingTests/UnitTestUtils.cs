using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PinMessaging;
using PinMessaging.Utils;
using System.Diagnostics;

namespace PinMessagingTests
{
    [TestClass]
    public class UnitTestUtils
    {
        [TestMethod]
        public void TestEmailChecker()
        {
            Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test@keke.fr"));
            Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test@keke.eu.fr"));
            Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("t@k.fr"));
            Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("test.kevin@keke.fr"));
            Assert.IsTrue(PinMessaging.Utils.EmailChecker.IsEmailValid("0@keke.fr"));
        }

        [TestMethod]
        public void TestEncyptSHA1()
        {
            Assert.ReferenceEquals(PinMessaging.Utils.Encrypt.SHA1Core.ConvertToSHA1("coucou"), "5ed25af7b1ed23fb00122e13d7f74c4d8262acd8");
        }

        [TestMethod]
        public void TestEncyptMD5()
        {
            Assert.ReferenceEquals(PinMessaging.Utils.Encrypt.MD5Core.ConvertToMD5("coucou"), "721a9b52bfceacc503c056e3b9b93cfa");
        }
    }
}
