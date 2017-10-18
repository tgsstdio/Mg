using System;
using Magnesium;

namespace OffscreenDemo
{
    class OffscreenDemoApplication : IDemoApplication
    {
        private OffscreenPipeline mRenderToTexture;
        private IScreenQuadPipeline mToScreen;

        public OffscreenDemoApplication(OffscreenPipeline renderToTexture, IScreenQuadPipeline toScreen)
        {
            mRenderToTexture = renderToTexture;
            mToScreen = toScreen;
        }

        public MgGraphicsDeviceCreateInfo Initialize()
        {
            return new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Width = 1280,
                Height = 720,
            };
        }

        public void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            const uint WIDTH = 1280;
            const uint HEIGHT = 720;
            const MgFormat COLOR_FORMAT = MgFormat.R8G8B8A8_UNORM;
            const MgFormat DEPTH_FORMAT = MgFormat.D32_SFLOAT;

            // 1. init offscreen device           
            var deviceLocal = new MgOffscreenDeviceLocalMemory(configuration);
            var colorOne = new MgOffscreenColorImageBuffer(
                configuration, 
                deviceLocal,
                COLOR_FORMAT,
                WIDTH,
                HEIGHT);

            var depthOne = new MgOffscreenDepthStencilContext(
                configuration,
                DEPTH_FORMAT,
                WIDTH,
                HEIGHT);

            var createInfo = new MgOffscreenGraphicsDeviceCreateInfo
            {
                Width = 1280,
                Height = 720,
                ColorAttachments = new []
                {
                    new MgOffscreenColorAttachmentInfo
                    {                        
                        Format = COLOR_FORMAT,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = colorOne.View
                    }
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = DEPTH_FORMAT,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                    StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
                    View = depthOne.View,
                    Layout = MgImageLayout.GENERAL,
                }
            };
            var offscreen = new Magnesium.MgOffscreenGraphicDevice(configuration, createInfo);
            // 2. init offscreen pipeline
            mRenderToTexture.Initialize(configuration, offscreen);

            // 3. init post processing pipeline
            mToScreen.Initialize(configuration, offscreen);

            // 4. init triangle 

            // 5. init screen quad

            // 6. init command buffers
           // mRenderToTexture.BuildCommandBuffers(offscreen, )

            throw new NotImplementedException();
        }

        public IMgSemaphore[] Render(IMgQueue queue, uint layerNo)
        {
            //throw new NotImplementedException();

            //// Command buffer to be sumitted to the queue

            //var submitInfos = new[]
            //{
            //    new MgSubmitInfo
            //    {
            //        // ADD COMMANDS HERE
            //        CommandBuffers = null,
            //    }
            //};

            //// Submit to queue
            //var err = mManager.Configuration.Queue.QueueSubmit(submitInfos, null);
            //Debug.Assert(err == Result.SUCCESS);

            return null;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void ReleaseManagedResources()
        {
  
        }

        public void ReleaseUnmanagedResources()
        {
        
        }
    }
}
