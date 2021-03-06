﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SmartDesk.Shared.Queries;

namespace SmartDesk.WebApp.Services {
  public class SettingsService : ISettingsService {
    private static readonly ConcurrentDictionary<int, Settings> Db = new ConcurrentDictionary<int, Settings>();

    static SettingsService () {
      Db.TryAdd(1, new Settings(new TimeSpan(4, 30, 0), 1, 80));
    }

    public Task<Settings> LoadSettings(int deviceId) {
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

    public async Task<Settings> LoadSettings(int deviceId) {
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
      var existingIndex = devices.FindIndex(x => x.DeviceId == settings.DeviceId);
      if(existingIndex != -1) devices.RemoveAt(existingIndex);
      devices.Add(settings);
      var updated = JsonConvert.SerializeObject(devices.ToArray());
      await blob.UploadTextAsync(updated);
      var now = DateTime.UtcNow.AddMinutes(1);

      var saBlobRef = container.GetBlockBlobReference($"{now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/00-00/device-list.json");
      await saBlobRef.UploadTextAsync(updated);

      saBlobRef = container.GetBlockBlobReference($"{now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{now.ToString("HH-mm", CultureInfo.InvariantCulture)}/device-list.json");
      await saBlobRef.UploadTextAsync(updated);

      //saBlobRef = container.GetBlockBlobReference($"{now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{now.ToString("HH-mm", CultureInfo.InvariantCulture)}/device-list.json");
      //await saBlobRef.UploadTextAsync(updated);
    }
  }
}
