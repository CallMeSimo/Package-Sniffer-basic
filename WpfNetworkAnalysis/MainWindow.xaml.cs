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
using System.Threading;



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
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? MacAddress { get; set; }
            public string? Ip { get; set; }
            public float RoundTrip { get; set; }
            public string? Status { get; set; }
            public string? Description { get; set; }

        }

        private List<NetworkObject_> networkObjects = new List<NetworkObject_>();

        /*        vvv          LIST UPDATE       vvv          */
        public void updateListResultLoopback()
        {
            //Start fresh
            networkObjects.Clear();
            dataGridNetworkObject.ItemsSource = null;

            //Arrange object
            PingReply netObject = NetworkObjectExtensions.GetNetworkInfoLoopback();
            networkObjects.Add(new NetworkObject_()
            {
                Status = netObject.Status.ToString(),
                RoundTrip = netObject.RoundtripTime,
                Ip = netObject.Address.ToString()
            });
            dataGridNetworkObject.ItemsSource = networkObjects;

        }

        public void UpdateListResultConnectedDevices()
        {
            //CLEAR
            networkObjects.Clear();
            dataGridNetworkObject.ItemsSource = null;

            //ARRANGE
            CaptureDeviceList devices = ConnectedDevices();

            //ACT
            foreach (var device in devices)
            {
                networkObjects.Add(new NetworkObject_()
                {
                    Name = device.Name.ToString(),
                    MacAddress = device.MacAddress.ToString(),
                    Description = device.Description.ToString()
                }
                );
            }
            dataGridNetworkObject.ItemsSource = networkObjects;
        }

        public void UpdateListResultSearch()
        {
            //CLEAR
            networkObjects.Clear();
            dataGridNetworkObject.ItemsSource = null;

            //ARRANGE
            NetworkObject_ networkObjectSearch = NetworkObjectExtensions.GetNetworkInfoSearch(InputTextBox.Text);

            //ACT
            networkObjects.Add(networkObjectSearch);
            dataGridNetworkObject.ItemsSource = networkObjects;

        }
        /*                  LIST UPDATE                 */

        public static class NetworkObjectExtensions
        {
            public static PingReply GetNetworkInfoLoopback()//
            {
                //Creates ping instance.
                Ping selfPing = new Ping();

                //Send ping to self
                PingReply reply = selfPing.Send("127.0.0.1", 1000);

                return reply;
            } 

            public static NetworkObject_ GetNetworkInfoSearch(string search)
            {
                //Creates instance.
                Ping myPing = new Ping();
                NetworkObject_ networkObject = new NetworkObject_();

                //Send ping
                try
                {
                    PingReply reply = myPing.Send(search, 1000);

                    if (reply.Status == IPStatus.Success)
                    {
                        //Save result.
                        networkObject.RoundTrip = reply.RoundtripTime;
                        networkObject.Ip = reply.Address.ToString();
                        networkObject.Address = search;
                        networkObject.Status = reply.Status.ToString();
                        networkObject.Description = reply.GetType().ToString();

                        return networkObject;
                    }
                }
                catch (PingException)
                {
                    System.Diagnostics.Debug.WriteLine("passed");
                    networkObject = new NetworkObject_ { Address = search, Status = "Missing" };
                    return networkObject;
                }
                catch (ArgumentException)
                {
                    networkObject = new NetworkObject_ { Address = search, Status = "Missing" };
                    return networkObject;
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

            void Device_OnPacketArrival(object s, PacketCapture e)
            {
                System.Diagnostics.Debug.WriteLine("THIS: " + e.GetPacket());
            }

            using var device = CaptureDeviceList.Instance[0];
            device.Open();
            device.OnPacketArrival += Device_OnPacketArrival;
            device.StartCapture();
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
            updateListResultLoopback();
        }

        private void ConnectedDevicedButton(object sender, RoutedEventArgs e)
        {
            UpdateListResultConnectedDevices();
        }

        private void SearchPing(object sender, RoutedEventArgs e)
        {
            //Thread UpdateListResultSearchThread = new Thread(new ThreadStart(UpdateListResultSearch));
            //UpdateListResultSearchThread.Start();
            UpdateListResultSearch();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Sniffer(object sender, RoutedEventArgs e)
        {
            NetworkSniffer();
        }
    }
}
