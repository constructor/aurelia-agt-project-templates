using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using DiffMatchPatch;
using CLICServices.Models;
using CLICServices.Utils;
using CLICServices.TemplateFileMappings;

namespace CLICServices
{
    public class PatchApplicationService
    {
        private List<PatchResult> patchResults;

        public string WebRootPath { get; set; }
        public string ContentRootPath { get; set; }

        TemplateFileMapper tfm;
        string projectName;

        public PatchApplicationService(string basePath)
        {
            ContentRootPath = Path.Combine(basePath, "Resources\\Templates\\");
        }

        public void AddAureliaToProject(TemplateItem templateItem, string sourcePath)
        {
            var templateId = templateItem.TemplateId;

            var p = Path.Combine(ContentRootPath, $"Content\\{templateId}\\patch\\patch.json");
            var results = LoadPatchJson(p);

            //get the users project name from the IDE generated files
            var projectName = GetSourceProjectName(templateId, sourcePath);

            //generate mappings
            var mapModel = new TemplateFileMappingsModel();
            mapModel.ProjectName = projectName;
            mapModel.TemplateId = templateId;
            tfm = new TemplateFileMapper(mapModel);

            ApplyAureliaPatch(results, templateItem, sourcePath);
        }

        public void ApplyAureliaPatch(List<PatchResult> result, TemplateItem templateItem, string sourcePath)
        {
            if (result == null)
                return;

            foreach (var f in result)
                ProcessAureliaPatch(f, templateItem, sourcePath);

            ProcessTextTransforms(templateItem, sourcePath);
        }

        private void ProcessAureliaPatch(PatchResult f, TemplateItem templateItem, string sourcePath)
        {
            var templateId = templateItem.TemplateId;

            var dRoot = Path.Combine(ContentRootPath, $"Content\\{templateId}\\destination");

            var sRel = $"\\content\\{templateId}\\source\\";
            var dRel = $"\\content\\{templateId}\\destination\\";

            var fromPath = CLICFileUtils.GetTemplatePath(ContentRootPath, templateId.ToString(), TemplatePathType.Destination) + f.DestinationFile.Replace(dRel, "");

            var mappedSourceFile = tfm.MapSourceFile(f.SourceFile);
            var toPath = Path.Combine(sourcePath, mappedSourceFile.Replace(sRel, ""));//need to map the sourcepath file (which can be of a different name) to the destination (patched) file: Eg/ MyProject1.sln > Site.sln

            var d = Path.GetDirectoryName(toPath);
            if (!Directory.Exists(d))
                Directory.CreateDirectory(d);

            if (f.PatchResultType == PatchResultType.CopyFile && (!File.Exists(toPath)))
            {
                File.Copy(fromPath, toPath, true);
                Console.WriteLine($"Copying: {toPath}");
                return;
            }

            if (f.PatchResultType == PatchResultType.Identical)
                return;

            if (f.PatchList == null)
                return;

            var content = File.ReadAllText(toPath);

            var dmp = new diff_match_patch();
            var r = dmp.patch_apply(f.PatchList, content);

            var newContent = r[0];

            ////Apply any text transforms
            //foreach (var t in templateItem.TemplateFileTransform)
            //{
            //    var match = f.SourceFile.EndsWith(t.FileName);
            //    if (match)
            //    {
            //        foreach (var sr in t.StringReplacements) {
            //            var newValue = sr.Value.Replace("{ProjectName}", "MyProject");
            //            newContent = newContent.ToString().Replace(sr.Key, newValue);
            //        }
            //    }

            //    var match2 = t.FileName == toPath;
            //}

            File.WriteAllText(toPath, newContent.ToString());
            Console.WriteLine($"Patched: {toPath}");
        }

        void ProcessTextTransforms(TemplateItem templateItem, string sourcePath)
        {
            var projectName = GetSourceProjectName(templateItem.TemplateId, sourcePath);
            var aureliaRoot = templateItem.AureliaRoot.Replace("{ProjectName}", projectName); 

            foreach (var t in templateItem.TemplateFileTransform)
            {
                var filename = t.FileName.Replace("{ProjectName}", projectName);
                var file = Path.Combine(sourcePath, filename);
                var content = File.ReadAllText(file);

                foreach (var sr in t.StringReplacements)
                {
                    var key = sr.Key;
                    var val = sr.Value.Replace("{ProjectName}", projectName);
                    content = content.ToString().Replace(key, val);
                }

                File.WriteAllText(file, content.ToString());
            }
        }






