using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public abstract class GuiDefinitionBase
    {
        public string? Name { get; set; }
        public string Content { get; set; } = "";
    }
}
