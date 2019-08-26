using Magnesium;
using Magnesium.Utilities;
using System.Diagnostics;
using System;
using Magnesium.Toolkit;

namespace OffscreenDemo
{
    public class TriangleDemoApplication : IDemoApplication
    {
        private MgOptimizedStorageBuilder mBuilder;
        private OffscreenPipeline mRenderToTexture;
        private UnmanagedResources mUnmanagedResources;
        private IMgGraphicsDevice mGraphics;

        public TriangleDemoApplication(MgOptimizedStorageBuilder builder, OffscreenPipeline renderToTexture)
        {
            mBuilder = builder;
            mRenderToTexture = renderToTexture;
        }

        public MgGraphicsDeviceCreateInfo Initialize()
        {
            return new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Width = 640,
                Height = 480,
                MinDepth = 0f,
                MaxDepth = 1f,
            };
        }

        class UnmanagedResources
        {
            public IMgCommandPool Pool { get; internal set; }
            public MgOptimizedStorageContainer StorageContainer { get; internal set; }
            public MgCommandBuildOrder[] Orders { get; internal set; }
            public IMgSemaphore[] SecondStage { get; internal set; }

            internal void Destroy(IMgGraphicsConfiguration configuration)
            {
                var device = configuration.Device;
                Debug.Assert(device != null);

                if (SecondStage != null)
                {
                    foreach (var stage in SecondStage)
                    {
                        stage.DestroySemaphore(device, null);
                    }
                }

                if (StorageContainer != null)
                {
                    StorageContainer.Storage.Destroy(device, null);
                    StorageContainer = null;
                }

                if (Pool != null)
                {
                    if (Orders != null)
                    {
                        foreach (var order in Orders)
                        {
                            device.FreeCommandBuffers(Pool, order.CommandBuffers);
                            order.CommandBuffers = null;
                        }
                    }

                    Pool.DestroyCommandPool(device, null);
                    Pool = null;
                }
            }
        }


        public void Prepare(IMgGraphicsConfiguration configuration, IMgGraphicsDevice screen)
        {
            mGraphics = screen;
            // 2. init offscreen pipeline
            mRenderToTexture.Initialize(configuration, screen);

            mUnmanagedResources = new UnmanagedResources();

            mUnmanagedResources.Pool = CreateCommandPool(configuration);

            mUnmanagedResources.StorageContainer = ReserveStorageSlots(configuration, mUnmanagedResources.Pool);

            PopulateRenderingSlots(configuration, mUnmanagedResources.Pool, mUnmanagedResources.StorageContainer);

            // 4. uniforms 
            PopulateUniformSlots(configuration, mUnmanagedResources.StorageContainer);

            // 6. init command buffers
            mUnmanagedResources.Orders = BuildCommandBuffers(configuration, screen, mUnmanagedResources.Pool, mUnmanagedResources.StorageContainer);

            // semaphores
            int length = screen.Framebuffers.Length;
            mUnmanagedResources.SecondStage = new IMgSemaphore[length];
            for (var i = 0; i < length; i += 1)
            {
                mUnmanagedResources.SecondStage[i] = GenerateSemaphore(configuration);
            }
        }

        private static IMgCommandPool CreateCommandPool(IMgGraphicsConfiguration configuration)
        {
            var poolCreateInfo = new MgCommandPoolCreateInfo
            {

            };
            var err = configuration.Device.CreateCommandPool(poolCreateInfo, null, out IMgCommandPool pool);
            Debug.Assert(err == MgResult.SUCCESS);
            return pool;
        }

        private MgOptimizedStorageContainer ReserveStorageSlots(IMgGraphicsConfiguration configuration, IMgCommandPool pool)
        {
            // 4. init triangle 

            var slots = new MgBlockAllocationList();
            mRenderToTexture.Reserve(slots);

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
            Debug.Assert(err == MgResult.SUCCESS);

            IMgCommandBuffer copyCmd = copyCmds[0];
            mRenderToTexture.Populate(container, configuration, copyCmd);
            configuration.Device.FreeCommandBuffers(pool, copyCmds);
        }

        private void PopulateUniformSlots(IMgGraphicsConfiguration configuration, MgOptimizedStorageContainer container)
        {
            mRenderToTexture.SetupUniforms(configuration, container);
        }

        private MgCommandBuildOrder[] BuildCommandBuffers(
            IMgGraphicsConfiguration configuration,
            IMgEffectFramework toScreen,
            IMgCommandPool cmdPool,
            MgOptimizedStorageContainer container
        )
        {
            int noOfCommandBuffers = toScreen.Framebuffers.Length;
            var commandBuffers = new IMgCommandBuffer[noOfCommandBuffers];

            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = (uint)noOfCommandBuffers,
                CommandPool = cmdPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };
            var err = configuration.Device.AllocateCommandBuffers(pAllocateInfo, commandBuffers);
            Debug.Assert(err == MgResult.SUCCESS);

            var firstOrder = mRenderToTexture.GenerateBuildOrder(container);
            var index = 0;
            index = AppendOrder(firstOrder, toScreen, noOfCommandBuffers, commandBuffers, index);

            mRenderToTexture.BuildCommandBuffers(firstOrder);

            return new[] { firstOrder };
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

        private IMgSemaphore GenerateSemaphore(IMgGraphicsConfiguration configuration)
        {
            var createInfo = new MgSemaphoreCreateInfo
            {

            };
            var err = configuration.Device.CreateSemaphore(createInfo, null, out IMgSemaphore temp);
            Debug.Assert(err == MgResult.SUCCESS);
            return temp;
        }

        public void ReleaseManagedResources(IMgGraphicsConfiguration configuration)
        {
       
        }

        public void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            if (mUnmanagedResources != null)
            {
                mUnmanagedResources.Destroy(configuration);
            }
        }

        public IMgSemaphore[] Render(IMgQueue queue, uint layerNo, IMgSemaphore semaphore)
        {
            IMgSemaphore isDone = mUnmanagedResources.SecondStage[layerNo];

            var first = mUnmanagedResources.Orders[0];

            var secondInfo = new[] {
                new MgSubmitInfo
                {
                    WaitSemaphores = new []
                    {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask = MgPipelineStageFlagBits.ALL_COMMANDS_BIT,
                            WaitSemaphore = semaphore,
                        },
                    },
                    CommandBuffers = new [] { first.CommandBuffers[layerNo] },
                    SignalSemaphores = new []
                    {
                        isDone
                    }
                }
            };

            //// Submit to queue
            var err = queue.QueueSubmit(secondInfo, null);
            Debug.Assert(err == MgResult.SUCCESS);

            err = queue.QueueWaitIdle();
            Debug.Assert(err == MgResult.SUCCESS);

            return new IMgSemaphore[] { isDone };
        }

        public void Update(IMgGraphicsConfiguration configuration)
        {
            mRenderToTexture.UpdateUniformBuffers(configuration, mUnmanagedResources.StorageContainer, mGraphics);
        }
    }
}
