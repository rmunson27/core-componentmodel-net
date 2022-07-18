using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rem.Core.ComponentModel;

/// <summary>
/// An interface for structure types that contain a default value that should be considered a degenerate case (the
/// analog of <see langword="null"/> for class types).
/// </summary>
/// <remarks>
/// This interface should be implemented by structures with defaults for which certain functionality may not work as
/// intended for other instances of the structure.
/// </remarks>
public interface IDefaultableStruct
{
    /// <summary>
    /// Gets whether or not the current value is the default.
    /// </summary>
    public bool IsDefault { get; }
}
