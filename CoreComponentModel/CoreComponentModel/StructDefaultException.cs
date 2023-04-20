using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a <see langword="struct"/> value is its invalid default value.
/// </summary>
/// <remarks>
/// This exception can be considered an analog of the <see cref="NullReferenceException"/> for defaultable
/// <see langword="struct"/> values.
/// </remarks>
public class StructDefaultException : Exception
{
    /// <summary>
    /// Constructs a new instance of the <see cref="StructDefaultException"/> class with a default error message.
    /// </summary>
    public StructDefaultException() : base("Value was default.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructDefaultException"/> class with the specified error message.
    /// </summary>
    /// <param name="message"></param>
    public StructDefaultException(string? message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructDefaultException"/> class with the specified error message
    /// and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public StructDefaultException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructDefaultException"/> class from the serialization data passed
    /// in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected StructDefaultException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
