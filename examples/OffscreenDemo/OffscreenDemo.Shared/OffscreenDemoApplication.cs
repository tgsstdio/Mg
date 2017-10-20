using System;
using Magnesium;
using Magnesium.Utilities;
using System.Diagnostics;

namespace OffscreenDemo
{
    class OffscreenDemoApplication : IDemoApplication
    {
        private OffscreenPipeline mRenderToTexture;
        private IScreenQuadPipeline mToScreen;
        private MgOptimizedStorageBuilder mBuilder;

        public OffscreenDemoApplication(MgOptimizedStorageBuilder builder, OffscreenPipeline renderToTexture, IScreenQuadPipeline toScreen)
        {
            mRenderToTexture = renderToTexture;
            mToScreen = toScreen;
            mBuilder = builder;
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
                ColorAttachments = new[]
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

            var pool = CreateCommandPool(configuration);

            var storage = ReserveStorageSlots(configuration, pool);

            PopulateRenderingSlots(configuration, pool, storage);

            // 4. uniforms 
            PopulateUniformSlots(configuration, pool, storage);

            PopulateDescriptorSets();

            // 6. init command buffers

            var noOfCommandBuffers = offscreen.Framebuffers.Length + screen.Framebuffers.Length;
            var commandBuffers = new IMgCommandBuffer[noOfCommandBuffers];

            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = (uint)noOfCommandBuffers,
                CommandPool = pool,
                Level = MgCommandBufferLevel.PRIMARY,
            };
            var err = configuration.Device.AllocateCommandBuffers(pAllocateInfo, commandBuffers);
            Debug.Assert(err == Result.SUCCESS);

            var firstOrder = mRenderToTexture.GenerateBuildOrder(storage);
            // SAME ARRAY 
            firstOrder.CommandBuffers = commandBuffers;
            firstOrder.First = 0;
            firstOrder.Count = offscreen.Framebuffers.Length;            

            mRenderToTexture.BuildCommandBuffers(firstOrder);

            var secondOrder = mToScreen.GenerateBuildOrder(storage);
            // SAME ARRAY
            secondOrder.CommandBuffers = commandBuffers;
            secondOrder.First = firstOrder.Count;
            secondOrder.Count = screen.Framebuffers.Length;

            mToScreen.BuildCommandBuffers(secondOrder);

            throw new NotImplementedException();
        }

        private static IMgCommandPool CreateCommandPool(IMgGraphicsConfiguration configuration)
        {
            var poolCreateInfo = new MgCommandPoolCreateInfo
            {

            };
            var err = configuration.Device.CreateCommandPool(poolCreateInfo, null, out IMgCommandPool pool);
            Debug.Assert(err == Result.SUCCESS);
            return pool;
        }

        private MgOptimizedStorage ReserveStorageSlots(IMgGraphicsConfiguration configuration, IMgCommandPool pool)
        {
            // 4. init triangle 

            var slots = new MgBlockAllocationList();
            mRenderToTexture.Reserve(slots);

            // 5. init screen quad
            mToScreen.Reserve(slots);

            var storageCreateInfo = new MgOptimizedStorageCreateInfo
            {
                Allocations = slots.ToArray(),
            };
            return mBuilder.Build(storageCreateInfo);
        }

        private void PopulateRenderingSlots(IMgGraphicsConfiguration configuration, IMgCommandPool pool, MgOptimizedStorage storage)
        {
            IMgCommandBuffer[] copyCmds = new IMgCommandBuffer[1];
            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = 1,
                CommandPool = pool,
                Level = MgCommandBufferLevel.PRIMARY,
            };
            var err = configuration.Device.AllocateCommandBuffers(pAllocateInfo, copyCmds);
            Debug.Assert(err == Result.SUCCESS);

            IMgCommandBuffer copyCmd = copyCmds[0];
            mRenderToTexture.Populate(storage, configuration, copyCmd);
            mToScreen.Populate(storage, configuration, copyCmd);
            configuration.Device.FreeCommandBuffers(pool, copyCmds);
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
