using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLICServices.Utils
{
    public static class CLICFileUtils
    {
        public static string GetTemplatePath(string rootPath, string templateId)
        {
            return Path.Combine(rootPath, $"content\\{templateId}\\");
        }

        public static string GetTemplatePath(string rootPath, string templateId, TemplatePathType templatePathType)
        {
            if (templatePathType == TemplatePathType.Source)
                return GetTemplateSourcePath(rootPath, templateId);

            if (templatePathType == TemplatePathType.Destination)
                return GetTemplateDestinationPath(rootPath, templateId);

            return GetTemplatePatchPath(rootPath, templateId);
        }

        public static string GetTemplateDestinationPath(string rootPath, string templateId)
        {
            return Path.Combine(rootPath, $"content\\{templateId}\\destination\\");
        }

        public static string GetTemplatePatchPath(string rootPath, string templateId)
        {
            return Path.Combine(rootPath, $"content\\{templateId}\\patch\\");
        }

        public static string GetTemplateSourcePath(string rootPath, string templateId)
        {
            return Path.Combine(rootPath, $"content\\{templateId}\\source\\");
        }

    }

    public enum TemplatePathType
    {
        Source,
        Destination,
        Patch
    }

}
