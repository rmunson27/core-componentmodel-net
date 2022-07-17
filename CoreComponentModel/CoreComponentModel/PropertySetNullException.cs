using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a property is set to <see langword="null"/> when not permitted by the current state of
/// the object.
/// </summary>
public class PropertySetNullException : PropertySetException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class.
    /// </summary>
    public PropertySetNullException() : base("Property was set to null.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class with an error message
    /// including the name of the property that was set to <see langword="null"/>.
    /// </summary>
    /// <param name="propName"></param>
    public PropertySetNullException(string propName) : base("Value cannot be null.", propName) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class with the specified error message
    /// and the name of the property that was set to <see langword="null"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    public PropertySetNullException(string message, string propName) : base(message, propName) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class with the specified error
    /// message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public PropertySetNullException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class with the specified error
    /// message, inner exception, and the name of the property that was set to <see langword="null"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="innerException"></param>
    public PropertySetNullException(
        string message, string propName, Exception innerException)
        : base(message, propName, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetNullException"/> class from the serialization
    /// data passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected PropertySetNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
