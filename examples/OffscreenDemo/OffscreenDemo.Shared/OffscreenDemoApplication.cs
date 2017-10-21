﻿using System;
using Magnesium;
using Magnesium.Utilities;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class OffscreenDemoApplication : IDemoApplication
    {
        private OffscreenPipeline mRenderToTexture;
        private ToScreenPipeline mToScreen;
        private MgOptimizedStorageBuilder mBuilder;

        public OffscreenDemoApplication(MgOptimizedStorageBuilder builder, OffscreenPipeline renderToTexture, ToScreenPipeline toScreen)
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

        class DemoAppResources
        {
            public MgOffscreenDeviceLocalMemory DeviceLocal { get; internal set; }
            public MgOffscreenColorImageBuffer ColorOne { get; internal set; }
            public MgOffscreenDepthStencilContext DepthOne { get; internal set; }
            public MgOffscreenGraphicDevice Offscreen { get; internal set; }

            public void Dispose(IMgGraphicsConfiguration configuration)
            {
                if (Offscreen != null)
                    Offscreen.Dispose();

                if (DepthOne != null)
                    DepthOne.Dispose();

                if (DeviceLocal != null)
                    DeviceLocal.FreeMemory();

                if (ColorOne != null)
                    ColorOne.Dispose();
            }
        }

        private DemoAppResources mResources;
        public void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            const uint WIDTH = 1280;
            const uint HEIGHT = 720;
            const MgFormat COLOR_FORMAT = MgFormat.R8G8B8A8_UNORM;
            const MgFormat DEPTH_FORMAT = MgFormat.D32_SFLOAT;

            mResources = new DemoAppResources();

            mResources.DepthOne = new MgOffscreenDepthStencilContext(configuration);
            mResources.DepthOne.Initialize(DEPTH_FORMAT, WIDTH, HEIGHT);

            // 1. init offscreen device           
            mResources.DeviceLocal = new MgOffscreenDeviceLocalMemory(configuration);

            mResources.ColorOne = new MgOffscreenColorImageBuffer(
                configuration,
                mResources.DeviceLocal);

            mResources.ColorOne.Initialize(
                COLOR_FORMAT,
                WIDTH,
                HEIGHT);

            mResources.Offscreen = new Magnesium.MgOffscreenGraphicDevice(configuration);

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
                        View = mResources.ColorOne.View
                    }
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = DEPTH_FORMAT,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                    StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
                    View = mResources.DepthOne.View,
                    Layout = MgImageLayout.GENERAL,
                }
            };

            mResources.Offscreen.Initialize(createInfo);
            /**

// 2. init offscreen pipeline
mRenderToTexture.Initialize(configuration, offscreen);

// 3. init post processing pipeline
mToScreen.Initialize(configuration, offscreen);

var pool = CreateCommandPool(configuration);

var storage = ReserveStorageSlots(configuration, pool);

PopulateRenderingSlots(configuration, pool, storage);

// 4. uniforms 
//  PopulateUniformSlots(configuration, pool, storage);

// PopulateDescriptorSets();

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

**/
        }

        private static IMgImageView InitializeImageView(IMgGraphicsConfiguration configuration, IMgImage image, MgFormat format, MgImageAspectFlagBits aspectMask)
        {
            var pViewCreateInfo = new MgImageViewCreateInfo
            {
                ViewType = MgImageViewType.TYPE_2D,
                Components = new MgComponentMapping
                {
                    R = MgComponentSwizzle.R,
                    G = MgComponentSwizzle.G,
                    B = MgComponentSwizzle.B,
                    A = MgComponentSwizzle.A,
                },
                Format = format,
                SubresourceRange = new MgImageSubresourceRange
                {
                    AspectMask = aspectMask,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                },
                Image = image,
            };
            var err = configuration.Device.CreateImageView(pViewCreateInfo, null, out IMgImageView pView);
            Debug.Assert(err == Result.SUCCESS);
            return pView;
        }

        private static IMgImage InitializeImage(IMgGraphicsConfiguration configuration, MgFormat format, uint width, uint height)
        {
            var pImageCreateInfo = new MgImageCreateInfo
            {
                Format = format,
                Usage = MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT,
                Extent = new MgExtent3D
                {
                    Width = width,
                    Height = height,
                    Depth = 1,
                },
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                ImageType = MgImageType.TYPE_2D,
                InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,
                MipLevels = 1,
                ArrayLayers = 1,
                Tiling = MgImageTiling.OPTIMAL,
            };
            var err = configuration.Device.CreateImage(pImageCreateInfo, null, out IMgImage pImage);
            Debug.Assert(err == Result.SUCCESS);
            return pImage;
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

            return new IMgSemaphore[] { };
        }

        public void Update()
        {

        }

        public void ReleaseManagedResources(IMgGraphicsConfiguration configuration)
        {
  
        }

        public void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            Debug.Assert(mResources != null);
            mResources.Dispose(configuration);
        }
    }
}
