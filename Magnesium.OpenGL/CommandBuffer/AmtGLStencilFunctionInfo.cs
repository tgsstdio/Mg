using System;

namespace Magnesium.OpenGL
{
    internal struct AmtGLStencilFunctionInfo : IEquatable<AmtGLStencilFunctionInfo>
    {
        public int ReferenceMask { get; set; }
        public MgCompareOp StencilFunction { get; set; }
        public uint WriteMask { get;  set; }
        public int CompareMask { get;  set; }

        public bool Equals(AmtGLStencilFunctionInfo other)
        {
            if (StencilFunction != other.StencilFunction)
                return false;

            //if (WriteMask != other.WriteMask)
            //    return false;

            if (ReferenceMask != other.ReferenceMask)
                return false;

            return (CompareMask == other.CompareMask);
        }
    }
}