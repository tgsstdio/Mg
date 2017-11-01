using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
    public class GLNextFramebufferSubpassInfo
    {
        public int Framebuffer { get; internal set; }
    }

    public class GLNextFramebuffer : IMgFramebuffer
    {
        private IGLFramebufferHelperSelector mSelector;

        public bool IsNullFramebuffer { get; private set; }
        public GLNextFramebuffer(IGLFramebufferHelperSelector selector, MgFramebufferCreateInfo createInfo)
        {
            if (createInfo == null)
                throw new ArgumentNullException("createInfo");

            if (createInfo.RenderPass == null)
                throw new ArgumentNullException("createInfo.RenderPass");

            var bRenderpass = (GLNextRenderPass) createInfo.RenderPass;

            mSelector = selector;
            CompatibilityProfile = bRenderpass.Profile;

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

            IsNullFramebuffer = isNullFramebuffer;

            if (isNullFramebuffer)
            { 
                var noOfSubpasses = bRenderpass.Subpasses.Length;
                Subpasses = new GLNextFramebufferSubpassInfo[noOfSubpasses];

                for (var i = 0; i < noOfSubpasses; i++)
                {
                    Subpasses[i] = GenerateFramebuffer(createInfo, bRenderpass.Subpasses[i]);
                }
            }
            else
            {
                Subpasses = new GLNextFramebufferSubpassInfo[] { };
            }
        }

        private GLNextFramebufferSubpassInfo GenerateFramebuffer(MgFramebufferCreateInfo createInfo, MgSubpassTransactionsInfo srcSubpass)
        {
            mSelector.Helper.GenFramebuffer(out int framebuffer);

            if (srcSubpass.ColorAttachments != null)
            {
                mSelector.Helper.BindFramebuffer(framebuffer);
                var noOfColorAttachments = srcSubpass.ColorAttachments.Length;

                for (var j = 0; j < noOfColorAttachments; ++j)
                {
                    var srcViewIndex = srcSubpass.ColorAttachments[j];
                    var bSrcView = (IGLImageView) createInfo.Attachments[srcViewIndex];

                    const int LEVEL = 0;
                    const int SAMPLES = 0;
                    mSelector.Helper.FramebufferColorAttachment(j, bSrcView.ViewTarget, bSrcView.TextureId, LEVEL, SAMPLES);
                }
            }

            {
                var srcViewIndex = srcSubpass.DepthAttachment.Value;
                var bSrcView = (IGLImageView)createInfo.Attachments[srcViewIndex];

                const int LEVEL = 0;
                const int SAMPLES = 0;
                mSelector.Helper.FramebufferDepthStencil(bSrcView.ViewTarget, bSrcView.TextureId, LEVEL, SAMPLES);
            }

            mSelector.Helper.CheckFramebufferStatus();
            mSelector.Helper.BindFramebuffer(0);

            return new GLNextFramebufferSubpassInfo {
                Framebuffer = framebuffer
            };
        }

        public MgRenderPassProfile CompatibilityProfile { get; }
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
