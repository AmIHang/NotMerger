using NotMerger.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Services.Parser
{
    public class ParserServiceInfo
    {
        public readonly ModuleConfigParser ConfigParser;
        public readonly GuiParser GuiParser;
        public readonly MenuParser MenuParser;
        public readonly WorkflowParser WorkflowParser;
        public readonly RuleParser RuleParser;

        public ParserServiceInfo()
            : this(new ModuleConfigParser(), new GuiParser(), new MenuParser(), new WorkflowParser(), new RuleParser())
        {
        }

        public ParserServiceInfo(ModuleConfigParser configParser, GuiParser guiParser, MenuParser menuParser,  WorkflowParser workflowParser, RuleParser ruleParser)
        {
            ArgumentNullException.ThrowIfNull(configParser, nameof(configParser));
            ArgumentNullException.ThrowIfNull(guiParser, nameof(guiParser));
            ArgumentNullException.ThrowIfNull(menuParser, nameof(menuParser));
            ArgumentNullException.ThrowIfNull(workflowParser, nameof(workflowParser));
            ArgumentNullException.ThrowIfNull(ruleParser, nameof(ruleParser));

           ConfigParser = configParser;
            GuiParser = guiParser;
            MenuParser = menuParser;
            WorkflowParser = workflowParser;
            RuleParser = ruleParser;
        }
    }
}
