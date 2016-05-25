using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.Queries.Dtos;
using SmartDesk.WebApp.Queries.TableEntities;

namespace SmartDesk.WebApp.Queries {
  public class DayRatioQuery : BaseQuery {
    public DayRatioQuery(CloudStorageAccount account) : base(account) {
    }

    public async Task<DayRatio> Query(string deviceId, DateTime date) {
      var tableClient = Account.CreateCloudTableClient();
      var durations = tableClient.GetTableReference("durations");
      var minDate = date.Date;
      var maxDate = minDate.AddDays(1).AddMilliseconds(-1);
      var query =
        new TableQuery<DurationRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge '{minDate.ToString("O")}' and RowKey le '{maxDate.ToString("O")}'");
      var results = await durations.FetchRecords(query);

      var grouped =
        results
          .Select(x => new {
            type = Functions.GetActivityType(x.isactive.ToBool(), x.standing.ToBool()),
            duration = x.duration
          })
          .GroupBy(x => x.type)
          .Select(x => new {type = x.Key, duration = x.Select(y => y.duration).Sum()})
          .ToList();

      return new DayRatio(
        grouped.FirstOrDefault(x => x.type == ActivityType.Standing)?.duration ?? 0,
        grouped.FirstOrDefault(x => x.type == ActivityType.Sitting)?.duration ?? 0,
        grouped.FirstOrDefault(x => x.type == ActivityType.Inactive)?.duration ?? 0
        );
    }
  }
}