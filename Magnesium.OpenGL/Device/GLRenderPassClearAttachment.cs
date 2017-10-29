
using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLRenderPassClearAttachment : IEquatable<GLRenderPassClearAttachment>
    {
        public uint Index { get; internal set; }
        public MgFormat Format { get; set; }
        public MgAttachmentLoadOp LoadOp { get; set; }
        public MgAttachmentLoadOp StencilLoadOp { get; set; }
        public GLClearAttachmentType AttachmentType { get; set; }
        public GLClearAttachmentDivisor DivisorType { get; set; }

        public float GetDivisor()
        {
            switch(DivisorType)
            {
                case GLClearAttachmentDivisor.SIGNED_BYTE:
                    return sbyte.MaxValue;
                case GLClearAttachmentDivisor.SIGNED_SHORT:
                    return short.MaxValue;
                case GLClearAttachmentDivisor.SIGNED_INT:
                    return int.MaxValue;
                case GLClearAttachmentDivisor.UNSIGNED_BYTE:
                    return byte.MaxValue;
                case GLClearAttachmentDivisor.UNSIGNED_SHORT:
                    return ushort.MaxValue;
                case GLClearAttachmentDivisor.UNSIGNED_INT:
                    return uint.MaxValue;
                case GLClearAttachmentDivisor.FLOAT:
                default:
                    return 1f;
            }
        }

        #region IEquatable implementation
        public bool Equals(GLRenderPassClearAttachment other)
        {
            if (Format != other.Format)
                return false;

            if (LoadOp != other.LoadOp)
                return false;

            if (StencilLoadOp != other.StencilLoadOp)
                return false;

            if (AttachmentType != other.AttachmentType)
                return false;

            return DivisorType == other.DivisorType;
        }
        #endregion
    }
}

