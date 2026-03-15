using NotMerger.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model
{
    public class ModuleSnapshot
    {
        public List<ModuleInfo> Modules { get; protected set; } = [];

    }
}
