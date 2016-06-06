using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace OfflineTickJob {
  class Program {
    private static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
    private static readonly string DeviceKey = ConfigurationManager.AppSettings["deviceKey"];
    private static readonly string HeartBeatKey = ConfigurationManager.AppSettings["heartbeatKey"];

    static void Main(string[] args) {
      Work().GetAwaiter().GetResult();
    }

    static async Task Work() {
      var client = CreateDeviceClient();
      while (true) {
        try {
          await SendTelemetryMessage(client, 0, false, 2);
          await Task.Delay(TimeSpan.FromMinutes(1));
        }
        catch (Exception) {
          await Task.Delay(TimeSpan.FromSeconds(10));
          await client.CloseAsync();
          client = CreateDeviceClient();
        }
      }
    }

    private static DeviceClient CreateDeviceClient() {
      return DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));
    }

    private static async Task SendTelemetryMessage(DeviceClient client, double height, bool isActive, int id = 1) {
      var telemetryDataPoint = new {
        DeviceId = id,
        Timestamp = DateTime.Now.ToString("O", CultureInfo.InvariantCulture),
        Height = height,
        IsActive = isActive
      };

      var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
      var message = new Message(Encoding.ASCII.GetBytes(messageString));
      await client.SendEventAsync(message);
    }
  }
}
