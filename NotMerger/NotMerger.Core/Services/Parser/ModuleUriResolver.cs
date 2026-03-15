using NotMerger.Core.Model.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotMerger.Core.Services.Parser
{
    public static class ModuleUriResolver
    {
        public static string Resolve(string source, IEnumerable<BaseUriConverterInfo> converters)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;

            var idx = source.IndexOf("://", StringComparison.Ordinal);
            if (idx < 0)
                return source;

            var moniker = source.Substring(0, idx);
            var path = source.Substring(idx + 3);

            var converter = converters
                .FirstOrDefault(x => string.Equals(x.Moniker, moniker, StringComparison.OrdinalIgnoreCase));

            if (converter == null)
                return source;

            return $"./{converter.BaseUri}/{path}";
        }
    }
}
