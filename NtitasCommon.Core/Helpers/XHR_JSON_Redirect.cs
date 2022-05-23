using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Custom JSON result class
    /// </summary>
    /// <remarks>
    /// Use this class when you want the browser window to automatically redirect/refresh after an ajax call.
    /// Has implicit cast to System.Web.Mvc.JsonResult, so an instance of this class can be returned from a 
    /// controller action.
    /// </remarks>
    /// <seealso cref="XHR_JSON_Result"/>
    public class XHR_JSON_Redirect
    {
        /// <summary>
        /// The URL to redirect the main browser window once this response is received by the ajax handlers (client-side)
        /// </summary>
        string RedirectURL { get; set; }


        /// <summary>
        /// Constructor - no parameters - will end up refreshing the current page
        /// </summary>
        public XHR_JSON_Redirect()
        {
            RedirectURL = "";
        }


        /// <summary>
        /// Constructor - sets the <paramref name="RedirectURL"/>
        /// </summary>
        /// <param name="RedirectURL"> The URL to redirect the main browser window once this response is received by the ajax handlers (client-side) </param>
        public XHR_JSON_Redirect(string RedirectURL)
        {
            this.RedirectURL = RedirectURL;
        }


        /// <summary>
        /// Converts this XHR_JSON_Redirect instance to a System.Web.Mvc.JsonResult instance using 
        /// an implicit cast that is defined for XHR_JSON_Redirect
        /// </summary>
        /// <param name="JsonBehaviour"> JSON get behaviour </param>
        /// <returns> System.Web.Mvc.JsonResult instance </returns>
        public System.Web.Mvc.JsonResult ToJsonResult(System.Web.Mvc.JsonRequestBehavior JsonBehaviour = System.Web.Mvc.JsonRequestBehavior.DenyGet)
        {
            // uses the implicit cast operator defined below
            return this;
        }


        /// <summary>
        /// Implicit cast to System.Web.Mvc.JsonResult
        /// </summary>
        /// <param name="r"> XHR_JSON_Result instance </param>
        /// <returns> JsonResult instance </returns>
        public static implicit operator System.Web.Mvc.JsonResult(XHR_JSON_Redirect r)
        {
            return new System.Web.Mvc.JsonResult
            {
                Data = new { data = new { r.RedirectURL }, code = 302 },
                ContentType = "application/json",
                ContentEncoding = null,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }
    }
}
