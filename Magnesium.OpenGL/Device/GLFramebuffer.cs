
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

	public class GLFramebuffer : IMgFramebuffer
	{
        private GLNextRenderPassSubpassInfo[] mSubpasses;

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

            var bRenderPass = (GLNextRenderPass) createInfo.RenderPass;

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
           // mSubpasses = new GLFramebufferSubpassInfo[noOfSubpasses];

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

