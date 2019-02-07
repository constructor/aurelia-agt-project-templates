using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CLIC.Commands
{
    public class BuildAndRunCommand : ICLICommand
    {
        public event EventHandler OnQuit;

        public void Execute(string[] args)
        {
            BuildAndRunAureliaApp(args[0]);
        }

        private static Process BuildAndRunAureliaApp(string path)
        {
            FileInfo f = new FileInfo(path);
            string drive = Path.GetPathRoot(f.FullName);

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
                    sw.WriteLine($"cd {path}");
                    sw.WriteLine($"{drive}");
                    sw.WriteLine("yarn install");
                    sw.WriteLine("au build");
                }
            }

            return p;
        }

        private static void ExecuteCommand(string command)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isWindows)
                ExecuteWinCommand(command);

            if (isOSX)
                ExecuteOSXCommand(command);

            if (isLinux)
                ExecuteLinuxCommand(command);
        }

        private static void ExecuteWinCommand(string args)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + args);

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
                Console.WriteLine(result);
            }
        }

        private static void ExecuteOSXCommand(string args)
        {
            Console.WriteLine("ExecuteOSXCommand NOT IMPLEMENTED. No action was taken.");
            //throw new NotImplementedException();
        }

        private static void ExecuteLinuxCommand(string args)
        {
            Console.WriteLine("ExecuteLinuxCommand NOT IMPLEMENTED. No action was taken.");
            //throw new NotImplementedException();
        }

    }

}
