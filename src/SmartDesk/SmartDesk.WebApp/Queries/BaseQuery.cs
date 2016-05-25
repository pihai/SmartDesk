using Microsoft.WindowsAzure.Storage;

namespace SmartDesk.WebApp.Queries {
  public class BaseQuery {
    protected readonly CloudStorageAccount Account;

    public BaseQuery(CloudStorageAccount account) {
      Account = account;
    }
  }
}