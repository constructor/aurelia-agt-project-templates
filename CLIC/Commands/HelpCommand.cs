using CLIC.Data;
using CLIC.Resources;
using CLIC.Services;
using CLICServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CLIC.Commands
{
    public class HelpCommand : ICLICommand
    {
        public event EventHandler OnQuit;
        ScreenOutputService screenOutput;

        public HelpCommand()
        {
            screenOutput = new ScreenOutputService();
        }

        public void Execute(string[] args)
        {
            OutputText.ShowStatusText();
        }

    }
}
