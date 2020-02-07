using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace CommonWebApi.Models.Binder
{
    public static class FilterModelBinder
    {
        public static IEnumerable<Filter> BindModel(string s)
        {
            if(!String.IsNullOrEmpty(s))
            {
                if (s.IndexOf(",", System.StringComparison.Ordinal) > -1)
                {
                    string[] stringArray = s.Split(new[] { "," }, StringSplitOptions.None);
                    IEnumerable<Filter> filters = stringArray.Where(str => !String.IsNullOrEmpty(str)).Select(str =>
                        new Filter()
                        {
                            FilterName = str.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(0),
                            FilterOperator = str.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(1),
                            FilterValue = str.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(2)
                        });
                    
                    return filters;
                }
                else if(s.IndexOf("'", System.StringComparison.Ordinal) > -1)
                {
                    Filter filter = new Filter();
                    filter.FilterName = s.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(0);
                    filter.FilterOperator = s.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(1);
                    filter.FilterValue = s.Trim("'".ToCharArray()).Split("'".ToCharArray()).ElementAtOrDefault(2);

                    return new List<Filter>() { filter };
                } else
                {
                    return new List<Filter>();
                }
            }
            return new List<Filter>();
        }
    }

    public class Filter
    {
        public string FilterName { get; set; }
        public string FilterOperator { get; set; }
        public string FilterValue { get; set; }
    }
}