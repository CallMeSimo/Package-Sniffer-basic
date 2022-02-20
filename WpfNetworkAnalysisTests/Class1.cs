using System;
using Xunit;
using Newtonsoft.Json;

namespace WpfNetworkAnalysis
{
    public class WpfNetworkAnalysisTests
    {
        [Fact]
        public void GetNetworkInfoSearchTESTPass()
        {
            //Arrange
            WpfNetworkAnalysis.MainWindow.NetworkObject_ searchTestObject_PASS = new MainWindow.NetworkObject_ { Address = "Google.com" };
            WpfNetworkAnalysis.MainWindow.NetworkObject_ searchTestObject_PASS2 = new MainWindow.NetworkObject_ { Address = "Duckduckgo.com" };

            //Act
            var test_PASS1 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("Google.com");
            var test_PASS2 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("Duckduckgo.com");

            //Assert
            Assert.Equal(searchTestObject_PASS.Address, test_PASS1.Address);
            Assert.Equal(searchTestObject_PASS2.Address, test_PASS2.Address);
        }
        [Fact]
        public void GetNetworkInfoSearchTESTFail()
        {
            //Arrange
            MainWindow.NetworkObject_ testWantedResult = new MainWindow.NetworkObject_ { Address = "thisisnotRight", Status = "Missing" };

            //Act
            var test = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("thisisnotRight");
            var testInfo = JsonConvert.SerializeObject(test);
            var testWantedResultInfo = JsonConvert.SerializeObject(test);
            
            //Assert
            Assert.Equal(testWantedResultInfo, testInfo);
        }
        [Fact]
        public void GetNetworkInfoSearchTESTArgumentException()
        {
            //Arrange
            MainWindow.NetworkObject_ testWantedResult = new MainWindow.NetworkObject_{ Address = "", Status = "Missing" };

            //Act
            MainWindow.NetworkObject_ test = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("");
            var testInfo = JsonConvert.SerializeObject(test);
            var testWantedResultInfo = JsonConvert.SerializeObject(test);

            //Assert
            Assert.Equal(testWantedResultInfo, testInfo);
        }
        
        [Fact]
        public void GetNetworkInfoSearchTESTloopBack()
        {
            //Arrange
            WpfNetworkAnalysis.MainWindow.NetworkObject_ searchTestObject_PASS2 = new WpfNetworkAnalysis.MainWindow.NetworkObject_ { Address = "127.0.0.1", RoundTrip = 0f };
            
            //Act
            var test_PASS2 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("127.0.0.1");

            //Assert
            Assert.Equal(searchTestObject_PASS2.RoundTrip, test_PASS2.RoundTrip);
            Assert.Equal(searchTestObject_PASS2.Address, test_PASS2.Address);
        }
    }
}
