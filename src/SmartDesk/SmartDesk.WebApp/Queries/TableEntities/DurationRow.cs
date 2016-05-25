using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmartDesk.WebApp.Queries.TableEntities {
  public class DurationRow : TableEntity {
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public long duration { get; set; }
    public string deviceid { get; set; }
    public long isactive { get; set; }
    public long standing { get; set; }
  }
}