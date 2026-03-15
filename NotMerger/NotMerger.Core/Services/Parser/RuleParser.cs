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
    public class RuleParser
    {
        public virtual List<RuleEntity> Parse(string xaml)
        {
            if (string.IsNullOrWhiteSpace(xaml))
                return new List<RuleEntity>();

            var doc = XDocument.Parse(xaml, LoadOptions.PreserveWhitespace);

            return doc
                .Descendants()
                .Where(x => x.Name.LocalName == "entity")
                .Select(entity => new RuleEntity
                {
                    Name = (string?)entity.Attribute("name"),
                    Groups = entity
                        .Elements()
                        .Where(child => child.Name.LocalName == "group")
                        .Select(group => new RuleGroup
                        {
                            Name = (string?)group.Attribute("name"),
                            Filter = (string?)group.Attribute("filter"),
                            Content = group.ToString()
                        })
                        .Where(x => !string.IsNullOrEmpty(x.Name))
                        .ToList()
                })
                .Where(entity => entity.Groups.Count > 0)
                .ToList();
        }
    }
}
