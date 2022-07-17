using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An interface for structure types that can determine if they are the default value.
/// </summary>
/// <remarks>
/// This interface should be implemented by structures with defaults for which certain functionality may not work as
/// expected for other instances of the structure.
/// </remarks>
public interface IDefaultDeterminableStruct
{
    /// <summary>
    /// Gets whether or not this struct value is default (not initialized).
    /// </summary>
    public bool IsDefault { get; }
}
