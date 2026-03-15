using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Config
{
    public class ModuleGroup
    {
        public string[] Tags { get; set; } = [];
        public string Name { get; set; }
        public ModuleGroup[] Groups { get; set; } = [];
        public Module[] Items { get; set; } = [];
    }
}
