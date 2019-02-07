using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLICServices.Models
{
    public class TemplateItem
    {
        public TemplateItem()
        {
            TemplateFileTransform = new List<TemplateFileTransform>();
        }

        [JsonProperty]
        public int TemplateId { get; set; }

        [JsonProperty]
        public string TemplateLabel { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string AureliaRoot { get; set; }

        public List<TemplateFileTransform> TemplateFileTransform { get; set; }
    }
}
