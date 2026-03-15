using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class RuleEntity
    {
        public string? Name { get; set; }
        public List<RuleGroup> Groups { get; set; } = new();
    }
}
