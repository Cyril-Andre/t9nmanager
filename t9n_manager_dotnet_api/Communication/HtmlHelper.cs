using System;
using System.Collections.Generic;
using System.IO;

namespace Communication
{
    public static class HtmlHelper
    {
        public static string loadHtmlTemplate(string path,string template, string locale="en", List<TemplateParameter> parameters=null)
        {
            var htmlString = File.ReadAllText($"{path}\\{template}.{locale}.html");
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var param in parameters)
                {
                    htmlString = htmlString.Replace("${#" + param.Key + "}", param.Value);
                }
            }
            return htmlString;
        }
    }
}
