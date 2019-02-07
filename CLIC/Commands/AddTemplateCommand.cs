using CLIC.Data;
using CLIC.Resources;
using CLICServices;
using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CLIC.Commands
{
    public class AddTemplateCommand : ICLICommand
    {
        public event EventHandler OnQuit;
        public string TemplateRoot { get; set; }

        PatchApplicationService patchApplicationService;
        AddTemplateCommandModel model;

        public AddTemplateCommand()
        {
            patchApplicationService = new PatchApplicationService(Utils.IOUtils.AssemblyDirectory);
        }

        public void Execute(string[] args)
        {
            model = ProcessArgs(args);

            if (model.ShowHelp)
            {
                Console.WriteLine();
                Console.WriteLine(OutputText.HelpAdd);
                var o = TemplateData.GetTempateDataOutput();
                Console.WriteLine(o);
                Console.WriteLine();
            }
            else if (model.IsValid())
            {
                //patchApplicationService.AddAureliaToProject(model.TemplateId, model.TemplateRoot);
                var ti = TemplateData.AllTemplates.Where(x => x.TemplateId == model.TemplateId).SingleOrDefault();
                if (ti != null)
                    patchApplicationService.AddAureliaToProject(ti, model.TemplateRoot);
                else {
                    Console.WriteLine("Error! No template item found...");
                }

                Console.WriteLine();
                Console.WriteLine("Installing Aurelia...");
                Console.WriteLine();

                try
                {
                    //TODO: make cross platform
                    if (model.HasPostBuildAction)
                    {
                        if (model.TemplateId == 1)
                            BuildAndRunAureliaApp(model).WaitForExit();

                        if (model.TemplateId == 2)
                            BuildAndRunAureliaAppVS(model).WaitForExit();

                        if (model.TemplateId == 3)
                            BuildAndRunAureliaAppVS(model).WaitForExit();

                    }
                }
                catch (Exception ex)
                {
                    var exx = ex;
                }

                Console.WriteLine();
                Console.WriteLine(OutputText.AureliaIsReady);
                Console.WriteLine();
            }
            else {
                Console.WriteLine();
                Console.WriteLine($"Error parsing arguments for command: {model.ErrorMessage}{Environment.NewLine}Please check and try again.");
                Console.WriteLine();
            }
        }

        private AddTemplateCommandModel ProcessArgs(string[] args)
        {
            args = args.ToList().Except(new string[] { "add" }).ToArray();
            model = new AddTemplateCommandModel();
            var m = model;

            var tn = "";

//#if DEBUG
//            var hereDebugDirectory = @"H:\Dev\T\1\ProjectName1";//static local machine directory for debug purposes only
//            var p = new OptionSet() {
//                { "n|name=", "The name of the project", v => {m.ProjectName = v; tn = v; } },
//                { "h|here", "Use the current directory of the console", v => m.RootDirectory = hereDebugDirectory },
//                { "p|projectfolder=", "The root directory of the source project", v => m.RootDirectory = v },
//                { "t|templateid=", "The TemplateId to apply to the source project", (int v) => m.TemplateId = v },
//                { "i|install", "Install the Aurelia CLI project dependencies after template processing", (v) => m.InstallAfter = true },
//                { "b|build", "Build the Aurelia CLI project after template processing", (v) => m.BuildAfter = true },
//                { "r|run", "Run the Aurelia CLI project after template processing", (v) => m.RunAfter = true },
//                { "w|watch", "Watch the Aurelia CLI project after template processing", (v) => m.RunAfter = true }
//            };
//#else
            var p = new OptionSet() {
                { "n|name=", "The name of the project", v => {m.ProjectName = v; tn = v; } },
                { "h|here", "Use the current directory of the console", v => m.TemplateRoot = Environment.CurrentDirectory },
                { "p|projectfolder=", "The root directory of the source project", v => m.TemplateRoot = v },
                { "t|templateid=", "The TemplateId to apply to the source project", (int v) => m.TemplateId = v },
                { "i|install", "Install the Aurelia CLI project dependencies after template processing", (v) => m.InstallAfter = true },
                { "b|build", "Build the Aurelia CLI project after template processing", (v) => m.BuildAfter = true },
                { "r|run", "Run the Aurelia CLI project after template processing", (v) => m.RunAfter = true },
                { "w|watch", "Watch the Aurelia CLI project after template processing", (v) => m.RunAfter = true }
            };
//#endif

            ParseArgs(args, p);

            // get the project filename as generated by the users IDE
            // TODO: map the project name given by the user to their project to the relevant files during the patching application process
            model.SourceProjectName = patchApplicationService.GetSourceProjectName(model.TemplateId, model.TemplateRoot);

            m.ShowHelp = args.Count() == 0 || string.IsNullOrEmpty(args[0]);

            return m;
        }

        private void ProjectFolderIdCheck()
        {
            //TODO: implement project folder / templateid compatibility
        }

        private bool ParseArgs(string[] args, OptionSet p)
        {
            try
            {
                p.Parse(args);
                return true;
            }
            catch (Exception ex)
            {
                model.ArgumentError = true;
                model.ErrorMessage = ex.Message;
                return false;
            }
        }

        private static Process BuildAndRunAureliaApp(AddTemplateCommandModel model)
        {
            var templateRoot = model.TemplateRoot;
            FileInfo fi = new FileInfo(templateRoot);
            string drive = Path.GetPathRoot(fi.FullName);

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
                    sw.WriteLine($"cd {templateRoot}");
                    sw.WriteLine($"{drive}");

                    if(model.InstallAfter)
                        sw.WriteLine("yarn install");

                    if (model.BuildAfter && !model.WatchAfter)
                        sw.WriteLine("au build");

                    if (model.WatchAfter && !model.RunAfter) //watch overrides build and run overrides watch
                        sw.WriteLine("au build --watch");

                    if (model.RunAfter)
                        sw.WriteLine("au run");
                }
            }

            return p;
        }

        private static Process BuildAndRunAureliaAppVS(AddTemplateCommandModel model)
        {
            var templateRoot = model.TemplateRoot;
            //FileInfo fi = new FileInfo(templateRoot);
            //string drive = Path.GetPathRoot(fi.FullName);

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
                    sw.WriteLine($"cd {templateRoot}\\{model.SourceProjectName}");
                    //sw.WriteLine($"{drive}");

                    if (model.InstallAfter)
                        sw.WriteLine("yarn install");

                    if (model.BuildAfter && !model.WatchAfter)
                        sw.WriteLine("au build");

                    if (model.WatchAfter && !model.RunAfter) //watch overrides build and run overrides watch
                        sw.WriteLine("au build --watch");

                    if (model.RunAfter)
                        sw.WriteLine("au run");
                }
            }

            return p;
        }

    }
}
