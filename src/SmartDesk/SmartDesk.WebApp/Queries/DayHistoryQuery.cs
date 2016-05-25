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
      var durations = tableClient.GetTableReference("changes");
      var minDate = date.Date;
      var maxDate = minDate.AddDays(1).AddMilliseconds(-1);
      var query = 
        new TableQuery<ChangeRow>().Where(
          $"PartitionKey eq '{deviceId}' and RowKey ge '{minDate.ToString("O")}' and RowKey le '{maxDate.ToString("O")}'");
      var results = await durations.FetchRecords(query);

      return
        results.Zip(results.Skip(1),
          (start, end) =>
            new DayHistoryEntry(DateTime.Parse(start.RowKey), DateTime.Parse(end.RowKey),
              Functions.GetActivityType(start.isactive.ToBool(), start.standing.ToBool()))).ToList();

      // TODO query event where timestmap > last change und < max day date
    }
  }
}