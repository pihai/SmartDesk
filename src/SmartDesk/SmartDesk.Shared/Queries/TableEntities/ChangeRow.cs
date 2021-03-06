using Microsoft.WindowsAzure.Storage.Table;

namespace SmartDesk.Shared.Queries.TableEntities {
  public class ChangeRow : TableEntity {
    public long deviceid { get; set; }
    public long isactive { get; set; }
    public long standing { get; set; }
    public long isonline { get; set; }
  }

  public class LastEventRow : TableEntity {
    public long deviceid { get; set; }
    public long isactive { get; set; }
    public long standing { get; set; }
    public long isonline { get; set; }
    public double height { get; set; }
  }
}