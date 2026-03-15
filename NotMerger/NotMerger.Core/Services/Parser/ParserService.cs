using NotMerger.Core.Model;
using NotMerger.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Services.Parser
{
    public class ParserService
    {
        protected readonly MergeConfig _config;
        protected readonly ParserServiceInfo _parserInfo;
        public ParserService(MergeConfig config)
            : this(config, new ParserServiceInfo()) { }
        public ParserService(MergeConfig config, ParserServiceInfo parserInfo)
        {
            _config = config;
            _parserInfo = parserInfo;
        }

        public virtual MergeContext LoadMergeContext()
        {
            var mergeContext = new MergeContext();
            mergeContext.CurrentVersion = LoadSnapshots(_config.CurrentVersion);
            mergeContext.NextVersion = LoadSnapshots(_config.NextVersion);
            mergeContext.Project = LoadSnapshots(_config.Project);
            return mergeContext;
        }

        protected virtual ModuleSnapshot LoadSnapshots(ModuleGroup config)
        {
            var snapshot = new ModuleSnapshot();
            foreach(var g in config.Groups)
                foreach(var i in LoadModuleGroup(g))
                    snapshot.Modules.Add(i);

            foreach (var i in config.Items)
                snapshot.Modules.Add(LoadModule(i));

            return snapshot;
        }

        protected virtual IEnumerable<ModuleInfo> LoadModuleGroup(ModuleGroup config)
        {
            foreach(var g in config.Groups)
                foreach(var item in LoadModuleGroup(g))
                {
                    foreach(var t in config.Tags)
                        item.AddTags(t);
                    yield return item;
                }
            
            foreach(var i in config.Items)
            {
                var item = LoadModule(i);
                foreach(var t in config.Tags)
                    item.AddTags(t);
                yield return item;
            }
        }

        protected virtual ModuleInfo LoadModule(Module config)
        {
            var mi = new ModuleInfo(config.Name, config.Path, _parserInfo);
            foreach(var tag in config.Tags)
                mi.AddTags(tag);

            return mi;
        }
    }
}
