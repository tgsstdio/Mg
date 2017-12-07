using Magnesium;
using Magnesium.Utilities;
using System;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class SimpleRenderableShape : IDisposable
    {
        private IMgRenderableElement mRender;
        private SimpleEffectPipeline mPipeline;
        public SimpleRenderableShape(IMgRenderableElement render, SimpleEffectPipeline pipeline)
        {
            mRender = render;
            mPipeline = pipeline;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                ReleaseUnmanagedResources();

                disposedValue = true;
            }
        }

        ~SimpleRenderableShape()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private IMgDevice mDevice;
        private IMgCommandPool mCommandPool;
        public void Initialize(IMgDevice device)
        {
            mDevice = device;
            mCommandPool = CreateCommandPool(device);
        }

        private void ReleaseUnmanagedResources()
        {
            if (mDevice != null)
            {
                if (mDescriptorPool != null)
                {
                    if (mDescriptorSets != null)
                    {
                        mDevice.FreeDescriptorSets(mDescriptorPool, mDescriptorSets);
                        mDescriptorSets = null;
                    }

                    mDescriptorPool.DestroyDescriptorPool(mDevice, null);
                    mDescriptorPool = null;
                }

                if (mCommandPool != null)
                {
                    if (mCommandBuffers != null)
                    {
                        mDevice.FreeCommandBuffers(mCommandPool, mCommandBuffers);
                        mCommandBuffers = null;
                    }

                    mCommandPool.DestroyCommandPool(mDevice, null);
                    mCommandPool = null;
                }
            }
        }

        private static IMgCommandPool CreateCommandPool(IMgDevice device)
        {
            var poolCreateInfo = new MgCommandPoolCreateInfo {  };
            var err = device.CreateCommandPool(poolCreateInfo, null, out IMgCommandPool pool);
            Debug.Assert(err == Result.SUCCESS);
            return pool;
        }

        public void Reserve(MgBlockAllocationList request)
        {
            mRender.Reserve(request);
        }

        public void Populate(MgOptimizedStorageContainer container)
        {
            if (mCommandPool != null && mDevice != null)
            {
                IMgCommandBuffer[] copyCmds = new IMgCommandBuffer[1];
                var pAllocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = 1,
                    CommandPool = mCommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };
                var err = mDevice.AllocateCommandBuffers(pAllocateInfo, copyCmds);
                Debug.Assert(err == Result.SUCCESS);

                IMgCommandBuffer copyCmd = copyCmds[0];
                mRender.Populate(mDevice, container, copyCmd);
                mDevice.FreeCommandBuffers(mCommandPool, copyCmds);
            }
        }

        public void Update(MgOptimizedStorageContainer container)
        {
            if (Order != null)
            {
                mRender.Refresh(mDevice, container, Order.Framework);
            }
        }

        private IMgCommandBuffer[] mCommandBuffers;
        private IMgDescriptorSet[] mDescriptorSets;
        private IMgDescriptorPool mDescriptorPool;
        public MgCommandBuildOrder Order { get; private set; }
        public void GenerateOrder(MgOptimizedStorageContainer container)
        {
            var noOfCommandBuffers = (uint) mPipeline.Framework.Framebuffers.Length;
            mCommandBuffers = new IMgCommandBuffer[noOfCommandBuffers];

            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = noOfCommandBuffers,
                CommandPool = mCommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };
            var err = mDevice.AllocateCommandBuffers(pAllocateInfo, mCommandBuffers);
            Debug.Assert(err == Result.SUCCESS);

            mDescriptorPool = mPipeline.InitializePool();
            mDescriptorSets = mPipeline.Allocate(mDescriptorPool, noOfCommandBuffers);

            Order = new MgCommandBuildOrder
            {
                CommandBuffers = mCommandBuffers,
                Framework = mPipeline.Framework,     
                DescriptorSets = mDescriptorSets,
            };
            mRender.Setup(Order, container);
            mRender.Build(Order, mPipeline);
        }
    }
}
