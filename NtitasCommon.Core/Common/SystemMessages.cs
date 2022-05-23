using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NtitasCommon.Core.Common
{
    /// <summary>
    /// System Messages class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemMessages : ActionFilterAttribute
    {
        /// <summary>
        /// Http Items cache key
        /// </summary>
        private static readonly String ItemKEY = "HTTPITEMS_SystemMessagesKey";

        /// <summary>
        /// Temp data cache key 
        /// </summary>
        private static readonly String TempDataKEY = "TEMPDATA_SystemMessagesKey";


        /// <summary>
        /// Mock list - needed for unit testing
        /// </summary>
        private static List<SystemMessage> MockList { get; set; }

        #region ActionFilterAttribute overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            // read filterContext.Controller.TempData
            if (filterContext.Controller.TempData.ContainsKey(TempDataKEY))
            {
                List<SystemMessage> list = (List<SystemMessage>)filterContext.Controller.TempData[TempDataKEY];
                foreach (var item in list)
                    item.IsRedirectActive = false;
                Clear();
                Add(list);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            // write to filterContext.Controller.TempData
            if (filterContext.Result is System.Web.Mvc.RedirectResult || filterContext.Result is System.Web.Mvc.RedirectToRouteResult || filterContext.Result is System.Web.Mvc.JsonResult)
                filterContext.Controller.TempData[TempDataKEY] = Get().Where(x => x.IsRedirectMsg && x.IsRedirectActive).ToList();
        }

        #endregion

        /// <summary>
        /// Clear the SystemMessages internal collections
        /// </summary>
        public static void Clear()
        {
            if (MockList != null)
                MockList.Clear();

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(ItemKEY);
                HttpContext.Current.Items.Remove(TempDataKEY);
            }
        }


        /// <summary>
        /// Writes the list to cache
        /// </summary>
        /// <param name="list"> list to cache </param>
        private static void Set(List<SystemMessage> list)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[ItemKEY] = list;
            else
                MockList = list;
        }

        /// <summary>
        /// Returns all system messages 
        /// </summary>
        /// <returns> list of all registered system messages </returns>
        public static List<SystemMessage> Get()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(ItemKEY))
                {
                    return (List<SystemMessage>)HttpContext.Current.Items[ItemKEY];
                }
                else
                {
                    List<SystemMessage> list = new List<SystemMessage>();
                    Set(list);
                    return list;
                }
            }
            else
            {
                if (MockList == null)
                    MockList = new List<SystemMessage>();
                return MockList;
            }
        }


        /// <summary>
        /// Adds the system message to a list stored in HttpContext.Items
        /// </summary>
        /// <param name="text"> message </param>
        /// <param name="isError"> is this an error message </param>
        /// <param name="isRedirectMessage"> set to true if the message should be displayed after a redirect </param>
        public static void Add(string text, bool isError = false, bool isRedirectMessage = false)
        {
            List<SystemMessage> list = Get();
            list.Add(new SystemMessage(text, isError, isRedirectMessage));
            Set(list);
        }


        /// <summary>
        /// Adds the system message to a list stored in HttpContext.Items
        /// </summary>
        /// <param name="text"> message </param>
        /// <param name="msgType"> message type </param>
        /// <param name="isRedirectMessage"> set to true if the message should be displayed after a redirect </param>
        public static void Add(string text, SystemMessageType msgType, bool isRedirectMessage = false)
        {
            List<SystemMessage> list = Get();
            list.Add(new SystemMessage(text, msgType, isRedirectMessage));
            Set(list);
        }


        /// <summary>
        /// Adds the system message to a list stored in HttpContext.Items 
        /// </summary>
        /// <param name="systemMessage"> message to add </param>
        public static void Add(SystemMessage systemMessage)
        {
            List<SystemMessage> list = Get();
            list.Add(systemMessage);
            Set(list);
        }


        /// <summary>
        /// Adds a list of System Message to the list stored in HttpContext.Items
        /// </summary>
        /// <param name="msgs"> list of messages to add </param>
        public static void Add(List<SystemMessage> msgs)
        {
            List<SystemMessage> list = Get();
            list.AddRange(msgs);
            Set(list);
        }


        /// <summary>
        /// Checks if there are any errors in the list and returns the result
        /// </summary>
        /// <returns></returns>
        public static Boolean HasErrors()
        {
            return Get().Exists(x => x.IsError);
        }


        /// <summary>
        /// Checks if there are no errors in the list and returns the result
        /// </summary>
        /// <returns></returns>
        public static Boolean NoErrors()
        {
            return !HasErrors();
        }


        /// <summary>
        /// Returns only the errors in the system messages
        /// </summary>
        /// <returns></returns>
        public static List<SystemMessage> GetErrors()
        {
            return Get().Where(x => x.IsError).ToList();
        }


        /// <summary>
        /// Returns only the success messages in the system messages
        /// </summary>
        /// <returns></returns>
        public static List<SystemMessage> GetNonErrors()
        {
            return Get().Where(x => x.IsError == false).ToList();
        }


        /// <summary>
        /// Joins all error messages and returns them (as HTML)
        /// </summary>
        /// <returns></returns>
        public static string GetErrorString()
        {
            if (HasErrors())
                return string.Join("<br />", Get().Select(x => x.Text).ToArray());
            else
                return "";
        }


        /// <summary>
        /// Checks to see if the specified message is present in the collection
        /// </summary>
        /// <param name="ErrorMessage"> error message to check </param>
        /// <returns> true if the error message is found, false otherwise </returns>
        public static bool Contains(String ErrorMessage)
        {
            return Get().Any(x => x.Text == ErrorMessage);
        }


        /// <summary>
        /// Sets MockList to null
        /// </summary>
        public static void Dispose()
        {
            MockList = null;
        }
    }
}
