using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Model.Parser
{
    public class GuiEntity
    {
        public string? Name { get; set; }

        public List<GuiTextDefinition> Texts { get; set; } = new();
        public List<GuiFeatureDefinition> Features { get; set; } = new();
        public List<GuiCustomFeatureDefinition> CustomFeatures { get; set; } = new();
        public List<GuiFormDefinition> Forms { get; set; } = new();
        public List<GuiListDefinitionDefinition> ListDefinitions { get; set; } = new();
        public List<GuiLookupDefinition> Lookups { get; set; } = new();

        public GuiStyleDefinition? Style { get; set; }
    }
}
