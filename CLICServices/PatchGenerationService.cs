using CLICServices.Models;
using CLICServices.Utils;
using DiffMatchPatch;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CLICServices
{
    public class PatchGenerationService
    {
        private readonly IHostingEnvironment env;
        string webRootPath, contentRootPath;
        //private Dictionary<string, string> result;
        private List<PatchResult> patchResults;

        public PatchGenerationService(IHostingEnvironment hostingEnvironment)
        {
            env = hostingEnvironment;
            webRootPath = env.WebRootPath;
            contentRootPath = env.ContentRootPath;
        }

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree
        public List<PatchResult> GeneratePatchForTemplate(int? templateId)
        {
            if (templateId == null)
                return null;

            patchResults = new List<PatchResult>();
            //var p = Path.Combine(webRootPath, $"content\\{templateId}\\destination");
            var p = CLICFileUtils.GetTemplatePath(webRootPath, templateId.ToString(), TemplatePathType.Destination);

            ProcessFolder(p);
            SavePatchJson(templateId);

            return patchResults;
        }

        public void ProcessFolder(string path)
        {
            var files = Directory.GetFiles(path);
            var dirs = Directory.GetDirectories(path);

            foreach (var f in files)
                ProcessFile(f);

            foreach (var d in dirs)
                ProcessFolder(d);
        }

        private List<Patch> ProcessFile(string f)
        {
            string destinationContent = "";
            string sourceContent = "";

            destinationContent = File.ReadAllText(f);

            //corresponding source path to destination file
            var sourcePath = f.Replace("\\destination\\", "\\source\\"); 

            if (!File.Exists(sourcePath))
            {
                patchResults.Add(new PatchResult { DestinationFile = MakeRelativePath(f), SourceFile = MakeRelativePath(sourcePath), PatchResultType = PatchResultType.CopyFile });
                return null;
            }

            //content of users original file
            sourceContent = File.ReadAllText(sourcePath);

            var dmp = new diff_match_patch();
            var diff = dmp.diff_main(sourceContent, destinationContent);
            dmp.diff_cleanupSemantic(diff);

            var patchResult = dmp.patch_make(diff);
            patchResults.Add(new PatchResult { DestinationFile = MakeRelativePath(f), SourceFile = MakeRelativePath(sourcePath), PatchList = patchResult, PatchResultType = patchResult.Count > 0 ? PatchResultType.Patch : PatchResultType.Identical });

            return patchResult;
        }

        private void GeneratePatch(PatchResult f)
        {
            var newPath = MakeAbsolutePath(f.DestinationFile.Replace("\\destination\\", "\\patchedSource\\"));

            var d = Path.GetDirectoryName(newPath);

            if (!Directory.Exists(d))
                Directory.CreateDirectory(d);

            if (f.PatchResultType == PatchResultType.CopyFile)
            {
                File.Copy(MakeAbsolutePath(f.DestinationFile), newPath, true);
                return;
            }

            var content = File.ReadAllText(MakeAbsolutePath(f.SourceFile));

            var dmp = new diff_match_patch();
            var r = dmp.patch_apply(f.PatchList, content);

            var newContent = r[0];

            File.WriteAllText(newPath, newContent.ToString());
        }

        private void SavePatchJson(int? templateId)
        {
            if (templateId == null)
                return;

            //var p = Path.Combine(webRootPath, $"content\\{templateId}\\patch\\");
            var p = CLICFileUtils.GetTemplatePath(webRootPath, templateId.ToString(), TemplatePathType.Patch);

            var json = JsonConvert.SerializeObject(patchResults);

            if (!Directory.Exists(Path.GetDirectoryName(p)))
                Directory.CreateDirectory(Path.GetDirectoryName(p));

            File.WriteAllText($"{p}\\patch.json", json);
        }

        public void Apply(int? templateId)
        {
            if (templateId == null)
                return;

            //var p = Path.Combine(webRootPath, $"content\\Template{templateId}\\patch\\patch.json");
            var p = CLICFileUtils.GetTemplatePath(webRootPath, templateId.ToString(), TemplatePathType.Patch) + "\\patch.json";
            var results = LoadPatchJson(p);
            ApplyPatch(results, templateId);
        }

        public void ApplyPatch(List<PatchResult> result, int? templateId)
        {
            if (templateId == null)
                return;

            //Copy over all original files first
            //var od = Path.Combine(webRootPath, $"content\\Template{templateId}\\source");
            var od = CLICFileUtils.GetTemplatePath(webRootPath, templateId.ToString(), TemplatePathType.Source);

            //var fd = Path.Combine(webRootPath, $"content\\Template{templateId}\\patchedSource");
            var fd = CLICFileUtils.GetTemplatePath(webRootPath, templateId.ToString()) + "\\patchedSource";

            DirectoryCopy(od, fd, true);

            foreach (var f in result)
            {
                GeneratePatch(f);
            }
        }

        private List<PatchResult> LoadPatchJson(string path)
        {
            var json = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<List<PatchResult>>(json);
            return result;
        }

        private string MakeRelativePath(string path)
        {
            return path.Replace(webRootPath, "");
        }

        private string MakeAbsolutePath(string path)
        {
            var p = webRootPath + path;
            return p;
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

