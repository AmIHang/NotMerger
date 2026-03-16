using NotMerger.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model
{
    public class MergeContext
    {
        public ModuleSnapshot? CurrentVersion { get; set; }
        public ModuleSnapshot? NextVersion { get; set; }
        public ModuleSnapshot? Project { get; set; }
    }
}
