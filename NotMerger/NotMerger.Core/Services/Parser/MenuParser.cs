using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotMerger.Core.Services.Parser
{
    public class MenuParser
    {
        public virtual List<MenuDefinition> Parse(string xaml)
        {
            var doc = XDocument.Parse(xaml, LoadOptions.PreserveWhitespace);

            return doc
                .Descendants()
                .Where(x => x.Name.LocalName == "MenuDefinition")
                .Select(menu => new MenuDefinition
                {
                    Name = (string?)menu.Attribute("Name"),
                    Entity = (string?)menu.Attribute("Entity"),

                    // vollständiges Element für Diff speichern
                    Content = menu.ToString()
                })
                .ToList();
        }
    }


}
