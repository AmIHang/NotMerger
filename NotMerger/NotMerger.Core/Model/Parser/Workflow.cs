using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class Workflow
    {
        public string? Entity { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Filter { get; set; }

        public string Content { get; set; } = "";
    }
}
