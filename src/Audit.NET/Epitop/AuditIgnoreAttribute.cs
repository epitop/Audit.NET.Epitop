using System;

namespace Audit.Core.Epitop
{
    /// <summary>
    ///     Attribute to ignore a property from being audited
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditIgnoreAttribute : Attribute
    {
    }
}