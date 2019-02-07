using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CLICServices.Models;

namespace CLICServices.TemplateFileMappings
{
    public class TemplateFileMapper
    {
        TemplateFileMappingsModel model;
        Dictionary<string, string> mappings;
        string templateProjectName;

        public TemplateFileMapper(TemplateFileMappingsModel model)
        {
            this.model = model;
            mappings = GetTemplateMappings(model.TemplateId);
        }

        public string MapSourceFile(string sourceFile)
        {
            if (mappings.ContainsKey(sourceFile))
                return mappings[sourceFile];

            if (sourceFile.Contains($"\\{templateProjectName}\\"))
                sourceFile = sourceFile.Replace($"\\{templateProjectName}\\", $"\\{model.ProjectName}\\");

            return sourceFile;
        }

        public Dictionary<string, string> GetTemplateMappings(int templateId)
        {
            switch (templateId)
            {
                case 1:
                    return GetTemplate1Mappings1();
                case 2:
                    return GetTemplate1Mappings2(); // or GetTemplateVSMappings(2); // less specific shared mappings for common template types
                case 3:
                    return GetTemplateVSMappings(3); // can use same mappings as is same template type 
            }

            return null;
        }

        /// <summary>
        /// 'dotnet new mvc' template file mappings
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetTemplate1Mappings1()
        {
            var templateId = 1;
            templateProjectName = "1";

            var d = new Dictionary<string, string>();

            var from = $"\\content\\{templateId}\\source\\Site.csproj";
            d.Add(from, ProcessFileList(from, model.ProjectName));

            return d;
        }

        /// <summary>
        /// Visual Studio 2017 MVC template file mappings
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetTemplate1Mappings2()
        {
            var templateId = 2;
            templateProjectName = "Site";

            var d = new Dictionary<string, string>();

            var from = $"\\content\\{templateId}\\source\\Site.sln";
            d.Add(from, ProcessFileList(from, model.ProjectName));

            from = $"\\content\\{templateId}\\source\\Site\\Site.csproj";
            d.Add(from, ProcessFileList(from, model.ProjectName));

            return d;
        }

        /// <summary>
        /// Visual Studio 2017 MVC template file mappings
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetTemplateVSMappings(int templateId)
        {
            templateProjectName = "Site";

            var d = new Dictionary<string, string>();

            var from = $"\\content\\{templateId}\\source\\Site.sln";
            d.Add(from, ProcessFileList(from, model.ProjectName));

            from = $"\\content\\{templateId}\\source\\Site\\Site.csproj";
            d.Add(from, ProcessFileList(from, model.ProjectName));

            return d;
        }

        string ProcessFileList(string from, string to)
        {
            string path = from;
            string file = Path.GetFileNameWithoutExtension(path);
            string result = path.Replace(file, to);
            return result;
        }

    }

}
