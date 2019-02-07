using System;
using CLIC.Data;
using CLIC.Resources;
using CLIC.Services;

namespace CLIC
{
    //https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file

    class Program
    {
        static CommandProcessingService cps;

        static void Main(string[] args)
        {
            //Console.BackgroundColor = ConsoleColor.DarkBlue;
            //Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.WriteLine(OutputText.Logo);
            OutputText.ShowStatusText();

            Console.WriteLine($"Passed args as command: {string.Join(" ", args)}");
            Console.WriteLine();

            cps = new CommandProcessingService(args);

            //begin the CLI
            cps.ReadCommand();
        }

    }
}
