using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Com0Com
{
    public class Com0ComManager
    {
        private readonly CmdRunner _cmdRunner;
        private readonly string _com0ComSetupC;

        /// <summary>
        /// Create a setupc.exe facade using the default implementation of ICmdRunner
        /// </summary>
        /// <param name="com0ComSetupC"></param>
        public Com0ComManager(string InstallPath = null)
        {
            if(InstallPath == null)
            { 
                var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                reg = reg.OpenSubKey("SOFTWARE");

                if (reg == null)
                    throw new KeyNotFoundException("Cannot find registry key SOFTWARE");

                reg = reg.OpenSubKey("com0com");

                if (reg == null)
                    throw new KeyNotFoundException("Cannot find com0com installation in registry, maybe it's not installed?");

                var path = reg.GetValue("Install_Dir", null);

                if(path == null)
                    throw new KeyNotFoundException("Cannot find com0com installation in registry, maybe it's not installed?");

                _com0ComSetupC = Path.Combine(path.ToString(), "setupc.exe");

            }
            else 
                _com0ComSetupC = InstallPath;

            if (!File.Exists(_com0ComSetupC))
                throw new FileNotFoundException("Cannot find setupc.exe in the com0com installation path");

            _cmdRunner = new CmdRunner();
        }

        /// <summary>
        /// Get all com0com null-modem connections installed on the system
        /// </summary>
        /// <returns>All com0com null-modem connections installed on the system</returns>
        public IEnumerable<CrossoverPortPair> GetCrossoverPortPairs()
        {
            var stdOutLines = _cmdRunner.RunCommandGetStdOut(
                Path.GetDirectoryName(_com0ComSetupC),
                _com0ComSetupC,
                "list");

            return ParsePortPairsFromStdOut(stdOutLines.Select(l => l.Trim()));
        }

        /// <summary>
        /// Get all com0com null-modem connections installed on the system
        /// </summary>
        /// <returns>All com0com null-modem connections installed on the system</returns>
        public async Task<IEnumerable<CrossoverPortPair>> GetCrossoverPortPairsAsync()
        {
            return await Task.Run(() => GetCrossoverPortPairs());
        }

        /// <summary>
        /// Create a com0com null-modem connection between virtual com ports with default names
        /// </summary>
        /// <returns>The created virtual port pair</returns>
        public CrossoverPortPair CreatePortPair()
        {
            var stdOutLines = _cmdRunner.RunCommandGetStdOut(
                Path.GetDirectoryName(_com0ComSetupC),
                _com0ComSetupC,
                "install - -");

            var ret = ParsePortPairsFromStdOut(stdOutLines.Select(l => l.Trim())).First();

            return ret;
        }

        /// <summary>
        /// Create a com0com null-modem connection between virtual com ports with default names
        /// </summary>
        /// <returns>The created virtual port pair</returns>
        public async Task<CrossoverPortPair> CreatePortPairAsync()
        {
            return await Task.Run(() => CreatePortPair());
        }

        /// <summary>
        /// Create a com0com null-modem connection between virtual com ports with specified names
        /// </summary>
        /// <param name="comPortNameA">The name of virtual com port A</param>
        /// <param name="comPortNameB">The name of virtual com port B</param>
        /// <returns>The created virtual port pair</returns>
        public CrossoverPortPair CreatePortPair(string comPortNameA, string comPortNameB)
        {
            var stdOutLines = _cmdRunner.RunCommandGetStdOut(
                Path.GetDirectoryName(_com0ComSetupC),
                _com0ComSetupC,
                $"install portname={comPortNameA} portname={comPortNameB}");

            return ParsePortPairsFromStdOut(stdOutLines.Select(l => l.Trim())).First();
        }

        /// <summary>
        /// Create a com0com null-modem connection between virtual com ports with specified names
        /// </summary>
        /// <param name="comPortNameA">The name of virtual com port A</param>
        /// <param name="comPortNameB">The name of virtual com port B</param>
        /// <returns>The created virtual port pair</returns>
        public async Task<CrossoverPortPair> CreatePortPairAsync(string comPortNameA, string comPortNameB)
        {
            return await Task.Run(() => CreatePortPair(comPortNameA, comPortNameB));
        }

        /// <summary>
        /// Remove a com0com null-modem connection and the two virtual com ports associated with the connection
        /// </summary>
        /// <param name="n"></param>
		public void DeletePortPair(int n)
        {
            _cmdRunner.RunCommandGetStdOut(
                Path.GetDirectoryName(_com0ComSetupC),
                _com0ComSetupC,
                $"remove {n}");
        }

        /// <summary>
        /// Remove a com0com null-modem connection and the two virtual com ports associated with the connection
        /// </summary>
        /// <param name="n"></param>
        public async Task DeletePortPairAsync(int n)
        {
            await Task.Run(() => DeletePortPair(n));
        }

        private IEnumerable<CrossoverPortPair> ParsePortPairsFromStdOut(IEnumerable<string> lines)
        {
            var portAMap = new Dictionary<int, string>();
            var portBMap = new Dictionary<int, string>();
            var lineRegex = new Regex(@"^CNC([AB])(\d+)\sPortName=(-|\w+)");

            foreach (var line in lines)
            {
                var match = lineRegex.Match(line);
                if (!match.Success) continue;

                var aOrB = match.Groups[1].Value;
                var portNum = Convert.ToInt32(match.Groups[2].Value);
                var portName = match.Groups[3].Value;

                if (aOrB == "A")
                {
                    portAMap.Add(portNum, portName);
                }
                else
                {
                    portBMap.Add(portNum, portName);
                }
            }

            var ret = new List<CrossoverPortPair>();
            foreach (var key in portAMap.Keys)
            {
                ret.Add(new CrossoverPortPair(portAMap[key], portBMap[key], key));
            }

            return ret;
        }

        public class CmdRunner
        {
            /// <summary>
            /// Run a command on the cmd line and get the standard out with no shell execute and no window
            /// </summary>
            /// <param name="workingDir">The working directory to run the command in</param>
            /// <param name="command">The command to run</param>
            /// <param name="args">The args to supply to the command</param>
            /// <returns>Lines of the Standard Out</returns>
            public string[] RunCommandGetStdOut(string workingDir, string command, string args)
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = workingDir,
                        FileName = command,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();

                while (!proc.HasExited)
                {
                    Thread.Sleep(100);
                }

                if (proc.ExitCode != 0)
                    throw new ApplicationException($"Exit code of {proc.ExitCode} received when running '{command} {args}'");

                var ret = new List<string>();
                while (!proc.StandardOutput.EndOfStream)
                {
                    ret.Add(proc.StandardOutput.ReadLine());
                }

                return ret.ToArray();
            }
        }
    }

    

    public class CrossoverPortPair
    {
        public CrossoverPortPair(string portNameA, string portNameB, int number)
        {
            PortNameA = portNameA;
            PortNameB = portNameB;
            PairNumber = number;
        }

        public string PortNameA { get; }
        public string PortNameB { get; }
        public int PairNumber { get; }

        public override string ToString()
        {
            return $"{PairNumber}: {PortNameA}<->{PortNameA}";

        }
    }

}
