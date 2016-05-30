using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SmartDesk.Client;
using SmartDesk.Client.Arduino;
using Microsoft.Win32;

namespace SmartDesk.Client
{
    class Program
    {
        private static DeviceClient _deviceClient;
        private static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
        private static readonly string DeviceKey = ConfigurationManager.AppSettings["deviceKey"];
        private static ISmartDeskClient client;
        private static bool IsActive = true;
        static void Main(string[] args)
        {
            client = new TestClient();
            string test = ConfigurationManager.AppSettings["deviceKey"];
            Console.WriteLine("Simulated device");
            SystemEvents.SessionSwitch += (sender, e) =>


               {
                   if (e.Reason == SessionSwitchReason.SessionLock)
                   {
                       //I left my desk
                       IsActive = false;
                       Console.WriteLine("Left desk");
                   }
                   else if (e.Reason == SessionSwitchReason.SessionUnlock)
                   {
                       //I returned to my desk
                       IsActive = true;
                       Console.WriteLine("Returned to desk");
                   }
               };


            _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            //var avgHeight = 50;
            //var rand = new Random();

            while (true)
            {
                //var currentHeight = avgHeight + rand.NextDouble() * 4 - 2;

                var height = client.GetHeight();
                var id = "1";
             
                var telemetryDataPoint = new
                {
                    DeviceId = id,
                    Timestamp = DateTime.Now.ToString("o"),
                    Height = height,
                    IsActive = IsActive
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await _deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Task.Delay(1000).Wait();
            }
        }
    }
}
