using System;
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
        private MgOffscreenDeviceFactory mOffscreenFactory;

        public OffscreenDemoApplication(
            MgOptimizedStorageBuilder builder,
            OffscreenPipeline renderToTexture,
            ToScreenPipeline toScreen,
            MgOffscreenDeviceFactory offscreenFactory
            )
        {
            mRenderToTexture = renderToTexture;
            mToScreen = toScreen;
            mBuilder = builder;
            mOffscreenFactory = offscreenFactory;
        }

        public MgGraphicsDeviceCreateInfo Initialize()
        {
            return new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Width = 1280,
                Height = 720,
                MinDepth = 0f,
                MaxDepth = 1f,
            };
        }

        class UnmanagedResources
        {
            public MgOffscreenDeviceLocalMemory DeviceLocal { get; internal set; }
            public IMgOffscreenDeviceAttachment ColorOne { get; internal set; }
            public IMgOffscreenDeviceAttachment DepthOne { get; internal set; }
            public IMgEffectFramework Offscreen { get; internal set; }
            public IMgCommandPool Pool { get; internal set; }
            public MgOptimizedStorageContainer StorageContainer { get; internal set; }
            public MgCommandBuildOrder[] Orders { get; internal set; }
            public IMgSemaphore FirstStage { get; internal set; }
            public IMgSemaphore[] SecondStage { get; internal set; }

            public void Dispose(IMgGraphicsConfiguration configuration)
            {
                var device = configuration.Device;
                Debug.Assert(device != null);

                if (SecondStage != null)
                {
                    foreach(var stage in SecondStage)
                    {
                        stage.DestroySemaphore(device, null);
                    }
                }

                if (FirstStage != null)
                {
                    FirstStage.DestroySemaphore(device, null);
                    FirstStage = null;
                }

                if (StorageContainer != null)
                {
                    if (StorageContainer.Storage != null)
                    {
                        StorageContainer.Storage.Destroy(device, null);
                    }
                }

                if (Pool != null)
                {
                    if (Orders != null)
                    {
                        foreach(var order in Orders)
                        {
                            if (order.CommandBuffers != null)
                            {
                                device.FreeCommandBuffers(Pool, order.CommandBuffers);
                                order.CommandBuffers = null;
                            }
                        }
                    }

                    Pool.DestroyCommandPool(configuration.Device, null);
                    Pool = null;
                }

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

        private IMgGraphicsDevice mGraphics;
        private UnmanagedResources mUnmanagedResources;
        public void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            const uint WIDTH = 256;
            const uint HEIGHT = 128;
            const MgFormat COLOR_FORMAT = MgFormat.R8G8B8A8_UNORM;
            const MgFormat DEPTH_FORMAT = MgFormat.D32_SFLOAT;

            mGraphics = screen;
            mUnmanagedResources = new UnmanagedResources();

            mUnmanagedResources.DepthOne = mOffscreenFactory.CreateDepthStencilAttachment(DEPTH_FORMAT, WIDTH, HEIGHT);

            // 1. init offscreen device           
            // TODO IOC this component somehow

            mUnmanagedResources.ColorOne = mOffscreenFactory.CreateColorAttachment(COLOR_FORMAT, WIDTH, HEIGHT);

            var createInfo = new MgOffscreenDeviceCreateInfo
            {
                Width = WIDTH,
                Height = HEIGHT,
                ColorAttachments = new[]
                {
                    new MgOffscreenColorAttachmentInfo
                    {
                        Format = COLOR_FORMAT,
                        LoadOp = MgAttachmentLoadOp.CLEAR,
                        StoreOp = MgAttachmentStoreOp.STORE,
                        View = mUnmanagedResources.ColorOne.View
                    }
                },
                DepthStencilAttachment = new MgOffscreenDepthStencilAttachmentInfo
                {
                    Format = DEPTH_FORMAT,
                    LoadOp = MgAttachmentLoadOp.CLEAR,
                    StoreOp = MgAttachmentStoreOp.STORE,
                    StencilLoadOp = MgAttachmentLoadOp.DONT_CARE,
                    StencilStoreOp = MgAttachmentStoreOp.DONT_CARE,
                    View = mUnmanagedResources.DepthOne.View,
                    Layout = MgImageLayout.GENERAL,
                },
                MinDepth = 0f,
                MaxDepth = 1f,
            };

            mUnmanagedResources.Offscreen = mOffscreenFactory.CreateOffscreenDevice(createInfo);

            // 2. init offscreen pipeline
            mRenderToTexture.Initialize(configuration, mUnmanagedResources.Offscreen);

            // 3. init post processing pipeline
            mToScreen.Initialize(configuration, screen);

            mUnmanagedResources.Pool = CreateCommandPool(configuration);

            mUnmanagedResources.StorageContainer = ReserveStorageSlots(configuration, mUnmanagedResources.Pool);

            PopulateRenderingSlots(configuration, mUnmanagedResources.Pool, mUnmanagedResources.StorageContainer);

            // 4. uniforms 
            PopulateUniformSlots(configuration, mUnmanagedResources.StorageContainer, mUnmanagedResources.ColorOne.View);

            // 6. init command buffers
            mUnmanagedResources.Orders = BuildCommandBuffers(configuration, mUnmanagedResources.Offscreen, screen, mUnmanagedResources.Pool, mUnmanagedResources.StorageContainer);

            // semaphores
            mUnmanagedResources.FirstStage = GenerateSemaphore(configuration);

            int length = mGraphics.Framebuffers.Length;
            mUnmanagedResources.SecondStage = new IMgSemaphore[length];
            for(var i = 0; i < length; i += 1)
            {
                mUnmanagedResources.SecondStage[i] = GenerateSemaphore(configuration);
            }
        }

        private IMgSemaphore GenerateSemaphore(IMgGraphicsConfiguration configuration)
        {
            var createInfo = new MgSemaphoreCreateInfo
            {
                
            };
            var err = configuration.Device.CreateSemaphore(createInfo, null, out IMgSemaphore temp);
            Debug.Assert(err == Result.SUCCESS);
            return temp;
        }

        private MgCommandBuildOrder[] BuildCommandBuffers(
            IMgGraphicsConfiguration configuration,
            IMgEffectFramework offscreen,
            IMgEffectFramework toScreen,
            IMgCommandPool cmdPool,
            MgOptimizedStorageContainer container
        )
        {
            int firstSection = offscreen.Framebuffers.Length;
            int secondSection = toScreen.Framebuffers.Length;
            var noOfCommandBuffers = firstSection + toScreen.Framebuffers.Length;
            var commandBuffers = new IMgCommandBuffer[noOfCommandBuffers];

            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = (uint)noOfCommandBuffers,
                CommandPool = cmdPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };
            var err = configuration.Device.AllocateCommandBuffers(pAllocateInfo, commandBuffers);
            Debug.Assert(err == Result.SUCCESS);

            var firstOrder = mRenderToTexture.GenerateBuildOrder(container);
            var index = 0;
            index = AppendOrder(firstOrder, offscreen, firstSection, commandBuffers, index);

            mRenderToTexture.BuildCommandBuffers(firstOrder);

            var secondOrder = mToScreen.GenerateBuildOrder(container);
            index = AppendOrder(secondOrder, toScreen, secondSection, commandBuffers, index);

            mToScreen.BuildCommandBuffers(secondOrder);

            return new[] { firstOrder, secondOrder };
        }

        private static int AppendOrder(MgCommandBuildOrder order, IMgEffectFramework framework, int count, IMgCommandBuffer[] source, int index)
        {
            order.Framework = framework;
            order.CommandBuffers = new IMgCommandBuffer[count];
            for (var i = 0; i < count; i += 1)
            {
                order.CommandBuffers[i] = source[index];
                index += 1;
            }

            order.First = 0;
            order.Count = count;
            return index;
        }

        private void PopulateUniformSlots(IMgGraphicsConfiguration configuration, MgOptimizedStorageContainer container, IMgImageView view)
        {
            mRenderToTexture.SetupUniforms(configuration, container);
            mToScreen.SetupUniforms(configuration, container, view);
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

        private MgOptimizedStorageContainer ReserveStorageSlots(IMgGraphicsConfiguration configuration, IMgCommandPool pool)
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

        private void PopulateRenderingSlots(IMgGraphicsConfiguration configuration, IMgCommandPool pool, MgOptimizedStorageContainer container)
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
            mRenderToTexture.Populate(container, configuration, copyCmd);
            mToScreen.Populate(container, configuration, copyCmd);
            configuration.Device.FreeCommandBuffers(pool, copyCmds);
        }

        public IMgSemaphore[] Render(IMgQueue queue, uint layerNo, IMgSemaphore semaphore)
        {
            //// Command buffer to be sumitted to the queue

            var first = mUnmanagedResources.Orders[0];
            var second = mUnmanagedResources.Orders[1];

            var firstInfo = new[]
            {
                new MgSubmitInfo
                {
                    // ADD COMMANDS HERE                    
                     WaitSemaphores = new []
                     {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask = MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            WaitSemaphore = semaphore,
                        },
                    },
                    CommandBuffers = new [] { first.CommandBuffers[0] },
                    SignalSemaphores = new [] { mUnmanagedResources.FirstStage },
                },
            };

            var err = queue.QueueSubmit(firstInfo, null);
            Debug.Assert(err == Result.SUCCESS);

            IMgSemaphore isDone = mUnmanagedResources.SecondStage[layerNo];

            var secondInfo = new [] {
                new MgSubmitInfo
                {
                    WaitSemaphores = new []
                    {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask = MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            WaitSemaphore = mUnmanagedResources.FirstStage,
                        },
                    },
                    CommandBuffers = new [] { second.CommandBuffers[layerNo] },
                    SignalSemaphores = new []
                    {
                        isDone
                    }
                }
            };

            //// Submit to queue
            err = queue.QueueSubmit(secondInfo, null);
            Debug.Assert(err == Result.SUCCESS);

            err = queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            return new IMgSemaphore[] { isDone };
           // return new IMgSemaphore[] { };
        }

        public void Update(IMgGraphicsConfiguration configuration)
        {
            mRenderToTexture.UpdateUniformBuffers(configuration, mUnmanagedResources.StorageContainer, mUnmanagedResources.Offscreen);
            mToScreen.UpdateUniformBuffers(configuration, mUnmanagedResources.StorageContainer, mGraphics);
        }

        public void ReleaseManagedResources(IMgGraphicsConfiguration configuration)
        {
  
        }

        public void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            if (mUnmanagedResources != null)
            {
                mUnmanagedResources.Dispose(configuration);
            }
            mRenderToTexture.ReleaseUnmanagedResources(configuration);
            mToScreen.ReleaseUnmanagedResources(configuration);
        }
    }
}
