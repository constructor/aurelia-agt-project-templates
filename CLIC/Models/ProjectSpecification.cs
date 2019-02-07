using System;
using System.Collections.Generic;
using System.Text;

namespace CLIC.Models
{
    public class ProjectSpecification
    {
        public string Name { get; set; }
        public string TemplateId { get; set; }
        public bool Install { get; set; }
        public bool Build { get; set; }
    }

}
