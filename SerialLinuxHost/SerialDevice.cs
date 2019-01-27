using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Ports
{
    public class SerialDevice : IDisposable
    {
        public const int READING_BUFFER_SIZE = 1024;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken CancellationToken => cts.Token;

        private Stream serialStream;
        
        protected readonly string portName;

        protected readonly int baudRate;

        public bool IsOpened => serialStream != null;

        public string Name => portName;

        public event EventHandler<SerialDeviceReceiveArgs> DataReceived;
        public event EventHandler Closed;

        bool supressClose = false;

        public SerialDevice(string portName, int baudRate)
        {
            this.portName = portName;
            this.baudRate = baudRate;
        }

        public unsafe void Open()
        {
            ProcessStartInfo pi = new ProcessStartInfo("BaudSetter", $"/dev/{portName} {baudRate}");
            Process.Start(pi).WaitForExit();

            serialStream = File.Open($"/dev/{portName}", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            Task.Run((Action)StartReading, CancellationToken);

        }

        private void StartReading()
        {
            try
            {
                if (serialStream == null)
                    throw new Exception();

                byte[] readBuffer = new byte[READING_BUFFER_SIZE];

                while (true)
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    int res = serialStream.Read(readBuffer, 0, readBuffer.Length);

                    if (res > 0)
                    {
                        byte[] buf = new byte[res];
                        Buffer.BlockCopy(readBuffer, 0, buf, 0, res);

                        if (DataReceived != null)
                            DataReceived(this, new SerialDeviceReceiveArgs { Data = buf });
                    }
                    else
                    {
                        if (!supressClose)
                        {
                            Close();

                            if (Closed != null)
                                Closed(this, EventArgs.Empty);

                        }

                        return;
                    }
                }
            }
            catch { }
        }

        public void Close()
        {
            supressClose = true;

            if (!IsOpened)
                return;

            serialStream.Close();
            serialStream.Dispose();
            serialStream = null;
        }

        public bool Write(byte[] buf)
        {
            try
            {
                if (!IsOpened)
                    return false;

                serialStream.Write(buf, 0, buf.Length);
                serialStream.Flush();
                return true;
            }
            catch { return false; }
        }

        public static string[] GetPortNames()
        {
            try
            {
                List<string> serialPorts = new List<string>();

                string[] ttys = System.IO.Directory.GetFiles("/dev/", "tty*");
                foreach (string dev in ttys)
                    serialPorts.Add(dev);

                ttys = System.IO.Directory.GetFiles("/dev/", "serial*");

                foreach (string dev in ttys)
                    serialPorts.Add(dev);

                return serialPorts.ToArray();
            }
            catch { return null; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (IsOpened)
            {
                Close();
            }
        }
    }

    public class SerialDeviceReceiveArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}