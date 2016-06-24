using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
      _deviceClient = CreateDeviceClient();

      //Task.Run(async () => {
      //  DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device2", HeartBeatKey));
      //  while (true) {
      //    await SendTelemetryMessage(0, false, 2);
      //    await Task.Delay(TimeSpan.FromMinutes(1));
      //  }
      //});


      //Foo().GetAwaiter().GetResult();

      //SendDeviceToCloudMessagesAsync();
      Foo().GetAwaiter().GetResult();
      Console.WriteLine("done");


      Console.ReadLine();
    }

    private static DeviceClient CreateDeviceClient() {
      return DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("device1", DeviceKey));
    }

    private static async Task Foo() {
      var standing = 120;
      var sitting = 75;

      TimeSpan now;

      do {
        now = DateTime.Now.TimeOfDay;

        if (now.Between(7, 8)) {
          // first sitting session
          await SendTelemetryMessage(sitting, true);
        }
        else if (now.Between(8, 8.5)) {
          // jausn -> inactiv
          await SendTelemetryMessage(sitting, false);
        }
        else if (now.Between(8.5, 10)) {
          // standing
          await SendTelemetryMessage(standing, true);
        } else if (now.Between(10, 11)) {
          // standing - inactive because of meeting
          await SendTelemetryMessage(standing, false);
        } else if (now.Between(11, 11.25)) {
          // standing
          await SendTelemetryMessage(standing, true);
        } else if (now.Between(11.25, 11.75)) {
          // inactive - lunch
          await SendTelemetryMessage(standing, false);
        } else if (now.Between(11.75, 12.5)) {
          // sitting
          await SendTelemetryMessage(sitting, true);
        } else if (now.Between(12.5, 14)) {
          // standing
          await SendTelemetryMessage(standing, true);
        } else if (now.Between(14, 14.25)) {
          // inactive .. coffee break
          await SendTelemetryMessage(standing, false);
        } else if (now > 14.25.Hours()) {
        // sitting the rest of the day
          await SendTelemetryMessage(sitting, true);
        }
        else {
          Console.WriteLine("Nothing to do.");
        }

        await Task.Delay(TimeSpan.FromMinutes(1));
      } while (now < 16.Hours());
    }

    private static async void SendDeviceToCloudMessagesAsync() {
      var standing = 120;
      var sitting = 60;

      // 3 min standing
      for (var i = 0; i < 4; i++) {
        await SendTelemetryMessage(130, false);
        await Task.Delay(TimeSpan.FromMinutes(1));
      }
      for (var i = 0; i < 4; i++) {
        await SendTelemetryMessage(130, true);
        await Task.Delay(TimeSpan.FromMinutes(1));
      }

      //// 4 minute sitting
      //for (var i = 0; i < 4; i++) {
      //  await SendTelemetryMessage(sitting, true);
      //  await Task.Delay(TimeSpan.FromMinutes(1));
      //}

      //for (var j = 0; j < 2; j++) {
      //  for (int i = 0; i < 3; i++) {
      //    await SendTelemetryMessage(standing, true);
      //    await Task.Delay(TimeSpan.FromMinutes(1));
      //  }

      //  await Task.Delay(TimeSpan.FromMinutes(3));

      //  for (int i = 0; i < 3; i++) {
      //    await SendTelemetryMessage(sitting, true);
      //    await Task.Delay(TimeSpan.FromMinutes(1));
      //  }

      //  for (int i = 0; i < 3; i++) {
      //    await SendTelemetryMessage(sitting, false);
      //    await Task.Delay(TimeSpan.FromMinutes(1));
      //  }

      //}
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
  }

  public static class Ext {
    public static TimeSpan Hours(this double d) {
      return TimeSpan.FromHours(d);
    }
    public static TimeSpan Hours(this int d) {
      return TimeSpan.FromHours(d);
    }
    public static bool Between(this TimeSpan t, double start, double end) {
      return t > start.Hours() && t < end.Hours();
    }
  }
}
