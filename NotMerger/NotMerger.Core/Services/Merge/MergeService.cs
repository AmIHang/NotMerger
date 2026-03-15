using NotMerger.Core.Model;
using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Services.Merge
{
    public class MergeService
    {
        protected readonly MergeContext _context;

        protected readonly FlattenedModuleInfo _currentVersion;
        protected readonly FlattenedModuleInfo _nextNextVersion;
        protected readonly FlattenedModuleInfo _project;

        public MergeService(MergeContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            _context = context;

            _currentVersion = FlattenModuleInfo(_context.CurrentVersion);
            _nextNextVersion = FlattenModuleInfo(_context.NextVersion);
            _project = FlattenModuleInfo(_context.Project);
        }

        protected virtual FlattenedModuleInfo FlattenModuleInfo(ModuleSnapshot snapshot)
        {
            var fmi = new FlattenedModuleInfo();

            foreach (var module in snapshot.Modules)
            {
                foreach(var guiItem in module.GuiItems)
                    fmi.AddGuiItem(guiItem);
                foreach (var wf in module.Workflows)
                    fmi.AddWorkflowItem(wf);
                foreach(var menu in module.Menus)
                    fmi.AddMenuItem(menu);
                foreach(var rule in module.Rules)
                    fmi.AddRuleItem(rule);
            }

            return fmi;
        }

        public virtual MergeResult GetMergeResult()
        {
            var result = new MergeResult();
            
            result.GuiItems.AddRange(GetGuiMergeItems());
            result.WorkflowItems.AddRange(GetWorkflowMergeItems());
            result.MenuItems.AddRange(GetMenuMergeItems());
            result.RuleItems.AddRange(GetRuleMergeItems());

            return result;
        } 

        protected MergeResultItem? CreateMergeResultItemOrNull(string curent, string next, string  project)
        {
            if (string.IsNullOrEmpty(curent + next) || curent == next)
                return null;

            return new MergeResultItem()
            {
                CurrentVersion = curent,
                NextVersion = next,
                Project = project
            };
        }

        protected virtual IEnumerable<MergeResultItem> GetGuiMergeItems()
        {
            foreach(var gui in _project. GuiItems)
            {
                string currentContent = string.Empty;
                if (_currentVersion.GuiItems.TryGetValue(gui.Key, out var current))
                    currentContent = current.Content;

                var nextContent = string.Empty;
                if(_nextNextVersion.GuiItems.TryGetValue(gui.Key, out var next))
                    nextContent = next.Content;

                var mergeItem = CreateMergeResultItemOrNull(currentContent, nextContent, gui.Value.Content);
                if(mergeItem != null)
                  yield return mergeItem;
            }
        }

        protected virtual IEnumerable<MergeResultItem> GetWorkflowMergeItems()
        {
            foreach (var gui in _project.Workflows)
            {
                string currentContent = string.Empty;
                if (_currentVersion.Workflows.TryGetValue(gui.Key, out var current))
                    currentContent = current.Content;

                var nextContent = string.Empty;
                if (_nextNextVersion.Workflows.TryGetValue(gui.Key, out var next))
                    nextContent = next.Content;


                var mergeItem = CreateMergeResultItemOrNull(currentContent, nextContent, gui.Value.Content);
                if (mergeItem != null)
                    yield return mergeItem;
            }
        }

        protected virtual IEnumerable<MergeResultItem> GetMenuMergeItems()
        {
            foreach (var gui in _project.Menus)
            {
                string currentContent = string.Empty;
                if (_currentVersion.Menus.TryGetValue(gui.Key, out var current))
                    currentContent = current.Content;

                var nextContent = string.Empty;
                if (_nextNextVersion.Menus.TryGetValue(gui.Key, out var next))
                    nextContent = next.Content;


                var mergeItem = CreateMergeResultItemOrNull(currentContent, nextContent, gui.Value.Content);
                if (mergeItem != null)
                    yield return mergeItem;
            }
        }

        protected virtual IEnumerable<MergeResultItem> GetRuleMergeItems()
        {
            foreach (var gui in _project.Rules)
            {
                string currentContent = string.Empty;
                if (_currentVersion.Rules.TryGetValue(gui.Key, out var current))
                    currentContent = current.Content;

                var nextContent = string.Empty;
                if (_nextNextVersion.Rules.TryGetValue(gui.Key, out var next))
                    nextContent = next.Content;

                var mergeItem = CreateMergeResultItemOrNull(currentContent, nextContent, gui.Value.Content);
                if (mergeItem != null)
                    yield return mergeItem;
            }
        }
    }

    public class FlattenedModuleInfo
    {
        protected Dictionary<string, GuiDefinitionBase> _guiItems = [];
        protected Dictionary<string, MenuDefinition> _menus = [];
        protected Dictionary<string, Workflow> _workflows = [];
        protected Dictionary<string, RuleGroup> _rules = [];

        public Dictionary<string, GuiDefinitionBase> GuiItems
            => new(_guiItems);

        public Dictionary<string, MenuDefinition> Menus
            => new(_menus);

        public Dictionary<string, Workflow> Workflows
            => new(_workflows);

        public Dictionary<string, RuleGroup> Rules
            => new(_rules);

        public bool AddGuiItem(KeyValuePair<string, GuiDefinitionBase> item)
            => _guiItems.TryAdd(item.Key, item.Value);

        public bool AddWorkflowItem(KeyValuePair<string, Workflow> item)
            => _workflows.TryAdd(item.Key, item.Value);

        public bool AddMenuItem(KeyValuePair<string, MenuDefinition> item)
            => _menus.TryAdd(item.Key, item.Value);

        public bool AddRuleItem(KeyValuePair<string, RuleGroup> item)
            => _rules.TryAdd(item.Key, item.Value);
    }
}
