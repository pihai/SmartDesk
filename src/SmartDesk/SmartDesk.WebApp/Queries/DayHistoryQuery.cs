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
      var minDate = date.Date;
      var maxDate = minDate.AddDays(1).AddMilliseconds(-1);
      var query =
        new TableQuery<ChangeRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge '{minDate.ToString("O")}' and RowKey le '{maxDate.ToString("O")}'");
      var durations = await durationsClient.FetchRecords(query);


      var result =
        durations
          .Pairwise(Tuple.Create)
          .Where(pair => pair.Item1.isonline.ToBool() && pair.Item2.isonline.ToBool())
          .Select(pair =>
            new DayHistoryEntry(
              DateTime.Parse(pair.Item1.RowKey),
              DateTime.Parse(pair.Item2.RowKey),
              Functions.GetActivityType(
                pair.Item1.isactive.ToBool(),
                pair.Item1.standing.ToBool()
              )
            )
          )
          .ToList();
      return result;
      // TODO query event where timestmap > last change und < max day date
    }
  }
}