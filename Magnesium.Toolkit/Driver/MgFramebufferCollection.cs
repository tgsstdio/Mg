using System.Diagnostics;

namespace Magnesium.Toolkit
{
    public class MgFramebufferCollection
    {
        private IMgGraphicsConfiguration mGraphicsConfiguration;
        public MgFramebufferCollection(IMgGraphicsConfiguration configuration)
        {
            mGraphicsConfiguration = configuration;
        }

        private IMgFramebuffer[] mFramebuffers;
        public IMgFramebuffer[] Framebuffers
        {
            get
            {
                return mFramebuffers;
            }
        }

        public void Create(IMgSwapchainCollection swapchains, IMgRenderPass pass, IMgImageView depthStencilView, uint width, uint height)
        {
            Debug.Assert(mGraphicsConfiguration.Partition != null);

            // Create frame buffers for every swap chain image
            var frameBuffers = new IMgFramebuffer[swapchains.Buffers.Length];
            for (uint i = 0; i < frameBuffers.Length; i++)
            {
                var frameBufferCreateInfo = new MgFramebufferCreateInfo
                {
                    RenderPass = pass,
                    Attachments = new[]
                    {
                        swapchains.Buffers[i].View,
						// Depth/Stencil attachment is the same for all frame buffers
						depthStencilView,
                    },
                    Width = width,
                    Height = height,
                    Layers = 1,
                };

                var err = mGraphicsConfiguration.Partition.Device.CreateFramebuffer(frameBufferCreateInfo, null, out frameBuffers[i]);
                Debug.Assert(err == MgResult.SUCCESS, err + " != MgResult.SUCCESS");
            }

            mFramebuffers = frameBuffers;
        }

        public void Clear()
        {
            if (mFramebuffers != null)
            {
                foreach (var fb in mFramebuffers)
                {
                    fb.DestroyFramebuffer(mGraphicsConfiguration.Partition.Device, null);
                }
                mFramebuffers = null;
            }
        }
    }
}
