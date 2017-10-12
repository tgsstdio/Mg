using System;
using System.Diagnostics;

namespace Magnesium
{
    public class MgOffscreenColorImageBuffer : IMgImageBufferCollection
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgOffscreenDeviceLocalMemory mDeviceLocal;
        private IMgDeviceMemory mOffscreenMemory;

        public MgOffscreenColorImageBuffer(IMgGraphicsConfiguration configuration, IMgOffscreenDeviceLocalMemory deviceLocal)
        {
            mConfiguration = configuration;
            mDeviceLocal = deviceLocal;
            mBuffers = new MgSwapchainBuffer[0];
        }

        public MgFormat Format { get; private set; }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        private MgSwapchainBuffer[] mBuffers;
        public MgSwapchainBuffer[] Buffers
        {
            get
            {
                return mBuffers;
            }
        }

        public void Create(IMgCommandBuffer cmd, MgColorFormatOption option, MgFormat overrideFormat, uint width, uint height)
        {
            // BASED ON 
            // https://github.com/SaschaWillems/Vulkan/blob/master/offscreen/offscreen.cpp

            Width = width;
            Height = height;
            Format = overrideFormat;

            ReleaseUnmanagedResources();

            // Color attachment
            var image = new MgImageCreateInfo
            {
                ImageType = MgImageType.TYPE_2D,
                Format = overrideFormat,
                Extent = new MgExtent3D
                {
                    Width = Width,
                    Height = Height,
                    Depth = 1,
                },
                MipLevels = 1,
                ArrayLayers = 1,
                // TODO : multisampling
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Tiling = MgImageTiling.OPTIMAL,
                // We will sample directly from the color attachment
                Usage =
                    MgImageUsageFlagBits.COLOR_ATTACHMENT_BIT
                    | MgImageUsageFlagBits.SAMPLED_BIT
            };

            var err = mConfiguration.Device.CreateImage(image, null, out IMgImage offscreenImage);
            Debug.Assert(err == Result.SUCCESS);

            mDeviceLocal.Initialize(offscreenImage);

            var colorImageView = new MgImageViewCreateInfo
            {
                ViewType = MgImageViewType.TYPE_2D,
                Format = Format,
                SubresourceRange = new MgImageSubresourceRange
                {
                    AspectMask = MgImageAspectFlagBits.COLOR_BIT,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                },
                Image = offscreenImage,
            };
            err = mConfiguration.Device.CreateImageView(colorImageView, null, out IMgImageView offscreenView);
            Debug.Assert(err == Result.SUCCESS);

            mBuffers = new[]
            {
               new MgSwapchainBuffer
               {
               Image = offscreenImage,
               View = offscreenView,
               }
            };
        }

        ~MgOffscreenColorImageBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Free all Vulkan resources used by the swap chain
        private bool mIsDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;
            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            foreach (var buffer in mBuffers)
            {
                buffer.View.DestroyImageView(mConfiguration.Device, null);
                buffer.Image.DestroyImage(mConfiguration.Device, null);
            }
            mBuffers = new MgSwapchainBuffer[0];

            mDeviceLocal.FreeMemory();
        }
    }
}
