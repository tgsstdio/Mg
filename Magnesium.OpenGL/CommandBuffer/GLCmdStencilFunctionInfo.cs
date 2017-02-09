using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLCmdStencilFunctionInfo : IEquatable<GLCmdStencilFunctionInfo>
    {
        public int ReferenceMask { get; set; }
        public MgCompareOp StencilFunction { get; set; }
        public uint CompareMask { get;  set; }

        public bool Equals(GLCmdStencilFunctionInfo other)
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