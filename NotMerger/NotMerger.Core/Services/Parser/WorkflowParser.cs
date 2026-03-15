using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotMerger.Core.Services.Parser
{
    public class WorkflowParser
    {
        public virtual List<Workflow> Parse(string xaml)
        {
            var doc = XDocument.Parse(xaml);

            return doc
                .Descendants()
                .Where(x => x.Name.LocalName == "Workflow")
                .Select(w => new Workflow
                {
                    Entity = (string?)w.Attribute("Entity"),
                    Name = (string?)w.Attribute("Name"),
                    Type = (string?)w.Attribute("Type"),
                    Filter = (string?)w.Attribute("Filter"),

                    // kompletter Workflow als String für Diff
                    Content = w.ToString()
                })
                .ToList();
        }
    }
}
