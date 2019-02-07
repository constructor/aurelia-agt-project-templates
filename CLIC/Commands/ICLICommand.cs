using System;
using System.Collections.Generic;
using System.Text;

namespace CLIC.Commands
{
    public interface ICLICommand
    {
        event EventHandler OnQuit;
        void Execute(string[] args);
    }
}
