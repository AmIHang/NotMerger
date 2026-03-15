using NotMerger.Core.Model.Config;
using NotMerger.Core.Model.Parser;
using NotMerger.Core.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NotMerger.Core.Model
{
    public class ModuleInfo
    {
        public string Name { get; protected set; }
        protected readonly string _configPath;

        private HashSet<string> _tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        protected List<string> _guiFiles = [];
        protected List<string> _menuFiles = [];
        protected List<string> _workflowFiles = [];
        protected List<string> _ruleFiles = [];
      
        protected List<BaseUriConverterInfo> _baseUriConverters = [];
        protected Dictionary<string, GuiDefinitionBase>? _guiItems = null;
        protected Dictionary<string, MenuDefinition>? _menus = null;
        protected Dictionary<string, Workflow>? _workflows = null;
        protected Dictionary<string, RuleGroup>? _rules = null;

        public readonly ParserServiceInfo _parserInfo;

        public Dictionary<string, GuiDefinitionBase> GuiItems
            => new(_guiItems ??= new Dictionary<string, GuiDefinitionBase>(LoadGuiItems()));

        public Dictionary<string, MenuDefinition> Menus
            => new(_menus ??= new Dictionary<string, MenuDefinition>(LoadMenus()));

        public Dictionary<string, Workflow> Workflows
            => new(_workflows ??= new Dictionary<string, Workflow>(LoadWorkflows()));

        public Dictionary<string, RuleGroup> Rules
            => new(_rules ??= new Dictionary<string, RuleGroup>(LoadRules()));

        public ModuleInfo(string name, string configPath, ParserServiceInfo parserInfo)
        {
            if(string.IsNullOrEmpty(name)) 
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrEmpty(configPath))
                throw new ArgumentNullException(nameof(configPath));
            ArgumentNullException.ThrowIfNull(parserInfo, nameof(parserInfo));

            Name = name;
            _configPath = configPath;
            _parserInfo = parserInfo;

            Load(parserInfo.ConfigParser);
        }

        public void AddTags(string tag)
            => _tags.Add(tag);

        public virtual void Load(ModuleConfigParser parser)
            => LoadCore(parser, _configPath);

        protected virtual void LoadCore(ModuleConfigParser parser, string configPath)
        {
            _baseUriConverters.Clear();
            _guiFiles.Clear();
            _menuFiles.Clear();
            _workflowFiles.Clear();
            _ruleFiles.Clear();

            var mc = parser.Parse(File.ReadAllText(configPath));
            foreach (var mr in mc.ModuleReferences)
                LoadCore(parser, Path.Combine(configPath, "../", mr));

            foreach (var moniker in mc.BaseUriConverters)
                _baseUriConverters.Add(moniker);

            var basePath = Path.Combine(configPath, "../");
            foreach (var gui in mc.Gui)
                _guiFiles.Add(Path.Combine(basePath, ModuleUriResolver.Resolve(gui, _baseUriConverters)));

            foreach(var menu in mc.Menus)
                _menuFiles.Add(Path.Combine(basePath, ModuleUriResolver.Resolve(menu, _baseUriConverters)));

            foreach(var wf in mc.Workflows)
                _workflowFiles.Add(Path.Combine(basePath, ModuleUriResolver.Resolve(wf, _baseUriConverters)));

            foreach (var rule in mc.Rules)
                _ruleFiles.Add(Path.Combine(basePath, ModuleUriResolver.Resolve(rule, _baseUriConverters)));
        }
   
        protected IEnumerable<KeyValuePair<string, GuiDefinitionBase>> LoadGuiItems()
        {
            foreach(var f in _guiFiles)
            {
                var ge = _parserInfo.GuiParser.Parse(File.ReadAllText(f));

                foreach (var p in ge.Packages)
                    foreach (var item in LoadGuiPackage(p, string.Empty))
                        yield return item;
                foreach (var e in ge.Entities)
                    foreach(var item in LoadGuiEntity(e, string.Empty))
                        yield return item;
            }
            
        }

        protected virtual IEnumerable<KeyValuePair<string, GuiDefinitionBase>> LoadGuiPackage(GuiPackage gp, string parentNs)
        {
            var ns = gp.Name;
            if (!string.IsNullOrEmpty(parentNs))
                ns = $"{parentNs}.{ns}";

            foreach (var p in gp.Packages)
                foreach (var item in LoadGuiPackage(p, ns))
                    yield return item;

            var ens = ns + ".";
            foreach (var ge in gp.Entities)
                foreach(var item in LoadGuiEntity(ge, ens))
                    yield return item;
        }

        protected virtual IEnumerable<KeyValuePair<string, GuiDefinitionBase>> LoadGuiEntity(GuiEntity ge, string ns)
        {
            var key = ns + ge.Name;

            if(ge.Style != null)
                yield return new KeyValuePair<string, GuiDefinitionBase>(key + "@Style", ge.Style);

            var textKey = key + "@Text#";
            foreach (var text in ge.Texts)
                yield return new KeyValuePair<string, GuiDefinitionBase>(textKey + text.Name + "_" + text.Lang, text);

            var featureKey = key + "@Feature#";
            foreach (var feature in ge.Features)
                yield return new KeyValuePair<string, GuiDefinitionBase>(featureKey+feature.Name, feature);

            var customFeatureKey = key +  "@CustomFeature#";
            foreach(var cf in ge.CustomFeatures)
                yield return new KeyValuePair<string, GuiDefinitionBase>(customFeatureKey+cf.Name, cf);

            var listKey = key + "@List#";
            foreach (var list in ge.ListDefinitions)
                yield return new KeyValuePair<string, GuiDefinitionBase>(listKey+list.Type, list);

            var lookupKey = key + "@Lookup#";
            foreach(var lookup in ge.Lookups)
                yield return new KeyValuePair<string, GuiDefinitionBase>(lookupKey+lookup.Name, lookup);

            var formKey = key + "@Form#";
            foreach(var form in ge.Forms)
                yield return new KeyValuePair<string, GuiDefinitionBase>(formKey+form.Name, form);
        }

        protected virtual IEnumerable<KeyValuePair<string, MenuDefinition>> LoadMenus()
        {
            foreach(var f in _menuFiles)
                foreach (var mi in _parserInfo.MenuParser.Parse(File.ReadAllText(f)))
                    yield return new KeyValuePair<string, MenuDefinition>($"{mi.Entity}@{mi.Name}", mi);
        }

        protected virtual IEnumerable<KeyValuePair<string, Workflow>> LoadWorkflows()
        {
            foreach (var f in _workflowFiles)
                foreach (var wi in _parserInfo.WorkflowParser.Parse(File.ReadAllText(f)))
                    yield return new KeyValuePair<string, Workflow>($"{wi.Entity}@{wi.Name}#{wi.Type}", wi);
        }

        protected virtual IEnumerable<KeyValuePair<string, RuleGroup>> LoadRules()
        {
            foreach(var f in _ruleFiles)
                foreach(var re in _parserInfo.RuleParser.Parse(File.ReadAllText(f)))
                    foreach (var rg in re.Groups)
                        yield return new KeyValuePair<string, RuleGroup>($"{re.Name}@{rg.Name}", rg);
        }
    }
}
