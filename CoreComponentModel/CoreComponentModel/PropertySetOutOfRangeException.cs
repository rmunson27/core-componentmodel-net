using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a property is set to a value outside the range of acceptable values for the current
/// state of the object.
/// </summary>
public class PropertySetOutOfRangeException : PropertySetException
{
    /// <summary>
    /// Gets the message for the exception.
    /// </summary>
    public override string Message
        => ActualValue is null
            ? base.Message
            : base.Message + Environment.NewLine + $"Actual value was {ActualValue}.";

    /// <summary>
    /// Gets the property set value that caused the exception.
    /// </summary>
    public virtual object? ActualValue { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class.
    /// </summary>
    public PropertySetOutOfRangeException() : base("Property was set to a value that was out of range.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the name of the
    /// property that was set to an invalid value.
    /// </summary>
    /// <param name="propName"></param>
    public PropertySetOutOfRangeException(string propName) : this("Value was out of range.", propName) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified error
    /// message and the name of the property that was set to an invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    public PropertySetOutOfRangeException(string message, string propName) : base(message, propName) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified error
    /// message, the invalid value that caused the exception, and the specified inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="actualValue"></param>
    /// <param name="innerException"></param>
    public PropertySetOutOfRangeException(string message, object? actualValue, Exception innerException)
        : base(message, innerException)
    {
        this.ActualValue = actualValue;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified error
    /// message, the invalid value that caused the exception, and the name of the property that was set to an
    /// invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="actualValue"></param>
    public PropertySetOutOfRangeException(string message, string propName, object? actualValue)
        : base(message, propName)
    {
        this.ActualValue = actualValue;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified
    /// error message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public PropertySetOutOfRangeException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified error
    /// message and inner exception and the name of the property that was set to an invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="innerException"></param>
    public PropertySetOutOfRangeException(string message, string propName, Exception innerException)
        : base(message, propName, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class with the specified error
    /// message, the invalid value that caused the exception, the specified inner exception, and the name of the
    /// property that was set to an invalid value.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="propName"></param>
    /// <param name="actualValue"></param>
    /// <param name="innerException"></param>
    public PropertySetOutOfRangeException(
        string message, string propName, object? actualValue, Exception innerException)
        : base(message, propName, innerException)
    {
        this.ActualValue = actualValue;
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="PropertySetOutOfRangeException"/> class from the serialization
    /// data passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected PropertySetOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ActualValue = info.GetValue(nameof(ActualValue), typeof(object));
    }

    /// <summary>
    /// Sets the <see cref="SerializationInfo"/> with the invalid property set value and additional
    /// exception information.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ActualValue), ActualValue);
    }
}
