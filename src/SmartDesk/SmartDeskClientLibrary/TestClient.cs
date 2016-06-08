using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDesk.Client.Arduino {
  public class TestClient : ISmartDeskClient {
    private Random rand;

    public TestClient(int seed) {
      rand = new Random(seed);
    }

    public TestClient() {
      rand = new Random();
    }

    public int GetHeight() {
      if (rand.Next(0, 100) > 50)
        return rand.Next(65, 75);
      return rand.Next(95, 110);
    }

    public bool IsConnected() {
      return true;
    }

    public bool TryGetHeight(out int height) {

      height = GetHeight();
      return true;
    }

    public void Dispose() {
      // nothing to do
    }
  }
}
