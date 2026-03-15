using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class ModuleConfigInfo
    {
        public List<string> ModuleReferences { get; init; } = new();
        public List<string> Rules { get; init; } = new();
        public List<string> Gui { get; init; } = new();
        public List<string> Menus { get; init; } = new();
        public List<string> Workflows { get; init; } = new();
        public List<BaseUriConverterInfo> BaseUriConverters { get; init; } = new();

    }
}
