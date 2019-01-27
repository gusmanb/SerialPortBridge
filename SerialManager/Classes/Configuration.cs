using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SerialManager.Classes
{
    public class Configuration
    {
        public IPAddress Server { get; set; }
        public int Port { get; set; }
        public string Com0comPath { get; set; }
        public List<PortBridgeConfig> Bridges { get; set; } = new List<PortBridgeConfig>();

        static JsonSerializerSettings settings;

        static Configuration()
        {
            settings = new JsonSerializerSettings();
            settings.Converters.Add(new IPAddressConverter());
            settings.Formatting = Formatting.Indented;

        }

        public static Configuration Load()
        {
            try
            {
                if (File.Exists("config.ini"))
                {
                    string config = File.ReadAllText("config.ini");
                    return JsonConvert.DeserializeObject<Configuration>(config, settings);
                }
            }
            catch { }

            return new Configuration();
        }

        public bool LocalPortUsed(string PortName)
        {
            return Bridges.Any(b => b.LocalPort == PortName);
        }

        public bool RemotePortUsed(string PortName)
        {
            return Bridges.Any(b => b.RemotePort == PortName);
        }

        public void Save()
        {
            File.WriteAllText("config.ini", JsonConvert.SerializeObject(this, settings));
        }

        public bool AddBridge(string LocalPort, string RemotePort, int Bauds)
        {
            if (Bridges.Any(b => b.LocalPort == LocalPort) || Bridges.Any(b => b.RemotePort == RemotePort))
                return false;

            Bridges.Add(new PortBridgeConfig { RemotePort = RemotePort, LocalPort = LocalPort, Bauds = Bauds });

            return true;
        }

        public bool RemoveBridge(string RemotePort)
        {
            var bridge = Bridges.Where(b => b.RemotePort == RemotePort).FirstOrDefault();

            if (bridge == null)
                return false;

            Bridges.Remove(bridge);

            return true;
        }

        public class PortBridgeConfig
        {
            public string LocalPort { get; set; }
            public string RemotePort { get; set; }
            public int Bauds { get; set; }
        }

        class IPAddressConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(IPAddress));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return IPAddress.Parse((string)reader.Value);
            }
        }

    }
}
