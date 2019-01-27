using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TCPSerialLinuxHost
{
    public class Port
    {
        StreamReader reader;
        StreamWriter writer;
        Stream baseStream;

        public event EventHandler Closed;
        public event EventHandler<PortReadEventArgs> PortData;

        bool supressClosed = false;

        public string Name { get; private set; }

        public Port(string Name)
        {
            this.Name = Name;
            baseStream = File.Open($"/dev/{Name}", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            reader = new StreamReader(baseStream, Encoding.ASCII);
            writer = new StreamWriter(baseStream, Encoding.ASCII);

            Read();
        }

        public void Close()
        {
            supressClosed = true;

            try
            {
                reader.Close();
                reader.Dispose();
            }
            catch { }

            try
            {
                writer.Close();
                writer.Dispose();
            }
            catch { }

            reader = null;
            writer = null;
        }

        public async void Send(string Data)
        {
            await writer.WriteAsync(Data);
        }

        private async void Read()
        {
            char[] buffer = new char[1024];
            int read = 0;
            try
            {
                Console.WriteLine("Starting port reading");
                while ((read = await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                {
                    string data = new string(new ReadOnlySpan<char>(buffer, 0, read));

                    Console.WriteLine($"Data received from port {data}");

                    if (PortData != null)
                        PortData(this, new PortReadEventArgs { Data = data });
                }
            }
            catch { }

            if (!supressClosed && Closed != null)
                Closed(this, EventArgs.Empty);

            try
            {
                reader.Close();
                reader.Dispose();

            }
            catch { }

            try
            {
                writer.Close();
                writer.Dispose();
            }
            catch { }

            try
            {
                baseStream.Close();
                baseStream.Dispose();
            }
            catch { }

            reader = null;
            writer = null;
        }
    }

    public class PortReadEventArgs
    {
        public string Data { get; set; }
    }
}
