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
      AddDeviceAsync(registryManager, "device1").Wait();
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
  }
}
