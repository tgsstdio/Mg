using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL.Internals
{
    public class GLNextRenderPass : IGLRenderPass
    {
        public MgRenderPassProfile Profile { get; private set; }

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

        public GLNextRenderPass(MgRenderPassCreateInfo createInfo)
        {
            if(createInfo == null)
                throw new ArgumentNullException("createInfo");

            if (createInfo.Attachments == null)
                throw new ArgumentNullException("createInfo.Attachments");

            if (createInfo.Subpasses == null)
                throw new ArgumentNullException("createInfo.Subpasses");

            Subpasses = MgSubpassTransactionExtractor.Extract(createInfo);
            Profile = new MgRenderPassProfile(createInfo);
            AttachmentFormats = InitializeAttachments(createInfo);
        }

        private static GLRenderPassClearAttachment[] InitializeAttachments(MgRenderPassCreateInfo createInfo)
        {
            var output = new List<GLRenderPassClearAttachment>();
            var noOfAttachments = createInfo.Attachments != null ? createInfo.Attachments.Length : 0;
            for (var i = 0U; i < noOfAttachments; i += 1)
            {
                var attachment = Populate(createInfo.Attachments[i], i);
                output.Add(attachment);
            }
            return output.ToArray();
        }

        private static GLRenderPassClearAttachment Populate(MgAttachmentDescription desc, uint index)
        {
            var attachment = new GLRenderPassClearAttachment
            {
                Index = index,
                Format = desc.Format,
                LoadOp = desc.LoadOp,
                StencilLoadOp = desc.StencilLoadOp,
                AttachmentType = GetAttachmentType(desc.Format),
                DivisorType = GetAttachmentDivisor(desc.Format),
            };
            return attachment;
        }

        public MgSubpassTransactionsInfo[] Subpasses { get; private set; }

        public GLRenderPassClearAttachment[] AttachmentFormats { get; private set; }

        public void DestroyRenderPass(IMgDevice device, IMgAllocationCallbacks allocator)
        {

        }
    }
}
