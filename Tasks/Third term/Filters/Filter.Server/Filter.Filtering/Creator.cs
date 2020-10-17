using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Filter.Filtering
{
    public static class Creator
    {
        static Dictionary<string, IFilter> filters;

        public static void Initialize()
        {
            filters = new Dictionary<string, IFilter>();
            filters["Gray"] = new Gray();
            filters["SobelX"] = new SobelX();
        }

        public static IFilter Create(string name)
        {
            if (filters.ContainsKey(name))
                return filters[name];
            return null;
        }

        public static string AvailableFilters()
        {
            StringBuilder result = new StringBuilder();
            foreach (string filter in filters.Keys)
                result.Append(filter + " ");
            return result.ToString() ;
        }
    }
}
