using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium
{
    public class MgOffscreenAttachmentInfo
    {
        public IMgImageView View { get; set; }
        public MgAttachmentDescription Description { get; set; }
    }

    public class MgOffscreenColorAttachmentInfo
    {
        public IMgImageView View { get; set; }
        public MgFormat Format { get; set; }
        public MgAttachmentLoadOp LoadOp { get; set; }
        public MgAttachmentStoreOp StoreOp { get; set; }
    }

    public class MgOffscreenDepthStencilAttachmentInfo
    {
        public IMgImageView View { get; set; }
        public MgImageLayout Layout { get; set; }
        public MgFormat Format { get; set; }
        public MgAttachmentLoadOp LoadOp { get; set; }
        public MgAttachmentStoreOp StoreOp { get; set; }
        public MgAttachmentLoadOp StencilLoadOp { get; set; }
        public MgAttachmentStoreOp StencilStoreOp { get; set; }
    }

    public class MgOffscreenGraphicsDeviceCreateInfo
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public MgOffscreenColorAttachmentInfo[] ColorAttachments { get; set; }
        public MgOffscreenDepthStencilAttachmentInfo DepthStencilAttachment { get; set; }
    }

    public class MgOffscreenGraphicDevice : IMgEffectFramework
    {
        private MgRect2D mScissor;
        public MgRect2D Scissor
        {
            get
            {
                return mScissor;
            }
        }

        private MgViewport mViewport;
        public MgViewport Viewport
        {
            get
            {
                return mViewport;
            }
        }

        private MgRenderPassCreateInfo mRenderPassInfo;
        public MgRenderPassCreateInfo RenderpassInfo
        {
            get 
            {
                return mRenderPassInfo;
            }
        }

        private IMgRenderPass mRenderPass;
        public IMgRenderPass Renderpass
        {
            get
            {
                return mRenderPass;
            }
        }

        private IMgFramebuffer mFramebuffer;
        public IMgFramebuffer[] Framebuffers
        {
            get
            {
                return new[] { mFramebuffer };
            }
        }

        private IMgGraphicsConfiguration mConfiguration;
        public MgOffscreenGraphicDevice(IMgGraphicsConfiguration configuration, MgOffscreenGraphicsDeviceCreateInfo createInfo)
        {
            mConfiguration = configuration;
            Initialize(createInfo);
        }

        private void ReleaseUnmanagedResources()
        {
            if(mFramebuffer != null)
            {
                mFramebuffer.DestroyFramebuffer(mConfiguration.Device, null);
                mFramebuffer = null;
            }

            if (mRenderPass != null)
            {
                mRenderPass.DestroyRenderPass(mConfiguration.Device, null);
                mRenderPass = null;
            }
        }

        private void Initialize(MgOffscreenGraphicsDeviceCreateInfo createInfo)
        {
            // color attachments 
            if (createInfo == null)
                throw new ArgumentNullException("createInfo");

            var descriptions = new List<MgAttachmentDescription>();
            var subpassColorAttachments = new List<MgAttachmentReference>();
            var frameBufferAttachments = new List<IMgImageView>();

            var attachmentIndex = 0U;

            if (createInfo.ColorAttachments != null)
            {
                foreach (var item in createInfo.ColorAttachments)
                {
                    var attachment = new MgAttachmentDescription
                    {
                        Format = item.Format,
                        // TODO : multisampling
                        Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                        LoadOp = item.LoadOp,
                        StoreOp = item.StoreOp,
                        StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                        StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
                        // these settings might have to change
                        InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                        FinalLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                    };
                    descriptions.Add(attachment);

                    var itemReference = new MgAttachmentReference
                    {
                        Attachment = attachmentIndex,
                        Layout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                    };

                    subpassColorAttachments.Add(itemReference);

                    frameBufferAttachments.Add(item.View);
                }
            }

            MgAttachmentReference depthStencilAttachment = null;
            if (createInfo.DepthStencilAttachment != null)
            {
                var item = createInfo.DepthStencilAttachment;

                var attachment = new MgAttachmentDescription
                {
                    Format = item.Format,
                    // TODO : multisampling
                    Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                    LoadOp = item.LoadOp,
                    StoreOp = item.StoreOp,
                    StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                    StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
                    // these settings might have to change
                    InitialLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                    FinalLayout = MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                };
                descriptions.Add(attachment);

                depthStencilAttachment = new MgAttachmentReference
                {
                    Attachment = attachmentIndex,
                    Layout = item.Layout,
                };

                frameBufferAttachments.Add(item.View);
            }

            var pCreateInfo = new MgRenderPassCreateInfo
            {
                Attachments = descriptions.ToArray(),
                Subpasses = new MgSubpassDescription[]
                {
                    new MgSubpassDescription
                    {
                       PipelineBindPoint = MgPipelineBindPoint.GRAPHICS,
                       ColorAttachments = subpassColorAttachments.ToArray(),
                       ResolveAttachments = null,
                       DepthStencilAttachment = depthStencilAttachment,
                    }                 
                },
            };
            var err = mConfiguration.Device.CreateRenderPass(pCreateInfo, null, out IMgRenderPass pass);
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
            mRenderPass = pass;
            mRenderPassInfo = pCreateInfo;

            var frameBufferCreateInfo = new MgFramebufferCreateInfo
            {
                RenderPass = mRenderPass,
                Attachments = frameBufferAttachments.ToArray(),                
                Width = createInfo.Width,
                Height = createInfo.Height,
                Layers = 1,                
            };

            err = mConfiguration.Device.CreateFramebuffer(frameBufferCreateInfo, null, out IMgFramebuffer fBuf);
            Debug.Assert(err == Result.SUCCESS, err + " != Result.SUCCESS");
            mFramebuffer = fBuf;

            mScissor = new MgRect2D
            {
                Offset = new MgOffset2D
                {
                    X = 0,
                    Y = 0,
                },
                Extent = new MgExtent2D
                {
                    Width = createInfo.Width,
                    Height = createInfo.Height,
                }
            };

            mViewport = new MgViewport
            {
                X = 0,
                Y = 0,
                Width = createInfo.Width,
                Height = createInfo.Height,
                MinDepth = 0,
                MaxDepth = 1000f,
            };

        }

        ~MgOffscreenGraphicDevice()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool mIsDisposed = false;
        private void Dispose(bool dispose)
        {
            if (mIsDisposed)
                return;

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }
    }
}
