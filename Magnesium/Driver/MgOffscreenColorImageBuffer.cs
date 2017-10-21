using System;
using System.Diagnostics;

namespace Magnesium
{
    public class MgOffscreenColorImageBuffer : IDisposable
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgOffscreenDeviceLocalMemory mDeviceLocal;
        private IMgDeviceMemory mOffscreenMemory;

        public MgOffscreenColorImageBuffer(
            IMgGraphicsConfiguration configuration,
            IMgOffscreenDeviceLocalMemory deviceLocal)
        {
            mConfiguration = configuration;
            mDeviceLocal = deviceLocal;
        }

        public MgFormat Format { get; private set; }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        private IMgImageView mImageView;
        public IMgImageView View { get => mImageView; private set => mImageView = value; }

        public void Initialize(
            MgFormat format,
            uint width,
            uint height)
        {
            Width = width;
            Height = height;
            Format = format;

            // BASED ON 
            // https://github.com/SaschaWillems/Vulkan/blob/master/offscreen/offscreen.cpp

            // Color attachment
            var image = new MgImageCreateInfo
            {
                ImageType = MgImageType.TYPE_2D,
                Format = Format,
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
            mOffscreenImage = offscreenImage;

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
            mImageView = offscreenView;
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
        private IMgImage mOffscreenImage;

        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;
            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            if (mImageView != null)
            {
                mImageView.DestroyImageView(mConfiguration.Device, null);
            }

            if (mOffscreenImage != null)
            { 
                mOffscreenImage.DestroyImage(mConfiguration.Device, null);
            }

            mDeviceLocal.FreeMemory();
        }
    }
}
