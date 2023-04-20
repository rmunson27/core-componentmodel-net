using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a property is set to an invalid enum value.
/// </summary>
[Obsolete("Will be removed in an upcoming version. Use `InvalidEnumArgumentException`.")]
public class InvalidEnumPropertySetException : PropertySetException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class.
    /// </summary>
    public InvalidEnumPropertySetException() : base("Property was set to invalid enum value.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class with the specified
    /// error message.
    /// </summary>
    /// <param name="message"></param>
    public InvalidEnumPropertySetException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class with the specified
    /// error message and the name of the property that caused the exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    public InvalidEnumPropertySetException(string message, string propName) : base(message, propName) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class with the specified error
    /// message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public InvalidEnumPropertySetException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class with the specified error
    /// message and inner exception and name of the property that caused the exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="innerException"></param>
    public InvalidEnumPropertySetException(string message, string propName, Exception innerException)
        : base(message, propName, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="InvalidEnumPropertySetException"/> class from the serialization
    /// data passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected InvalidEnumPropertySetException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
