using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a property is set to an invalid value.
/// </summary>
public class PropertySetException : InvalidOperationException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class.
    /// </summary>
    public PropertySetException() : base("Property was set to invalid value.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class with the specified error message.
    /// </summary>
    /// <param name="message"></param>
    public PropertySetException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class with the specified error message and
    /// the name of the property that was set to an invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    public PropertySetException(string message, string propName)
        : base(FormatMessageWithPropertyName(message, propName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class with the specified error message
    /// and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public PropertySetException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class with the specified error message,
    /// inner exception, and the name of the property that was set to an invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="innerException"></param>
    public PropertySetException(string message, string propName, Exception innerException)
        : base(FormatMessageWithPropertyName(message, propName), innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetException"/> class from the serialization
    /// data passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected PropertySetException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>
    /// Formats the specified error message with the name of the property that caused the error.
    /// </summary>
    /// <remarks>
    /// This function is used to generate error messages in the <see cref="PropertySetException"/> class, and may also
    /// be handy when extending that class.
    /// </remarks>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <returns></returns>
    protected static string FormatMessageWithPropertyName(string message, string propName)
        => $"{message} (Property '{propName}')";
}
