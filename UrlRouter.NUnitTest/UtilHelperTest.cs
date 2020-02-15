using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace UrlRouter.NUnitTest
{
    public class UtilHelperTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoDesktopWindows()
        {
            string userAgentInput = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246";
            string resultExpected = "Windows [10]";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoDesktopLinux()
        {
            string userAgentInput = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1";
            string resultExpected = "Ubuntu";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoDesktopMac()
        {
            string userAgentInput = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit/601.3.9 (KHTML, like Gecko) Version/9.0.2 Safari/601.3.9";
            string resultExpected = "Mac [10_11_2]";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoSmartphoneWindows()
        {
            string userAgentInput = "Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 4.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 630) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537";
            string resultExpected = "Windows Phone [8.1]";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoSmartphoneAndroid()
        {
            string userAgentInput = "Mozilla/5.0 (Linux; Android 6.0.1; SAMSUNG SM-G925F Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/4.0 Chrome/44.0.2403.133 Mobile Safari/537.36";
            string resultExpected = "Android [6.0.1]";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_ValidoSmartphoneIos()
        {
            string userAgentInput = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_3 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) CriOS/65.0.3325.152 Mobile/15E216 Safari/604.1";
            string resultExpected = "iOS [11_3]";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }
        [Test]
        public void TestObterSistemaOperacional_InputNull()
        {
            string userAgentInput = null;
            string resultExpected = "Não detectado";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_InputEmpty()
        {
            string userAgentInput = string.Empty;
            string resultExpected = "Não detectado";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterSistemaOperacional_Invalido()
        {
            string userAgentInput = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit/601.3.9 (KHTML, like Gecko) Version/9.0.2 Safari/601.3.9";
            string resultExpected = "Windows";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterSistemaOperacional(userAgentInput);
            Assert.AreNotEqual(resultExpected, resultMethod);
        }
        
        [Test]
        public void TestObterIpMaquinaCliente_RequestNull()
        {

            HttpRequest requestInput = null;
            string resultExpected = string.Empty;
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterIpMaquinaCliente(requestInput);
            Assert.AreEqual(resultExpected, resultMethod);

        }

        [Test]
        public void TestObterIpMaquinaCliente_RequestValido()
        {

            Mock<HttpContext> moqContext = new Mock<HttpContext>();
            Mock<HttpRequest> moqRequest = new Mock<HttpRequest>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.Request.Headers["HTTP_X_FORWARDED_FOR"]).Returns("0.0.0.0");

            HttpRequest requestInput = moqRequest.Object;
            string resultExpected = "0.0.0.0";
            string resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterIpMaquinaCliente(requestInput);
            Assert.AreEqual(resultExpected, resultMethod);

        }

        [Test]
        public void TestHasDeviceMobiles_InputNull()
        {
            string userAgentInput = null;
            bool resultExpected = false;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestHasDeviceMobile_InputEmpty()
        {
            string userAgentInput = string.Empty;
            bool resultExpected = false;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestHasDeviceMobile_Invalido()
        {
            string userAgentInput = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1";
            bool resultExpected = false;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestHasDeviceMobile_ValidoPhone()
        {
            string userAgentInput = "SonyEricssonW840i/R1BD001/SEMC-Browser/4.2 Profile/MIDP-2.0 Configuration/CLDC-1.1";
            bool resultExpected = true;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestHasDeviceMobile_ValidoSmartphone()
        {
            string userAgentInput = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1";
            bool resultExpected = true;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestHasDeviceMobile_ValidoTablet()
        {
            string userAgentInput = "Mozilla/5.0 (Linux; Android 6.0.1; SGP771 Build/32.2.A.0.253; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/52.0.2743.98 Safari/537.36";
            bool resultExpected = true;
            bool resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.HasDeviceMobile(userAgentInput);
            Assert.AreEqual(resultExpected, resultMethod);
        }

        [Test]
        public void TestObterDeviceMobile_InputNull()
        {
            string userAgentInput = null;
            UrlRouter.AspNetMvc.Helper.DeviceMobile resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.Null(resultMethod);
        }

        [Test]
        public void TestObterDeviceMobile_InputEmpty()
        {
            string userAgentInput = string.Empty;
            UrlRouter.AspNetMvc.Helper.DeviceMobile resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.Null(resultMethod);
        }

        [Test]
        public void TestObterDeviceMobile_ValidoIOSiPhone()
        {
            string userAgentInput = "Mozilla/5.0 (Apple-iPhone7C2/1202.466; U; CPU like Mac OS X; en) AppleWebKit/420+ (KHTML, like Gecko) Version/3.0 Mobile/1A543 Safari/419.3";
            string resultModeloExpected = "smartphone";
            var resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.AreEqual(resultModeloExpected, resultMethod.Device);
        }

        [Test]
        public void TestObterDeviceMobile_ValidoAndroidGalaxySamsung()
        {
            string userAgentInput = "Mozilla/5.0 (Linux; Android 6.0.1; SM-G935S Build/MMB29K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36";
            string resultModeloExpected = "smartphone";
            var resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.AreEqual(resultModeloExpected, resultMethod.Device);
        }

        [Test]
        public void TestObterDeviceMobile_ValidoWindowsPhone()
        {
            //string userAgentInput = "Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Microsoft; Lumia 950) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Mobile Safari/537.36 Edge/13.1058";
            string userAgentInput = "Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 4.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 630) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537";
            string resultModeloExpected = "smartphone";
            var resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.AreEqual(resultModeloExpected, resultMethod.Device);
        }

        [Test]
        public void TestObterDeviceMobile_ValidoAndroidTablet()
        {
            string userAgentInput = "Mozilla/5.0 (Linux; Android 6.0.1; SHIELD Tablet K1 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Safari/537.36";
            string resultModeloExpected = "tablet";
            var resultMethod = UrlRouter.AspNetMvc.Helper.UtilHelper.ObterDeviceMobile(userAgentInput);
            Assert.AreEqual(resultModeloExpected, resultMethod.Device);
        }

    }
}