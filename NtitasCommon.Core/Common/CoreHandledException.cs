using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Common
{
    /// <summary>
    /// CoreHandledException class should be used to wrap all handled exceptions (handled in the Core project) before the exceptions are logged.
    /// </summary>
    /// <remarks>
    /// This allows us to provide more info regarding the handled exceptions and should help with the debug process.
    /// </remarks>
    [Serializable()]
    public class CoreHandledException : Exception, ISerializable
    {
        /// <summary>
        /// Parameter-less constructor. Exception base constructor is invoked
        /// </summary>
        public CoreHandledException() : base() { }

        /// <summary>
        /// Constructor accepting a message. Exception base constructor is invoked
        /// </summary>
        /// <param name="message"> exception message </param>
        public CoreHandledException(string message) : base(message) { }

        /// <summary>
        /// Constructor accepting a message and inner exception. Exception base constructor is invoked
        /// </summary>
        /// <param name="message"> exception message </param>
        /// <param name="inner"> inner exception </param>
        public CoreHandledException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Serialization constructor. Exception base constructor is invoked
        /// </summary>
        /// <param name="info"> serialization info </param>
        /// <param name="context"> context </param>
        public CoreHandledException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
