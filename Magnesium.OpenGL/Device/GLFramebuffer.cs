
using System;

namespace Magnesium.OpenGL.Internals
{
    public enum GLClearAttachmentDivisor : byte
    {
        FLOAT = 0,
        SIGNED_BYTE,
        UNSIGNED_BYTE,
        SIGNED_SHORT,
        UNSIGNED_SHORT,
        SIGNED_INT,
        UNSIGNED_INT,
    }

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

    public class GLFramebufferSubpassInfo
    {
        private uint mSubpass;
        private GLRenderPassClearAttachment[] mColorAttachments;
        private GLRenderPassClearAttachment mDepthStencil;

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

        public GLFramebufferSubpassInfo(MgRenderPassCreateInfo createInfo, uint subpassIndex)
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

	public class GLFramebuffer : IMgFramebuffer
	{
        private GLFramebufferSubpassInfo[] mSubpasses;

        public GLFramebuffer(IGLFramebufferHelperSelector selector, MgFramebufferCreateInfo createInfo)
        {
            if (createInfo == null)
            {
                throw new ArgumentNullException("createInfo");
            }

            if (createInfo.RenderPass == null)
            {
                throw new ArgumentNullException("createInfo.RenderPass");
            }

            var bRenderPass = (IGLRenderPass) createInfo.RenderPass;

            // iterate over attachements 
            // how to clear buffers 

            // how to build GL fbo


            for (var i = 0; i < createInfo.Attachments.Length; i += 1)
            {

                var clearFormat = bRenderPass.AttachmentFormats[i];

                if (clearFormat.AttachmentType == GLClearAttachmentType.DEPTH_STENCIL)
                {
                    // reserve i for expect MgClearDepthStencil
                        // either texture or renderbuffer
                }
                else 
                {
                    // use for color attachment i
                   // selector.Helper.FramebufferTexture2D(i, GL_FRAMEBUFFER_ATT GL_TEXTURE_2D_MULTISAMPLE, tex, 0);

                }
            }

            //var noOfSubpasses = bRenderPass.Subpasses.Length;
            //mSubpasses = new GLFramebufferSubpassInfo[noOfSubpasses];

            //for (var i = 0; i < noOfSubpasses; i++)
            //{
            //    var srcSubpass = bRenderpass.Subpasses[i];
            //    Debug.Assert(srcSubpass.ColorAttachments != null);

            //    var noOfColorAttachments = srcSubpass.ColorAttachments.Length;

            //    var dstSubpass = new AmtFramebufferSubpassInfo
            //    {
            //        ColorAttachments = new IAmtImageView[noOfColorAttachments],
            //        DepthStencil = (srcSubpass.DepthStencil != null)
            //            ? (IAmtImageView)createInfo.Attachments[srcSubpass.DepthStencil.Index]
            //            : null,
            //    };

            //    for (var j = 0; j < noOfColorAttachments; ++j)
            //    {
            //        var imageViewIndex = srcSubpass.ColorAttachments[j].Index;
            //        dstSubpass.ColorAttachments[j] = (IAmtImageView)createInfo.Attachments[imageViewIndex];
            //    }

            //    Subpasses[i] = dstSubpass;
            //}
        }

		#region IMgFramebuffer implementation
		public void DestroyFramebuffer (IMgDevice device, IMgAllocationCallbacks allocator)
		{
			
		}
		#endregion
	}
}

