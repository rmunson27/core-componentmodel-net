using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// The exception thrown from a <see langword="struct"/> instance property or method called on the default when the
/// property or method is not supported for default instances.
/// </summary>
/// <remarks>
/// This exception can be considered the defaultable <see langword="struct"/> equivalent of
/// a <see cref="NullReferenceException"/>.
/// </remarks>
public class DefaultInstanceException : InvalidOperationException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="DefaultInstanceException"/> with a default error message.
    /// </summary>
    public DefaultInstanceException() : base("Instance cannot be default.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DefaultInstanceException"/> with the supplied error message.
    /// </summary>
    /// <param name="message"></param>
    public DefaultInstanceException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DefaultInstanceException"/> with the supplied error message and
    /// inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DefaultInstanceException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DefaultInstanceException"/> from the serialization data passed in
    /// (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected DefaultInstanceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
