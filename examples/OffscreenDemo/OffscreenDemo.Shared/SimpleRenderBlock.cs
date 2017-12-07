using Magnesium;
using Magnesium.Utilities;
using System;

namespace OffscreenDemo
{
    public class SimpleRenderBlock : IDisposable
    {
        private MgOptimizedStorageBuilder mBuilder;
        private SimpleRenderableShape[] mShapes;
        public SimpleRenderBlock(
            MgOptimizedStorageBuilder builder,
            SimpleRenderableShape[] shapes
        )
        {
            mBuilder = builder;
            mShapes = shapes;
        }

        private IMgDevice mDevice;
        private MgOptimizedStorageContainer mContainer;
        public void Initialize(IMgGraphicsConfiguration configuration)
        {
            mDevice = configuration.Device;

            var request = new MgBlockAllocationList();
            foreach (var shape in mShapes)
            {
                shape.Initialize(mDevice);
                shape.Reserve(request);
            }
            var storageCreateInfo = new MgOptimizedStorageCreateInfo
            {
                Allocations = request.ToArray(),
            };
            mContainer = mBuilder.Build(storageCreateInfo);
            foreach (var shape in mShapes)
            {
                shape.Populate(mContainer);
                shape.Update(mContainer);
                shape.GenerateOrder(mContainer);
            }
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

        private void ReleaseUnmanagedResources()
        {
            if (mShapes != null)
            {
                foreach (var shape in mShapes)
                {
                    shape.Dispose();
                }
                mShapes = null;
            }

            if (mDevice != null)
            {
                if (mContainer != null)
                {
                    mContainer.Storage.Destroy(mDevice, null);
                    mContainer = null;
                }
            }
        }

        ~SimpleRenderBlock() {
           Dispose(false);
         }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
