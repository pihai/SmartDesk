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
using System.Globalization;

namespace SmartDesk.Client {
  class Program {
    private static DeviceClient _deviceClient;
    private static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
    private static readonly string DeviceKey = ConfigurationManager.AppSettings["deviceKey"];
    private static readonly string Port = "COM3";
    private static bool TestMode = false;

    private static bool IsActive = true;
    private static ISmartDeskClient client;
    static void Main(string[] args) {


      try {
        if (TestMode) {
          Console.WriteLine("=> Starting in TEST Mode!");
          client = new TestClient();
        }
        else {
          Console.WriteLine($"=> Starting Client on Port: {Port}");
          client = new ArduinoSerialClient(Port);
        }

        SystemEvents.SessionSwitch += (sender, e) => {
          if (e.Reason == SessionSwitchReason.SessionLock) {
            //I left my desk
            IsActive = false;
            Console.WriteLine("Left desk");
          }
          else if (e.Reason == SessionSwitchReason.SessionUnlock) {
            //I returned to my desk
            IsActive = true;
            Console.WriteLine("Returned to desk");
          }
        };

        _deviceClient = CreateDeviceClient();
        SendDeviceToCloudMessagesAsync();
        Console.ReadLine();
      }
      finally {
        client.Dispose();
      }
    }

    private static async void SendDeviceToCloudMessagesAsync() {
      while (true) { 
        var height = client.GetHeight();
        await SendTelemetryMessage(height, IsActive);

        Task.Delay(60000).Wait();
      }
    }
    private static async Task SendTelemetryMessage(double height, bool isActive, int id = 1) {
      var telemetryDataPoint = new {
        DeviceId = id,
        Timestamp = DateTime.Now.ToString("O", CultureInfo.InvariantCulture),
        Height = height,
        IsActive = isActive
      };

      var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
      var message = new Message(Encoding.ASCII.GetBytes(messageString));
      try {
        await _deviceClient.SendEventAsync(message);
      }
      catch (Exception e) {
        try {
          await _deviceClient.CloseAsync();
        }
        catch {
        }
        Console.WriteLine(e);
        await Task.Delay(20000);
        _deviceClient = CreateDeviceClient();
        Console.WriteLine("retry");
        await _deviceClient.SendEventAsync(message);
      }
      Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
    }
    private static DeviceClient CreateDeviceClient() {
      return DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));
    }
  }
}
