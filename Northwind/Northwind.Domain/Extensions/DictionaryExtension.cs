using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Data.Domain.Extensions
{
    public static class DictionaryExtension
    {
        public static string ToString(this IDictionary<string, object> source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
            {
                throw new ArgumentException("Parameter source can not be null.");
            }
            var pairs = source.Select(x => string.Format("{0}{1}{2}", x.Key, keyValueSeparator, JsonConvert.SerializeObject(x.Value)));

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
