using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDesk.Client {
  public interface ISmartDeskClient: IDisposable {
    bool IsConnected();

    int GetHeight();

    bool TryGetHeight(out int height);

  }
}
