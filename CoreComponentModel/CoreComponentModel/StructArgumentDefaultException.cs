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
public class StructArgumentDefaultException : ArgumentException
{
    /// <summary>
    /// Constructs a new instancce of the <see cref="StructArgumentDefaultException"/> class.
    /// </summary>
    public StructArgumentDefaultException() : base("Argument was default.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> class with a default error
    /// message including the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="paramName"></param>
    public StructArgumentDefaultException(string paramName)
        : base(FormatMessageWithParamName("Value cannot be default.", paramName))
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="StructArgumentDefaultException"/> class with an error message
    /// including the supplied message and the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="paramName"></param>
    /// <param name="message"></param>
    public StructArgumentDefaultException(string paramName, string message)
        : base(FormatMessageWithParamName(message, paramName))
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
    protected StructArgumentDefaultException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <summary>
    /// Formats the supplied error message with the name of the parameter that caused the exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    protected static string FormatMessageWithParamName(string message, string paramName)
        => $"{message} (Parameter '{paramName}')";
}

