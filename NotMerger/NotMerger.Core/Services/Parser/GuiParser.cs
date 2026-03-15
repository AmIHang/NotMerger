using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotMerger.Core.Services.Parser
{
    public class GuiParser
    {
        public virtual GuiRepository Parse(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return new GuiRepository();

            var doc = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);

            var root = doc.Root;
            if (root == null || root.Name.LocalName != "GuiRepository")
                throw new InvalidOperationException("Root element 'GuiRepository' was not found.");

            return new GuiRepository
            {
                Packages = root.Elements()
                    .Where(x => x.Name.LocalName == "package")
                    .Select(ParsePackage)
                    .ToList(),

                Entities = root.Elements()
                    .Where(x => x.Name.LocalName == "entity")
                    .Select(ParseEntity)
                    .ToList()
            };
        }

        protected virtual GuiPackage ParsePackage(XElement packageElement)
        {
            return new GuiPackage
            {
                Name = (string?)packageElement.Attribute("name"),

                Packages = packageElement.Elements()
                    .Where(x => x.Name.LocalName == "package")
                    .Select(ParsePackage)
                    .ToList(),

                Entities = packageElement.Elements()
                    .Where(x => x.Name.LocalName == "entity")
                    .Select(ParseEntity)
                    .ToList()
            };
        }

        protected virtual GuiEntity ParseEntity(XElement entityElement)
        {
            var directChildren = entityElement.Elements().ToList();

            return new GuiEntity
            {
                Name = (string?)entityElement.Attribute("name"),

                Texts = directChildren
                    .Where(x => x.Name.LocalName == "text")
                    .Select(ParseText)
                    .ToList(),

                Features = directChildren
                    .Where(x => x.Name.LocalName == "feature")
                    .Select(ParseFeature)
                    .ToList(),

                CustomFeatures = directChildren
                    .Where(x => x.Name.LocalName == "customFeature")
                    .Select(ParseCustomFeature)
                    .ToList(),

                Forms = directChildren
                    .Where(x => x.Name.LocalName == "form")
                    .Select(ParseForm)
                    .ToList(),

                ListDefinitions = directChildren
                    .Where(x => x.Name.LocalName == "listDefinition")
                    .Select(ParseListDefinition)
                    .ToList(),

                Lookups = directChildren
                    .Where(x => x.Name.LocalName == "lookup")
                    .Select(ParseLookup)
                    .ToList(),

                Style = directChildren
                    .Where(x => x.Name.LocalName == "style")
                    .Select(ParseStyle)
                    .FirstOrDefault()
            };
        }

        protected virtual GuiTextDefinition ParseText(XElement element)
        {
            return new GuiTextDefinition
            {
                Name = (string?)element.Attribute("name"),
                Lang = (string?)element.Attribute("lang"),
                Value = element.Value,
                Content = element.ToString()
            };
        }

        protected virtual GuiFeatureDefinition ParseFeature(XElement element)
        {
            return new GuiFeatureDefinition
            {
                Name = (string?)element.Attribute("name"),
                DataType = (string?)element.Attribute("data-type"),
                Gui = (string?)element.Attribute("gui"),
                Content = element.ToString()
            };
        }

        protected virtual GuiCustomFeatureDefinition ParseCustomFeature(XElement element)
        {
            return new GuiCustomFeatureDefinition
            {
                Name = (string?)element.Attribute("name"),
                DataType = (string?)element.Attribute("data-type"),
                Gui = (string?)element.Attribute("gui"),
                Content = element.ToString()
            };
        }

        protected virtual GuiFormDefinition ParseForm(XElement element)
        {
            return new GuiFormDefinition
            {
                Name = (string?)element.Attribute("name"),
                Content = element.ToString()
            };
        }

        protected virtual GuiListDefinitionDefinition ParseListDefinition(XElement element)
        {
            return new GuiListDefinitionDefinition
            {
                Name = (string?)element.Attribute("name"),
                Type = (string?)element.Attribute("type"),
                Content = element.ToString()
            };
        }

        protected virtual GuiLookupDefinition ParseLookup(XElement element)
        {
            return new GuiLookupDefinition
            {
                Name = (string?)element.Attribute("name"),
                Content = element.ToString()
            };
        }

        protected virtual GuiStyleDefinition ParseStyle(XElement element)
        {
            return new GuiStyleDefinition
            {
                Name = (string?)element.Attribute("name"),
                Content = element.ToString()
            };
        }
    }
}
