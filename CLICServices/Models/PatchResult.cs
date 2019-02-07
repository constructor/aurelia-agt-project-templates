using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLICServices.Models
{
    public class PatchResult
    {
        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }
        public PatchResultType PatchResultType { get; set; }
        public List<Patch> PatchList { get; set; }
    }

    public enum PatchResultType
    {
        CopyFile,
        Patch,
        Identical
    }
}
