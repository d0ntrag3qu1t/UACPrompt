using System;
using System.Diagnostics;
using System.ComponentModel;

namespace ElevatedProcessExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a PID as a command-line argument.");
                return;
            }

            int pid;
            if (!int.TryParse(args[0], out pid))
            {
                Console.WriteLine("Invalid PID provided.");
                return;
            }

            Process targetProcess;
            try
            {
                targetProcess = Process.GetProcessById(pid);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"No process with PID {pid} is running.");
                return;
            }

            string executablePath = targetProcess.MainModule.FileName;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Verb = "runas", // Request elevation
                    UseShellExecute = true
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };

                process.Start();
                // process.WaitForExit(); // Wait for the launched process to finish
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
