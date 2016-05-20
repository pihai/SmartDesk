using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SmartDesk.Client
{
    class Program
    {
        private static DeviceClient _deviceClient;
        private static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
        private static readonly string DeviceKey = ConfigurationManager.AppSettings["deviceKey"];

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device");
            _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            var avgHeight = 50;
            var rand = new Random();

            while (true)
            {
                var currentHeight = avgHeight + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = "myFirstDevice",
                    timestamp = DateTime.Now.ToString("o"), 
                    currentHeight = currentHeight,
                    loggedOn = true
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
