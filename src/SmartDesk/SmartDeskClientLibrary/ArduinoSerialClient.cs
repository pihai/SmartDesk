using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDesk.Client.Arduino
{
    public class ArduinoSerialClient : ISmartDeskClient, IDisposable
    {
        private SerialPort serialPort;
        public ArduinoSerialClient(string portName)
        {
            serialPort = new SerialPort(portName, 9600);
            serialPort.Open();
        }


        public int GetHeight()
        {
            //Send read request
            serialPort.Write(new byte[] { Convert.ToByte('R') }, 0, 1);
            string value = serialPort.ReadLine();
            return int.Parse(value);

        }

        public bool IsConnected()
        {
            return serialPort.IsOpen;
        }

        public bool TryGetHeight(out int height)
        {
            if (!IsConnected())
            {
                height = -1;
                return false;
            }
            height = GetHeight();
            return true;

        }
        public void Dispose()
        {
            serialPort.Dispose();
        }

    }
}
