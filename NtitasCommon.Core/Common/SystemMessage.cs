using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Common
{
    /// <summary>
    /// SystemMessage class is used to pass messages from the Core layer to the Web layer
    /// </summary>
    public class SystemMessage
    {
        /// <summary>
        /// Text Property
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// IsError Property
        /// </summary>
        public Boolean IsError { get; set; }

        /// <summary>
        /// MessageType Property
        /// </summary>
        public SystemMessageType MessageType { get; set; }

        /// <summary>
        /// IsRedirectMsg Property
        /// </summary>
        public Boolean IsRedirectMsg { get; set; }

        /// <summary>
        /// IsRedirectActive Property
        /// </summary>
        public Boolean IsRedirectActive { get; set; }


        /// <summary>
        /// SystemMessage constructor
        /// </summary>
        /// <param name="text"> the text of the message </param>
        /// <param name="isError"> true if this is an error message </param>
        /// <param name="isRedirectMessage"> is the message supposed to be persisted over a redirect result </param>
        public SystemMessage(string text, bool isError = false, bool isRedirectMessage = false)
        {
            Text = text;
            IsError = isError;
            MessageType = isError ? SystemMessageType.Error : SystemMessageType.Success;
            IsRedirectMsg = isRedirectMessage;

            if (IsRedirectMsg)
                IsRedirectActive = true;
        }


        /// <summary>
        /// SystemMessage constructor
        /// </summary>
        /// <param name="text"> the text of the message </param>
        /// <param name="msgType"> the type of the message, see SystemMessageType enum </param>
        /// <param name="isRedirectMessage"> is the message supposed to be persisted over a redirect result </param>
        public SystemMessage(string text, SystemMessageType msgType, bool isRedirectMessage = false)
        {
            Text = text;
            IsError = msgType == SystemMessageType.Error;
            MessageType = msgType;
            IsRedirectMsg = isRedirectMessage;

            if (IsRedirectMsg)
                IsRedirectActive = true;
        }
    }



    /// <summary>
    /// Enum used by the SystemMessage class
    /// </summary>
    public enum SystemMessageType
    {
        /// <summary>
        /// Success Type
        /// </summary>
        Success = 1,

        /// <summary>
        /// Info Type
        /// </summary>
        Info = 2,

        /// <summary>
        /// Warning Type
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Error Type
        /// </summary>
        Error = 4
    }    
}
