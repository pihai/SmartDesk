using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity {
  class Program {
    static void Main(string[] args) {
      var connectionString = ConfigurationManager.AppSettings["iotCnnStr"];
      var registryManager = RegistryManager.CreateFromConnectionString(connectionString);

      Console.WriteLine("Enter a command: (new | list)");

      switch (Console.ReadLine()) {
        case "new":
          Console.WriteLine("Enter the device id");
          AddDeviceAsync(registryManager, Console.ReadLine()).GetAwaiter().GetResult();
          break;
        case "list":
          ListAllDevices(registryManager).GetAwaiter().GetResult();
          break;
        default:
          Console.WriteLine("Unknown command");
          break;
      }

      Console.ReadLine(); 
    }

    private static async Task AddDeviceAsync(RegistryManager registryManager, string deviceId) {
      Device device;
      try {
        device = await registryManager.AddDeviceAsync(new Device(deviceId));
      }
      catch (DeviceAlreadyExistsException) {
        device = await registryManager.GetDeviceAsync(deviceId);
      }
      Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
    }

    private static async Task ListAllDevices(RegistryManager registryManager) {
      foreach (var device in await registryManager.GetDevicesAsync(1000)) {
        Console.WriteLine($"{device.Id} - last active {device.LastActivityTime}");
      }
    }
  }
}
