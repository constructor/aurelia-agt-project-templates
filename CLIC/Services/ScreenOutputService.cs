using System;
using System.Collections.Generic;
using System.Text;

namespace CLIC.Services
{
    public class ScreenOutputService
    {
        public const string Padding = "     ";

        public void WritePaddedLine(string text)
        {
            Console.WriteLine();
            Console.WriteLine($"{Padding}{text}");
            Console.WriteLine();
        }
    }
}
