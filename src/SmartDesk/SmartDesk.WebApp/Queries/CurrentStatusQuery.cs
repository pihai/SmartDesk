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
      var durations = tableClient.GetTableReference("durations");
      var minDate = DateTime.Now.AddMinutes(-1);
      var query =
        new TableQuery<DurationRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge '{minDate.ToString("O")}'");
      var results = await durations.FetchRecords(query);

      return 
        results
          .OrderByDescending(x => x.Timestamp)
          .Select(x => new CurrentStatus(x.isactive.ToBool(),true,Functions.GetActivityType(x.isactive.ToBool(),x.standing.ToBool())))
          .FirstOrDefault() ?? new CurrentStatus(false,false,ActivityType.Inactive);

  
    }
  }
} 