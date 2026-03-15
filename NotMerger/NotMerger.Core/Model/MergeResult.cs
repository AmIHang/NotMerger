using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model
{
    public class MergeResult
    {
        public List<MergeResultItem> GuiItems { get; set; } = [];
        public List<MergeResultItem> MenuItems { get; set; } = [];
        public List<MergeResultItem> WorkflowItems { get; set; } = [];
        public List<MergeResultItem> RuleItems { get; set; } = [];

    }

    public class MergeResultItem
    {
        public string Key { get; set; } 
        public string CurrentVersion { get; set; }
        public string NextVersion { get; set; }
        public string Project { get; set; }
    }
}
