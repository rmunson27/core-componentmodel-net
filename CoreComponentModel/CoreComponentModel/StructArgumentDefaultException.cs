using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An exception thrown when a <see langword="struct"/> argument is the default value of its type when that is not
/// permitted by the method being called.
/// </summary>
/// <remarks>
/// This class extends <see cref="ArgumentNullException"/> because in the sense that an invalid default has zeroed bits
/// that are invalid, it is similar to the <see cref="null"/> case, which is a zeroed reference
/// or <see cref="Nullable{T}"/>. The hope is that this will make it easier to handle these similar exceptions, which
/// essentially represent the same case.
/// </remarks>
public class StructArgumentDefaultException : ArgumentNullException
{
    /// <summary>
    /// Constructs a new instancce of the <see cref="StructArgumentDefaultException"/> class.
    /// </summary>
    public StructArgumentDefaultException() : base(null, "Argument was default.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> class with a default error
    /// message including the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="paramName"></param>
    public StructArgumentDefaultException(string paramName)
        : base(null, FormatMessageWithParamName("Value cannot be default.", paramName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> class with an error message
    /// including the supplied message and the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="paramName"></param>
    /// <param name="message"></param>
    public StructArgumentDefaultException(string? paramName, string? message)
        : base(null, FormatMessageWithParamName(message, paramName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> class with the supplied error
    /// message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public StructArgumentDefaultException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> from the serialization info and
    /// streaming context passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected StructArgumentDefaultException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>
    /// Formats the supplied error message with the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    protected static string FormatMessageWithParamName(string? message, string? paramName)
    {
        message ??= "Value cannot be the default.";
        return paramName is null ? message : $"{message} (Parameter '{paramName}')";
    }
}
