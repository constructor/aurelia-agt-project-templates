using CLIC.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CLIC.Resources
{
    public static class OutputText
    {
        public const string Logo = @"
    _               _ _       ___ _     _          _ _____         _    
   /_\ _  _ _ _ ___| (_)__ _ / __| |___| |__  __ _| |_   _|__  ___| |___
  / _ \ || | '_/ -_) | / _` | (_ | / _ \ '_ \/ _` | | | |/ _ \/ _ \ (_-<
 /_/ \_\_,_|_| \___|_|_\__,_|\___|_\___/_.__/\__,_|_| |_|\___/\___/_/__/
 _______________________________________________________________________
";


        public const string Templates = @"
  _____               _      _          
 |_   _|__ _ __  _ __| |__ _| |_ ___ ___
   | |/ -_) '  \| '_ \ / _` |  _/ -_|_-<
   |_|\___|_|_|_| .__/_\__,_|\__\___/__/
                |_|                     
 _______________________________________
";

        public const string AureliaIsReady = @"
    _               _ _        _                      _          
   /_\ _  _ _ _ ___| (_)__ _  (_)___  _ _ ___ __ _ __| |_  _     
  / _ \ || | '_/ -_) | / _` | | (_-< | '_/ -_) _` / _` | || |  _ 
 /_/ \_\_,_|_| \___|_|_\__,_| |_/__/ |_| \___\__,_\__,_|\_, | (_)
                                                        |__/  
 _________________________________________________________________
";

        public const string HelpAdd = @"     add [-t|--templateid] [-h|--here] [-p|--projectfolder] [-i|-install] [-b|--build] [-r|--run] [-w|--watch]
     
     Adds an Aurelia CLI project an existing project. You can then install, build, run and 
     continue to develop your app using the Aurelia CLI.
        ";

        public const string HelpNew = @"     new [-t|--templateid] [-h|--here] [-i|-install] [-b|--build] [-r|--run] [-w|--watch]
     
     Creates a new Aurelia CLI project. You can then install, build, run and continue to develop
     your app using the Aurelia CLI. (NOT YET IMPLEMENTED)
        ";

        public const string HelpTemplates = @"     templates -list : Displays a list of templates available to the user.
        ";

        public static void ShowStatusText()
        {
            Console.WriteLine();
            //Console.WriteLine($"Current working directory: {Environment.CurrentDirectory}");
            //Console.WriteLine();
            Console.WriteLine($"Templates:");
            Console.WriteLine();
            Console.WriteLine($"{TemplateData.GetTempateDataOutput()}");
            Console.WriteLine();
            Console.WriteLine($"Commands: help, add, new");
            Console.WriteLine();
            Console.WriteLine($"{OutputText.HelpAdd}");
            Console.WriteLine();
            Console.WriteLine($"{OutputText.HelpNew}");
            Console.WriteLine();
        }

    }
}
