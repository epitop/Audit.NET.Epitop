using System;
using System.Runtime.Serialization;

namespace Audit.Core
{
    /// <summary>
    /// Target object data.
    /// </summary>
    public class AuditTarget
    {
        /// <summary>
        /// The type of the object tracked
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// The value of the object tracked after the auditscope is saved
        /// </summary>
        public object EventObject { get; set; }
    }
}