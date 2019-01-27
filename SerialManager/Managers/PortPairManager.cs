using Com0Com;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialManager.Models
{
    public class PortPairManager
    {
        Com0ComManager manager;

        public IEnumerable<CrossoverPortPair> PortPairs { get { return manager.GetCrossoverPortPairs();  } }
        public IEnumerable<string> FreePorts
        {
            get
            {
                var names = SerialPort.GetPortNames();
                var pairs = PortPairs;

                List<string> freeNames = new List<string>();

                for (int buc = 1; buc < 100; buc++)
                {
                    string name = "COM" + buc;

                    if (names.Contains(name) || pairs.Any(p => p.PortNameA == name || p.PortNameB == name))
                        continue;

                    freeNames.Add(name);
                }

                return freeNames;
            }
        }

        public PortPairManager(string Com0comPath)
        {
            manager = new Com0ComManager(Com0comPath);
        }

        public bool CreatePair(string InputPort, string OutputPort)
        {
            var freePorts = FreePorts;

            if (!freePorts.Contains(InputPort) || !freePorts.Contains(OutputPort))
                return false;

            try
            {
                var pair = manager.CreatePortPair(InputPort, OutputPort);
                return pair != null ;
            }
            catch { return false; }
        }

        public bool DeletePair(int PairNumber)
        {
            try
            {
                manager.DeletePortPair(PairNumber);
                return true;
            }
            catch { return false; }
        }
    }
}
