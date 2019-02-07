using System;
using System.Collections.Generic;
using System.Text;

namespace CLICServices.Models
{
    public class TemplateFileTransform
    {
        public TemplateFileTransform()
        {
            StringReplacements = new Dictionary<string, string>();
        }

        public string FileName { get; set; }
        public Dictionary<string,string> StringReplacements { get; set; }
    }
}
