using CLIC.Commands;
using CLIC.Resources;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CLIC.Services
{
    public class CommandProcessingService
    {
        ScreenOutputService screenOutput;
        Dictionary<string, Type> commandTypeMappings;

        public CommandProcessingService(string[] args)
        {
            screenOutput = new ScreenOutputService();
            commandTypeMappings = new Dictionary<string, Type>();
            populateCommandMappings();

            if (args.Length > 0)
                ProcessCommand(string.Join(" ", args));
        }

        void populateCommandMappings()
        {
            // Add to template
            commandTypeMappings.Add("add", typeof(AddTemplateCommand));
            // New template
            commandTypeMappings.Add("new", typeof(TemplateSpecificationCommand));
            // New template
            commandTypeMappings.Add("help", typeof(HelpCommand));
        }

        public void ReadCommand()
        {
            Console.Write("Au_> ");
            var command = Console.ReadLine();
            ProcessCommand(command);
        }

        public void ProcessCommand(string cmdStr)
        {
            if (String.IsNullOrEmpty(cmdStr))
                NotRecognised();

            var parts = cmdStr.Split(" ");
            var c = parts[0];

            if (IsQuit(c))
            {
                QuitTool();
                return;
            }

            ExecuteCommand(c, parts);
            //ReadCommand();
        }

        void ExecuteCommand(string cmd, string[] args)
        {
            if (!commandTypeMappings.ContainsKey(cmd))
            {
                ForwardCommand(cmd, args);
                return;
            }

            Type cT = commandTypeMappings[cmd];

            ExecuteCommandType(cT, args);
        }

        void ExecuteCommandType(Type cmdT, string[] args)
        {
            ICLICommand o = (ICLICommand)Activator.CreateInstance(cmdT);
            o.OnQuit += (object sender, EventArgs e) => QuitCommand();
            o.Execute(args);
        }

        void ExecuteCommandType<T>(string[] args) where T : ICLICommand, new()
        {
            T o = new T();
            o.OnQuit += (object sender, EventArgs e) => QuitCommand();
            o.Execute(args);
        }

        public void QuitCommand()
        {
            Console.WriteLine();
            Console.WriteLine("Operation cancelled.");
            Console.WriteLine();
            ReadCommand();
        }

        public void QuitTool()
        {
            Console.WriteLine();
            Console.WriteLine("Leaving Aurelia Global Tools");
            Console.WriteLine();
        }

        public void ForwardCommand(string command, string[] args)
        {
            var fullCommand = string.Join(" ", args);
            var result = CommandTools.ExecuteCommand(fullCommand);

            if (!string.IsNullOrEmpty(result))
                Console.WriteLine(result);
            else
                NotRecognised();
        }

        public void NotRecognised()
        {
            Console.WriteLine("");
            Console.WriteLine($"Command not recognised");
            Console.WriteLine("");
            ReadCommand();
        }


        private bool IsQuit(string command)
        {
            var a = command.Trim().ToUpper();
            return a == "Q" || a == "QUIT";
        }

    }
}
