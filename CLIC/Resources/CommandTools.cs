using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CLIC.Resources
{
    public static class CommandTools
    {
        public static string ExecuteCommand(string command)
        {
            string result = string.Empty;

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isWindows)
                result = ExecuteWinCommand(command);

            if (isOSX)
                result = ExecuteOSXCommand(command);

            if (isLinux)
                result = ExecuteLinuxCommand(command);

            return result;
        }

        private static string ExecuteWinCommand(string command)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                string result = process.StandardOutput.ReadToEnd();
                //Console.WriteLine(result);
                return result;
            }
        }

        private static Process ExecuteWinCommands(string[] commands)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    foreach (var command in commands)
                        sw.WriteLine(command);
                }
            }

            return p;
        }

        private static string ExecuteOSXCommand(string command)
        {
            //Console.WriteLine("ExecuteLinuxCommand NOT IMPLEMENTED. No action was taken.");
            //throw new NotImplementedException();
            return "ExecuteLinuxCommand NOT IMPLEMENTED. No action was taken.";
        }

        private static string ExecuteLinuxCommand(string command)
        {
            //Console.WriteLine("ExecuteLinuxCommand NOT IMPLEMENTED. No action was taken.");
            //throw new NotImplementedException();
            return "ExecuteLinuxCommand NOT IMPLEMENTED. No action was taken.";
        }

    }
}
