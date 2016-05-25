using Microsoft.WindowsAzure.Storage.Table;

namespace SmartDesk.WebApp.Queries.TableEntities {
  public class ChangeRow : TableEntity {
    public string deviceid { get; set; }
    public long isactive { get; set; }
    public long standing { get; set; }
  }
}