using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class RuleGroup
    {
        public string? Name { get; set; }
        public string? Filter { get; set; }

        // kompletter Group-XML-Content für Diff
        public string Content { get; set; } = "";
    }
}
