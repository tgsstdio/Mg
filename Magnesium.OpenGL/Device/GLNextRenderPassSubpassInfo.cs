
using System;

namespace Magnesium.OpenGL.Internals
{
    public class GLNextRenderPassSubpassInfo
    {
        private uint mSubpass;
        public uint Subpass { get => mSubpass; }

        private GLRenderPassClearAttachment[] mColorAttachments;
        public GLRenderPassClearAttachment[] ColorAttachments { get => mColorAttachments; }

        private GLRenderPassClearAttachment mDepthStencil;
        public GLRenderPassClearAttachment DepthStencil { get => mDepthStencil; }

        static GLClearAttachmentType GetAttachmentType(MgFormat format)
        {
            switch (format)
            {
                case MgFormat.D16_UNORM:
                case MgFormat.D16_UNORM_S8_UINT:
                case MgFormat.D24_UNORM_S8_UINT:
                case MgFormat.D32_SFLOAT:
                case MgFormat.D32_SFLOAT_S8_UINT:
                    return GLClearAttachmentType.DEPTH_STENCIL;

                case MgFormat.R8_SINT:
                case MgFormat.R8G8_SINT:
                case MgFormat.R8G8B8_SINT:
                case MgFormat.R8G8B8A8_SINT:
                case MgFormat.R16_SINT:
                case MgFormat.R16G16_SINT:
                case MgFormat.R16G16B16_SINT:
                case MgFormat.R16G16B16A16_SINT:
                case MgFormat.R32_SINT:
                case MgFormat.R32G32_SINT:
                case MgFormat.R32G32B32_SINT:
                case MgFormat.R32G32B32A32_SINT:
                case MgFormat.R64_SINT:
                case MgFormat.R64G64_SINT:
                case MgFormat.R64G64B64_SINT:
                case MgFormat.R64G64B64A64_SINT:
                    return GLClearAttachmentType.COLOR_INT;

                case MgFormat.R8_UINT:
                case MgFormat.R8G8_UINT:
                case MgFormat.R8G8B8_UINT:
                case MgFormat.R8G8B8A8_UINT:
                case MgFormat.R16_UINT:
                case MgFormat.R16G16_UINT:
                case MgFormat.R16G16B16_UINT:
                case MgFormat.R16G16B16A16_UINT:
                case MgFormat.R32_UINT:
                case MgFormat.R64_UINT:
                    return GLClearAttachmentType.COLOR_UINT;

                case MgFormat.R32_SFLOAT:
                case MgFormat.R32G32_SFLOAT:
                case MgFormat.R32G32B32_SFLOAT:
                case MgFormat.R32G32B32A32_SFLOAT:
                    return GLClearAttachmentType.COLOR_FLOAT;
                default:
                    throw new NotSupportedException();
            }
        }

        static GLClearAttachmentDivisor GetAttachmentDivisor(MgFormat format)
        {
            switch (format)
            {
                case MgFormat.D16_UNORM:
                case MgFormat.D16_UNORM_S8_UINT:
                case MgFormat.D24_UNORM_S8_UINT:
                case MgFormat.D32_SFLOAT:
                case MgFormat.D32_SFLOAT_S8_UINT:
                    return GLClearAttachmentDivisor.FLOAT;

                case MgFormat.R8_SINT:
                case MgFormat.R8G8_SINT:
                case MgFormat.R8G8B8_SINT:
                case MgFormat.R8G8B8A8_SINT:
                    return GLClearAttachmentDivisor.SIGNED_BYTE;

                case MgFormat.R8_UINT:
                case MgFormat.R8G8_UINT:
                case MgFormat.R8G8B8_UINT:
                case MgFormat.R8G8B8A8_UINT:
                    return GLClearAttachmentDivisor.UNSIGNED_BYTE;

                case MgFormat.R16_UINT:
                case MgFormat.R16G16_UINT:
                case MgFormat.R16G16B16_UINT:
                case MgFormat.R16G16B16A16_UINT:
                    return GLClearAttachmentDivisor.UNSIGNED_SHORT;

                case MgFormat.R16_SINT:
                case MgFormat.R16G16_SINT:
                case MgFormat.R16G16B16_SINT:
                case MgFormat.R16G16B16A16_SINT:
                    return GLClearAttachmentDivisor.SIGNED_SHORT;

                case MgFormat.R32_SINT:
                case MgFormat.R32G32_SINT:
                case MgFormat.R32G32B32_SINT:
                case MgFormat.R32G32B32A32_SINT:
                    return GLClearAttachmentDivisor.SIGNED_INT;

                // return smaller of the two
                case MgFormat.R64_SINT:
                case MgFormat.R64G64_SINT:
                case MgFormat.R64G64B64_SINT:
                case MgFormat.R64G64B64A64_SINT:
                    return GLClearAttachmentDivisor.UNSIGNED_INT;

                case MgFormat.R32_UINT:
                case MgFormat.R64_UINT:
                    return GLClearAttachmentDivisor.UNSIGNED_INT;

                case MgFormat.R32_SFLOAT:
                case MgFormat.R32G32_SFLOAT:
                case MgFormat.R32G32B32_SFLOAT:
                case MgFormat.R32G32B32A32_SFLOAT:
                    return GLClearAttachmentDivisor.FLOAT;

                default:
                    throw new NotSupportedException();
            }
        }

        public GLNextRenderPassSubpassInfo(MgRenderPassCreateInfo createInfo, uint subpassIndex)
        {
            mSubpass = subpassIndex;

            // check subpass description for correct values 
            var subpass = createInfo.Subpasses[subpassIndex];

            mColorAttachments = new GLRenderPassClearAttachment[subpass.ColorAttachmentCount];
            for (var i = 0; i < subpass.ColorAttachmentCount; ++i)
            {
                var color = subpass.ColorAttachments[i];
                var desc = createInfo.Attachments[color.Attachment];

                mColorAttachments[i] = new GLRenderPassClearAttachment
                {
                    Index = color.Attachment,
                    Format = desc.Format,
                    LoadOp = desc.LoadOp,
                    StencilLoadOp = desc.StencilLoadOp,
                    AttachmentType = GetAttachmentType(desc.Format),
                    DivisorType = GetAttachmentDivisor(desc.Format),
                };
            }

            var depthStencil = subpass.DepthStencilAttachment;
            if (depthStencil != null)
            {
                var desc = createInfo.Attachments[depthStencil.Attachment];
                mDepthStencil = new GLRenderPassClearAttachment
                {
                    Index = depthStencil.Attachment,
                    Format = desc.Format,
                    LoadOp = desc.LoadOp,
                    StencilLoadOp = desc.StencilLoadOp,
                    AttachmentType = GetAttachmentType(desc.Format),
                    DivisorType = GetAttachmentDivisor(desc.Format),
                };
            }
        }
    }
}