        public void AddAureliaToProject(int templateId, string sourcePath)
        {
            var p = Path.Combine(ContentRootPath, $"Content\\{templateId}\\patch\\patch.json");
            var results = LoadPatchJson(p);

            //get the users project name from the IDE generated files
            var projectName = GetSourceProjectName(templateId, sourcePath);

            //generate mappings
            var mapModel = new TemplateFileMappingsModel();
            mapModel.ProjectName = projectName;
            mapModel.TemplateId = templateId;
            tfm = new TemplateFileMapper(mapModel);

            ApplyAureliaPatch(results, templateId, sourcePath);
        }

        private List<PatchResult> LoadPatchJson(string path)
        {
            var json = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<List<PatchResult>>(json);
            return result;
        }

        public void ApplyAureliaPatch(List<PatchResult> result, int templateId, string sourcePath)
        {
            if (result == null)
                return;

            foreach (var f in result)
                ProcessAureliaPatch(f, templateId, sourcePath);
        }

        private void ProcessAureliaPatch(PatchResult f, int templateId, string sourcePath)
        {
            var dRoot = Path.Combine(ContentRootPath, $"Content\\{templateId}\\destination");

            var sRel = $"\\content\\{templateId}\\source\\";
            var dRel = $"\\content\\{templateId}\\destination\\";

            var fromPath = CLICFileUtils.GetTemplatePath(ContentRootPath, templateId.ToString(), TemplatePathType.Destination) + f.DestinationFile.Replace(dRel, "");

            var mappedSourceFile = tfm.MapSourceFile(f.SourceFile);
            var toPath = Path.Combine(sourcePath, mappedSourceFile.Replace(sRel, ""));//need to map the sourcepath file (which can be of a different name) to the destination (patched) file: Eg/ MyProject1.sln > Site.sln

            var d = Path.GetDirectoryName(toPath);
            if (!Directory.Exists(d))
                Directory.CreateDirectory(d);

            if (f.PatchResultType == PatchResultType.CopyFile && (!File.Exists(toPath)))
            {
                File.Copy(fromPath, toPath, true);
                Console.WriteLine($"Copying: {toPath}");
                return;
            }

            if (f.PatchResultType == PatchResultType.Identical)
                return;

            if (f.PatchList == null)
                return;

            var content = File.ReadAllText(toPath);

            var dmp = new diff_match_patch();
            var r = dmp.patch_apply(f.PatchList, content);

            //Apply any text transforms

            var newContent = r[0];

            File.WriteAllText(toPath, newContent.ToString());
            Console.WriteLine($"Patched: {toPath}");
        }

        /// <summary>
        /// Get the name of the project from the filename
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public string GetSourceProjectName(int templateId, string root)
        {
            string sourceProjectName = string.Empty;

            if (templateId == 1)
                sourceProjectName = GetSourceProjectNameByExt(root, ".csproj");

            if (templateId == 2)
                sourceProjectName = GetSourceProjectNameByExt(root, ".sln");

            if (templateId == 3)
                sourceProjectName = GetSourceProjectNameByExt(root, ".sln");

            return sourceProjectName;
        }

        /// <summary>
        /// For template type 1 - 'dotnet new mvc' cli project 
        /// </summary>
        /// <returns></returns>
        private string GetSourceProjectNameByExt(string root, string ext)
        {
            var f = Directory.GetFiles(root).Select(x => new FileInfo(x))
                                                 .Where(x => x.Name.ToUpper().EndsWith(ext.ToUpper()))
                                                 .FirstOrDefault();
            return f?.Name.Replace(f.Extension, "");
        }

        public List<TemplateItem> LoadTemplateItemsData()
        {
            var l = new List<TemplateItem>();
            var tRoot = Path.Combine(ContentRootPath, $"Content");
            var tDirs = new DirectoryInfo(tRoot).GetDirectories();

            foreach (var d in tDirs) {
                var n = d.Name;
                var f = Path.Combine(d.FullName, "template.json");
                var json = File.ReadAllText(f);
                var ti = JsonConvert.DeserializeObject<TemplateItem>(json);
                l.Add(ti);
            }

            return l;
        }

    }

}

