using Microsoft.WindowsAzure.Storage;

namespace SmartDesk.Shared.Queries {
  public class BaseQuery {
    protected readonly CloudStorageAccount Account;

    public BaseQuery(CloudStorageAccount account) {
      Account = account;
    }
  }
}