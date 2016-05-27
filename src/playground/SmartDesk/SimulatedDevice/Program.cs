using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice {
  class Program {
    private static DeviceClient _deviceClient;
    private static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
    private static readonly string DeviceKey = ConfigurationManager.AppSettings["deviceKey"];
    private static readonly string HeartBeatKey = ConfigurationManager.AppSettings["heartbeatKey"];

    static void Main(string[] args) {
      Console.WriteLine("Simulated device");
      _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));

      Task.Run(async () => {
        DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device2", HeartBeatKey));
        while (true) {
          await SendTelemetryMessage(0, false, 2);
          await Task.Delay(TimeSpan.FromMinutes(1));
        }
      });
      

      SendDeviceToCloudMessagesAsync();
      Console.ReadLine();
    }

    private static async void SendDeviceToCloudMessagesAsync() {
      var standing = 100;
      var sitting = 50;

      for (var j = 0; j < 2; j++) {
        for (int i = 0; i < 3; i++) {
          await SendTelemetryMessage(standing, true);
          await Task.Delay(TimeSpan.FromMinutes(1));
        }

        await Task.Delay(TimeSpan.FromMinutes(3));

        for (int i = 0; i < 3; i++) {
          await SendTelemetryMessage(sitting, true);
          await Task.Delay(TimeSpan.FromMinutes(1));
        }

        for (int i = 0; i < 3; i++) {
          await SendTelemetryMessage(sitting, false);
          await Task.Delay(TimeSpan.FromMinutes(1));
        }

      }
      Console.WriteLine("Finished.");
    }
    //private static async void SendDeviceToCloudMessagesAsync() {
    //  var standing = 100;
    //  var sitting = 50;
    //  // 2 minutes active standing
    //  for (int i = 0; i < 3; i++) {
    //    await SendTelemetryMessage(standing, true);
    //    await Task.Delay(TimeSpan.FromMinutes(1));
    //  }

    //  // 3 minutes of active sitting
    //  for (int i = 0; i < 3; i++) {
    //    await SendTelemetryMessage(sitting, true);
    //    await Task.Delay(TimeSpan.FromMinutes(1));
    //  }

    //  //// 5 minutes no sensor values
    //  //await Task.Delay(TimeSpan.FromMinutes(5));

    //  // 2 minutes of active sitting
    //  for (int i = 0; i < 3; i++) {
    //    await SendTelemetryMessage(sitting, true);
    //    await Task.Delay(TimeSpan.FromMinutes(1));
    //  }

    //  // 3 minutes inactive sitting
    //  for (int i = 0; i < 3; i++) {
    //    await SendTelemetryMessage(sitting, false);
    //    await Task.Delay(TimeSpan.FromMinutes(1));
    //  }

    //  Console.WriteLine("Finished.");
    //}

    private static async Task SendTelemetryMessage(double height, bool isActive, int id = 1) {
      var telemetryDataPoint = new {
        DeviceId = id,
        Timestamp = DateTime.Now.ToString("o"),
        Height = height,
        IsActive = isActive
      };
      var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
      var message = new Message(Encoding.ASCII.GetBytes(messageString));
      await _deviceClient.SendEventAsync(message);
      Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
    }
  }
}
