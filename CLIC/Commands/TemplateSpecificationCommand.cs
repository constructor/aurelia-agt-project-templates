using CLIC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLIC.Commands
{
    public class TemplateSpecificationCommand : ICLICommand
    {
        public event EventHandler OnQuit;

        public void Execute(string[] args)
        {
            var ts = new TemplateSpecification();

            foreach (var q in ts.Questions)
            {
                string answer = q.AskQuestion();

                if (IsQuit(answer))
                    OnQuit.Invoke(this, null);
            }

            ts.ProcessChoices();

            Console.WriteLine();

            var json = JsonConvert.SerializeObject(ts, Formatting.Indented);

            Console.WriteLine();
            Console.WriteLine("Serialised Result (that is passed to template creation service):");
            Console.WriteLine();
            Console.WriteLine(json);
        }

        private bool IsQuit(string answer)
        {
            var a = answer.Trim().ToUpper();
            return a == "Q" || a == "QUIT";
        }
    }
}
