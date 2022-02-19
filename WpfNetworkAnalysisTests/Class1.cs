using System;
using Xunit;

namespace WpfNetworkAnalysis
{
    public class WpfNetworkAnalysisTests
    {
        [Fact]
        public void GetNetworkInfoSearchTESTPass()
        {
            //Arrange
            WpfNetworkAnalysis.MainWindow.NetworkObject_ searchTestObject_PASS = new MainWindow.NetworkObject_ { Address = "Google.com" };

            //Act
            var test_PASS1 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("Google.com");

            //Assert
            Assert.Equal(searchTestObject_PASS.Address, test_PASS1.Address);
        }
        [Fact]
        public void GetNetworkInfoSearchTESTFail()
        {
            //Arrange + Act
            var test_FAIL1 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("thisisnotRight");

            //Assert
            Assert.Null(test_FAIL1);
        }

        [Fact]
        public void GetNetworkInfoSearchTESTloopBack()
        {
            //Arrange
            WpfNetworkAnalysis.MainWindow.NetworkObject_ searchTestObject_PASS2 = new WpfNetworkAnalysis.MainWindow.NetworkObject_ { Address = "127.0.0.1", roundTrip = 0f };
            //Act
            var test_PASS2 = WpfNetworkAnalysis.MainWindow.NetworkObjectExtensions.GetNetworkInfoSearch("127.0.0.1");

            //Assert
            Assert.Equal(searchTestObject_PASS2.roundTrip, test_PASS2.roundTrip);
            Assert.Equal(searchTestObject_PASS2.Address, test_PASS2.Address);
        }
    }
}
