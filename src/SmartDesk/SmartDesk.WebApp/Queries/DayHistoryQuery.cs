using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.Queries.Dtos;
using SmartDesk.WebApp.Queries.TableEntities;

namespace SmartDesk.WebApp.Queries {
  public class DayHistoryQuery : BaseQuery {
    public DayHistoryQuery(CloudStorageAccount account) : base(account) {
    }

    public async Task<List<DayHistoryEntry>> Query(string deviceId, DateTime date) {
      var tableClient = Account.CreateCloudTableClient();
      var durationsClient = tableClient.GetTableReference("changes");

      // query all changes between 00:00 and 23:59.59
      var minDate = date.Date;
      var maxDate = minDate.AddDays(1).AddMilliseconds(-1);
      var query =
        new TableQuery<ChangeRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge '{minDate.ToString("O")}' and RowKey le '{maxDate.ToString("O")}'");
      var durations = await durationsClient.FetchRecords(query);

      var lastEvent = Enumerable.Empty<LastEventRow>();
      // hacky
      if (date.Date == DateTime.Today) {

        // query the last known event, because the current period has not end yet
        var deviceUniqueClient = tableClient.GetTableReference("deviceunique");
        var query2 =
          new TableQuery<LastEventRow>().Where(
            $"PartitionKey eq '{deviceId}' and RowKey ge 'LastEvent'");
        lastEvent = await deviceUniqueClient.FetchRecords(query2);
      }

      var result =
        durations.Concat(lastEvent.Select(x => new ChangeRow { isonline = x.isonline, isactive = x.isactive, standing = x.standing, RowKey = x.Timestamp.ToString("o")}))
          .Pairwise(Tuple.Create)
          .Where(pair => pair.Item1.isonline.ToBool())
          .Select(pair =>
            new DayHistoryEntry(
              DateTime.Parse(pair.Item1.RowKey),
              DateTime.Parse(pair.Item2.RowKey),
              Functions.GetActivityType(
                pair.Item1.isactive.ToBool(),
                pair.Item1.standing.ToBool(),
                true
                )
              )
          )
          .ToList();
      return result;
    }
  }
}