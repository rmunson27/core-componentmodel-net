using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a <see langword="struct"/> property is set to the default value of its type when that
/// is not permitted by the current state of the object.
/// </summary>
[Obsolete("Will be removed in an upcoming version. Use `StructArgumentDefaultException`.")]
public class StructPropertySetDefaultException : PropertySetException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> class.
    /// </summary>
    public StructPropertySetDefaultException() : base("Property was set to default.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> class with a default error
    /// message including the name of the property that was set to an invalid default value.
    /// </summary>
    /// <param name="propName"></param>
    public StructPropertySetDefaultException(string propName)
        : base(FormatMessageWithPropertyName("Value cannot be default.", propName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> class with an error message
    /// including the supplied message and the name of the property that was set to an invalid default value.
    /// </summary>
    /// <param name="propName"></param>
    /// <param name="message"></param>
    public StructPropertySetDefaultException(string propName, string message)
        : base(FormatMessageWithPropertyName(message, propName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> class with the supplied error
    /// message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public StructPropertySetDefaultException(string message, Exception innerException)
        : base(message, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> class with the name of the
    /// property that was set to an invalid default value and the supplied error message and inner exception.
    /// </summary>
    /// <param name="propName"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public StructPropertySetDefaultException(string propName, string message, Exception innerException)
        : base(message, propName, innerException)
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructPropertySetDefaultException"/> from the serialization info
    /// and streaming context passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected StructPropertySetDefaultException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
