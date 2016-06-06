using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.Queries.Dtos;
using SmartDesk.WebApp.Queries.TableEntities;

namespace SmartDesk.WebApp.Queries {
  public class TotalStatsQuery : BaseQuery {
    public TotalStatsQuery(CloudStorageAccount account) : base(account) {
    }

    public async Task<TotalStats> Query(string deviceId) {
      var tableClient = Account.CreateCloudTableClient();
      var durations = tableClient.GetTableReference("durations");

      var query =
        new TableQuery<DurationRow>().Where(
          $"PartitionKey eq '{deviceId}'");
      var results = await durations.FetchRecords(query);

      var grouped =
        results
          .Select(x => new {
            type = Functions.GetActivityType(x.isactive.ToBool(), x.standing.ToBool(), true),
            duration = x.duration
          })
          .GroupBy(x => x.type)
          .Select(x => new { type = x.Key, duration = x.Select(y => y.duration).Sum() })
          .ToList();

      var active =
        results
         .Where(x => x.isactive.ToBool())
         .Select(x => x.duration).Sum();

      return new TotalStats(
        active,
        grouped.FirstOrDefault(x => x.type == ActivityType.Standing)?.duration ?? 0,
        grouped.FirstOrDefault(x => x.type == ActivityType.Sitting)?.duration ?? 0

        );
    }
  }
}