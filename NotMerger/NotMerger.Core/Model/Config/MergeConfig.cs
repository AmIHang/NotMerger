using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Config
{
    public class MergeConfig
    {
        public ModuleGroup CurrentVersion { get; set; }
        public ModuleGroup NextVersion {  get; set; }
        public ModuleGroup Project {  get; set; }
    }
}
