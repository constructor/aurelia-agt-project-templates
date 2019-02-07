using CLICServices;
using CLICServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLIC.Utils
{
    public static class IOUtils
    {
        static PatchApplicationService patchApplicationService;

        static List<TemplateItem> AllTemplates;

        static IOUtils()
        {
            patchApplicationService = new PatchApplicationService(AssemblyDirectory);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static List<TemplateItem> LoadTemplateItemsData()
        {
            if (AllTemplates != null)
                return AllTemplates;

            AllTemplates = patchApplicationService.LoadTemplateItemsData();

            return AllTemplates;
        }

    }
}
