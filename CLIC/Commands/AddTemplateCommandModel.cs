using CLIC.Data;
using CLIC.Utils;
using CLICServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLIC.Commands
{
    public class AddTemplateCommandModel
    {
        PatchApplicationService patchApplicationService;

        public string ProjectName { get; set; } //is this different from SourceProjectName? Does this override SourceProjectName for the Aurelia project name?
        public int TemplateId { get; set; }

        public string TemplateRoot { get; set; }

        public bool InstallAfter { get; set; }
        public bool BuildAfter { get; set; }
        public bool RunAfter { get; set; }
        public bool WatchAfter { get; set; }

        public bool ShowHelp { get; set; }
        public bool ArgumentError { get; set; }
        public string ErrorMessage { get; set; }

        public string SourceProjectName { get; set; }

        public bool HasPostBuildAction { get { return BuildAfter || RunAfter || WatchAfter; } }

        public AddTemplateCommandModel()
        {

        }

        public bool IsValid()
        {
            var isValid = false;
            ArgumentError = false;

            var isValidDirectory = Directory.Exists(TemplateRoot);

            patchApplicationService = new PatchApplicationService(TemplateRoot);
            SourceProjectName = patchApplicationService.GetSourceProjectName(TemplateId, TemplateRoot);
            var hasProjectFile = !string.IsNullOrEmpty(SourceProjectName);

            var totalTemplates = TemplateData.AllTemplates.Count;
            var isValidTemplate = TemplateId > 0 && TemplateId <= totalTemplates;

            isValid = isValidDirectory && isValidTemplate && hasProjectFile && !ArgumentError;

            if (!isValidDirectory)
                ErrorMessage = "The directory of your project is required.";

            if (!hasProjectFile)
                ErrorMessage = $"A project could not be located at: {TemplateRoot}";

            if (!isValidTemplate)
                ErrorMessage = "A template id is required.";

            return isValid;
        }

    }

}
