using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.Queries.Dtos;
using SmartDesk.WebApp.Queries.TableEntities;

namespace SmartDesk.WebApp.Queries {
  public class CurrentStatusQuery : BaseQuery {
    public CurrentStatusQuery(CloudStorageAccount account) : base(account) {
    }

    public async Task<CurrentStatus> Query(string deviceId) {
      var tableClient = Account.CreateCloudTableClient();
      var deviceUniqueClient = tableClient.GetTableReference("deviceunique");
      var query =
        new TableQuery<LastEventRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge 'LastEvent'");
      var lastEvents = await deviceUniqueClient.FetchRecords(query);
      var lastEvent = lastEvents.FirstOrDefault();
      var activityType = Functions.GetActivityType(lastEvent.isactive.ToBool(), lastEvent.standing.ToBool(), lastEvent.isonline.ToBool());
      return new CurrentStatus(lastEvent.isactive.ToBool(), lastEvent.isonline.ToBool(), activityType, lastEvent.height);
    }
  }
} 