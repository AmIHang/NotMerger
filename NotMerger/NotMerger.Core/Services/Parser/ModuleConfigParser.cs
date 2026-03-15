using NotMerger.Core.Model;
using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotMerger.Core.Services.Parser
{
    public class ModuleConfigParser
    {
        public virtual ModuleConfigInfo Parse(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentException("XML must not be null or empty.", nameof(xml));

            XDocument document;
            try
            {
                document = XDocument.Parse(xml, LoadOptions.None);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse module config XML.", ex);
            }

            var result = new ModuleConfigInfo();

            var moduleElement = document.Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Module");

            if (moduleElement is null)
                return result;

            ParseModuleReferences(moduleElement, result);
            ParseGuiRepositories(moduleElement, result);
            ParseRulesRepositories(moduleElement, result);
            ParseWebUiRoutes(moduleElement, result);
            ParseBaseUriConverters(moduleElement, result);

            return result;
        }

        protected virtual void ParseModuleReferences(XElement moduleElement, ModuleConfigInfo result)
        {
            var references = moduleElement.Elements()
                .Where(e => e.Name.LocalName == "ModuleReference")
                .Select(e => (string?)e.Attribute("Source"))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!.Trim())
                .Where(v => v.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase));

            result.ModuleReferences.AddRange(references);
        }

        protected virtual void ParseGuiRepositories(XElement moduleElement, ModuleConfigInfo result)
        {
            var guiFiles = moduleElement.Descendants()
                .Where(e => e.Name.LocalName == "GuiRepository")
                .Elements()
                .Where(e => e.Name.LocalName == "XmlSource")
                .Select(e => (string?)e.Attribute("File"))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!.Trim());

            result.Gui.AddRange(guiFiles);
        }

        protected virtual void ParseRulesRepositories(XElement moduleElement, ModuleConfigInfo result)
        {
            var ruleFiles = moduleElement.Descendants()
                .Where(e => e.Name.LocalName == "RulesRepository")
                .Elements()
                .Where(e => e.Name.LocalName == "XmlSource")
                .Select(e => (string?)e.Attribute("File"))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!.Trim());

            result.Rules.AddRange(ruleFiles);
        }

        protected virtual void ParseWebUiRoutes(XElement moduleElement, ModuleConfigInfo result)
        {
            var webUiRoutes = moduleElement.Descendants()
                .Where(e => e.Name.LocalName == "WebUIRoute");

            foreach (var route in webUiRoutes)
            {
                var menuSources = route.Elements()
                    .Where(e => e.Name.LocalName == "MenuRepository")
                    .Select(e => (string?)e.Attribute("Source"))
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(v => v!.Trim());

                var workflowSources = route.Elements()
                    .Where(e => e.Name.LocalName == "WorkflowRepository")
                    .Select(e => (string?)e.Attribute("Source"))
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(v => v!.Trim());

                result.Menus.AddRange(menuSources);
                result.Workflows.AddRange(workflowSources);
            }
        }

        protected virtual void ParseBaseUriConverters(XElement moduleElement, ModuleConfigInfo result)
        {
            var converters = moduleElement.Elements()
                .Where(e => e.Name.LocalName == "BaseUriConverter")
                .Select(e => new BaseUriConverterInfo
                {
                    Moniker = (string?)e.Attribute("Moniker") ?? "",
                    BaseUri = (string?)e.Attribute("BaseUri") ?? ""
                })
                .Where(c => !string.IsNullOrWhiteSpace(c.Moniker));

            result.BaseUriConverters.AddRange(converters);
        }
    }
}
