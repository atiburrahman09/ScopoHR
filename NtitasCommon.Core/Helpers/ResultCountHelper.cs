using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Model needed for the ResultCountMessage partial
    /// </summary>
    public class ResultCountHelper
    {
        /// <summary>
        /// The number of items to show in the message
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Optional message that is shown when there is only 1 item
        /// </summary>
        /// <remarks>
        ///  Eg: "There was 1 item found."
        ///  If not specified, system uses CommonStrings.List_Count
        /// </remarks>
        public string SingleItemMessage { get; set; }

        /// <summary>
        /// Optional message that is shown when there are multiple items 
        /// </summary>
        /// <remarks>
        /// Eg: "There were {0} items found."  
        /// If not specified, system uses CommonStrings.List_CountSingle
        /// </remarks>
        public string MultiItemMessage { get; set; }
    }
}
