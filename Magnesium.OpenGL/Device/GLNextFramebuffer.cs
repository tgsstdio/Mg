using Magnesium.OpenGL.Internals;
using Magnesium.Toolkit;
using System;

namespace Magnesium.OpenGL
{
    public class GLNextFramebuffer : IMgFramebuffer
    {
        private IGLFramebufferHelperSelector mSelector;
        
        public GLNextFramebuffer(IGLFramebufferHelperSelector selector, MgFramebufferCreateInfo createInfo)
        {
            if (createInfo == null)
                throw new ArgumentNullException("createInfo");

            if (createInfo.RenderPass == null)
                throw new ArgumentNullException("createInfo.RenderPass");

            var bRenderpass = (GLNextRenderPass) createInfo.RenderPass;

            mSelector = selector;
            Profile = bRenderpass.Profile;

            bool isNullFramebuffer = true;
            if (createInfo.Attachments != null)
            {
                foreach (var attachment in createInfo.Attachments)
                {
                    var bView = (IGLImageView)attachment;
                    if (!bView.IsNullImage)
                    {
                        isNullFramebuffer = false;
                        break;
                    }
                }               
            }

            var noOfSubpasses = bRenderpass.Subpasses.Length;
            Subpasses = new GLNextFramebufferSubpassInfo[noOfSubpasses];
            for (var i = 0; i < noOfSubpasses; i++)
            {
                var fbo = 
                    (isNullFramebuffer)
                    ? 0
                    : GenerateFramebuffer(createInfo, bRenderpass.Subpasses[i]);
                Subpasses[i] = new GLNextFramebufferSubpassInfo
                {
                    Framebuffer = fbo,
                };
            }
        }

        private int GenerateFramebuffer(MgFramebufferCreateInfo createInfo, MgSubpassTransactionsInfo srcSubpass)
        {
            mSelector.Helper.GenFramebuffer(out int framebuffer);

            mSelector.Helper.BindFramebuffer(framebuffer);
            if (srcSubpass.ColorAttachments != null)
            {
                var noOfColorAttachments = srcSubpass.ColorAttachments.Length;

                var colorAttachments = new uint[noOfColorAttachments];
                for (var j = 0; j < noOfColorAttachments; ++j)
                {
                    var srcViewIndex = srcSubpass.ColorAttachments[j];
                    var bSrcView = (IGLImageView) createInfo.Attachments[srcViewIndex];

                    const int LEVEL = 0;
                    const int SAMPLES = 0;
                    mSelector.Helper.FramebufferColorAttachment(j, bSrcView.ViewTarget, bSrcView.TextureId, LEVEL, SAMPLES);
                    colorAttachments[j] = srcViewIndex;
                }

                mSelector.Helper.EnableColorAttachments(colorAttachments);
            }

            if (srcSubpass.DepthAttachment.HasValue)
            {
                var srcViewIndex = srcSubpass.DepthAttachment.Value;
                var bSrcView = (IGLImageView)createInfo.Attachments[srcViewIndex];

                const int LEVEL = 0;
                const int SAMPLES = 0;
                mSelector.Helper.FramebufferDepthStencil(bSrcView.ViewTarget, bSrcView.TextureId, LEVEL, SAMPLES);
            }

            mSelector.Helper.CheckFramebufferStatus();
            mSelector.Helper.BindFramebuffer(0);

            return framebuffer;
        }

        public MgRenderPassProfile Profile { get; }
        public GLNextFramebufferSubpassInfo[] Subpasses { get; }

        public void DestroyFramebuffer(IMgDevice device, IMgAllocationCallbacks allocator)
        {            
            foreach(var subpass in Subpasses)
            {
                if (subpass.Framebuffer != 0)
                {
                    mSelector.Helper.DeleteFramebuffer(subpass.Framebuffer);
                }
            }
        }
    }
}
