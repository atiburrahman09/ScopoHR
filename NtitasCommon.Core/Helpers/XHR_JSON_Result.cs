using NtitasCommon.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Custom JSON result class that has an implicit cast to System.Web.Mvc.JsonResult
    /// </summary>
    /// <remarks>
    /// Use this class return JSON data along with system messages that will be automatically displayed to the user.
    /// Has implicit cast to System.Web.Mvc.JsonResult, so an instance of this class can be returned from a 
    /// controller action.
    /// </remarks>
    /// <seealso cref="XHR_JSON_Redirect"/>
    public class XHR_JSON_Result
    {
        /// <summary>
        /// Data to pass 
        /// </summary>
        Object data { get; set; }

        /// <summary>
        /// System messages to be automatically displayed using toastr
        /// </summary>
        List<SystemMessage> messages { get; set; }


        /// <summary>
        /// Constructor - no parameters
        /// </summary>
        /// <remarks>
        /// This constructor populates the messages list by calling SystemMessages.Get() - that means
        /// it will send all SystemMessages set during the execution of this request.
        /// </remarks>
        public XHR_JSON_Result()
        {
            data = new { };
            messages = SystemMessages.Get();
        }


        /// <summary>
        /// Constructor - sets the JSON <paramref name="data_"/>.
        /// </summary>
        /// <remarks>
        /// This constructor populates the messages list by calling SystemMessages.Get() - that means
        /// it will send all SystemMessages set during the execution of this request.
        /// </remarks>
        /// <param name="data_"> JSON data </param>
        public XHR_JSON_Result(Object data_)
        {
            data = data_;
            messages = SystemMessages.Get();
        }


        /// <summary>
        /// Constructor - sets the JSON <paramref name="data_"/> and <paramref name="messages_"/> 
        /// </summary>
        /// <param name="data_"> JSON data </param>
        /// <param name="messages_"> system messages to be displayed </param>
        public XHR_JSON_Result(Object data_, List<SystemMessage> messages_)
        {
            data = data_;
            messages = messages_;
        }


        /// <summary>
        /// Converts this XHR_JSON_Result instance to a System.Web.Mvc.JsonResult instance using 
        /// an implicit cast that is defined for XHR_JSON_Result
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
        public static implicit operator System.Web.Mvc.JsonResult(XHR_JSON_Result r)
        {
            return new System.Web.Mvc.JsonResult
            {
                Data = new { data = r.data, messages = r.messages },
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }
    }
}
