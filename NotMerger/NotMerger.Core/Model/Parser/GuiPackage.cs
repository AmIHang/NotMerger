using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class GuiPackage
    {
        public string? Name { get; set; }

        public List<GuiPackage> Packages { get; set; } = new();
        public List<GuiEntity> Entities { get; set; } = new();
    }
}
