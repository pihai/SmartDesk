using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SmartDesk.WebApp.Queries;

namespace SmartDesk.WebApp.Services {
  public class SettingsService : ISettingsService {
    private static readonly ConcurrentDictionary<string, Settings> Db = new ConcurrentDictionary<string, Settings>();

    static SettingsService () {
      Db.TryAdd("1", new Settings(new TimeSpan(4, 30, 0), "1", 80));
    }

    public Task<Settings> LoadSettings(string deviceId) {
      Settings result;
      Db.TryGetValue(deviceId, out result);
      return Task.FromResult(result);
    }

    public Task SaveSettings(Settings settings) {
      Db.AddOrUpdate(settings.DeviceId, settings, (k, s) => settings);
      return Task.CompletedTask;
    }
  }

  public class BlobStorageSettingsService : BaseQuery, ISettingsService {
    public BlobStorageSettingsService(CloudStorageAccount account) : base(account) {
    }

    public async Task<Settings> LoadSettings(string deviceId) {
      var client = Account.CreateCloudBlobClient();
      var container = client.GetContainerReference("sa-ref-data");
      var blob = container.GetBlockBlobReference("device-list.json");
      var result = await blob.DownloadTextAsync();
      var devices = JsonConvert.DeserializeObject<Settings[]>(result);
      var device = devices.FirstOrDefault(x => x.DeviceId == deviceId);
      return device;
    }

    // CAUTION: this method doesn't work correctly in concurrent szenarios
    // Consider using ETAGs to solve this issue
    public async Task SaveSettings(Settings settings) {
      var client = Account.CreateCloudBlobClient();
      var container = client.GetContainerReference("sa-ref-data");
      var blob = container.GetBlockBlobReference("device-list.json");
      var result = await blob.DownloadTextAsync();
      var devices = JsonConvert.DeserializeObject<Settings[]>(result).ToList();
      devices.RemoveAt(devices.FindIndex(x => x.DeviceId == settings.DeviceId));
      devices.Add(settings);
      var updated = JsonConvert.SerializeObject(devices.ToArray());
      await blob.UploadTextAsync(updated);
    }
  }
}
