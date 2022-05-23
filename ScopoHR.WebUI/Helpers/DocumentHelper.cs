using ScopoHR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ScopoHR.WebUI.Helpers
{
    public static class DocumentHelper
    {
        public static Dictionary<DocumentCategory, string> GetLocations()
        {
            Dictionary<DocumentCategory, string> res = new Dictionary<DocumentCategory, string>();

            foreach (var c in Enum.GetValues(typeof(DocumentCategory)))
            {
                res.Add((DocumentCategory)c, ConfigurationManager.AppSettings.Get(((DocumentCategory)c).ToString()));
            }
            return res;
        }
    }
}