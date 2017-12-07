#region --- License ---
/* Copyright (c) 2006, 2007 Stefanos Apostolopoulos
 * See license.txt for license info
 */
#endregion

using Magnesium;
using System;

namespace OffscreenDemo
{
    // Abstract base class for procedurally generated geometry
    // 
    // All classes derived from it must produce Counter-Clockwise (CCW) primitives.
    // Derived classes must create a single VBO and IBO, without primitive restarts for strips. 
    // Uses an double-precision all-possible-attributes VertexT2dN3dV3d Array internally.
    // Cannot directly use VBO, but has Get-methods to retrieve VBO-friendly data.
    // Can use a Display List to prevent repeated immediate mode draws.
    //

    public interface IDrawableShape: IDisposable
    {
        MgPrimitiveTopology PrimitiveMode { get;  }
        VertexT2fN3fV3f[] VertexArray { get; }
        uint[] IndexArray { get; }
    }
}
