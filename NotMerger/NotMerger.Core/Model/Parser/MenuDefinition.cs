using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class  MenuDefinition
    {
        public string? Name { get; set; }
        public string? Entity { get; set; }

        // kompletter XML Inhalt für Diff
        public string Content { get; set; } = "";

        public string Key => $"{Entity}|{Name}";
    }
}
