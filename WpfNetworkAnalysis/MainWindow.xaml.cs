using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpPcap;

namespace WpfNetworkAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class NetworkObject_
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Ip { get; set; }
            public float roundTrip { get; set; }
            public float status { get; set; }

            //public NetworkObject_(string hostName, string hostAddress, string hostIp)
            //{
            //    this.hostName = hostName;
            //    this.hostAddress = hostAddress;
            //    this.hostIp = hostIp;
            //}

        }

        /*                  LIST UPDATE                 */
        public void updateListResult()
        {
            //Start fresh
            lst.Items.Clear();

            //Arrange object
            NetworkObject_ netObject = NetworkObjectExtensions.GetNetworkInfoSearch("Google.com");
            lst.Items.Add("Hit: " + netObject.Address);

        }

        public void UpdateListResultConnectedDevices()
        {
            lst.Items.Clear();

            CaptureDeviceList devices = ConnectedDevices();
            foreach (var device in devices)
            {
                lst.Items.Add("Hit: " + device.ToString());
            }
        }

        public static class NetworkObjectExtensions
        {
            public static void GetNetworkInfoLoopback(ListBox boxList)
            {
                boxList.Items.Clear();

                //Creates ping instance.
                Ping selfPing = new Ping();

                //Send ping to self
                PingReply reply = selfPing.Send("127.0.0.1", 1000);

                boxList.Items.Add("Hit: " + "127.0.0.1" + " : " + reply.RoundtripTime + "ms" + " : " + reply.Status);


            } 

            public static NetworkObject_ GetNetworkInfoSearch(string search)
            {

               // boxList.Items.Clear();

                //Creates ping instance.
                Ping myPing = new Ping();

                //Send ping
                try
                {
                    PingReply reply = myPing.Send(search, 1000);

                    if (reply != null)
                    {
                        //Save result.
                        NetworkObject_ networkObject = new NetworkObject_();
                        networkObject.roundTrip = reply.RoundtripTime;
                        networkObject.Name = reply.Address.ToString();
                        networkObject.Address = search;

                        //boxList.Items.Add("Hit: " + networkObject.Address + " : " + networkObject.roundTrip + "ms" + " : " + reply.Status);
                        return networkObject;
                    }
                }
                catch
                {
                    //boxList.Items.Add("Hit: " + search + " : " + "Failed");
                    return null;
                }
                return null;
            }
        }

        public CaptureDeviceList ConnectedDevices()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return devices;
            }
            return devices;
        }

        public void NetworkSniffer()
        {

        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            //GetNetworkInfo();
            //updateListResult();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Loopback(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("pass");
            updateListResult();
        }

        private void ConnectedDevicedButton(object sender, RoutedEventArgs e)
        {
            UpdateListResultConnectedDevices();
        }
    }
}
