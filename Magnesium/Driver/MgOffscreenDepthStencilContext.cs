using System;
using System.Diagnostics;

namespace Magnesium
{
    public class MgOffscreenDepthStencilContext : IDisposable
    {
        private IMgGraphicsConfiguration mConfiguration;
        public MgOffscreenDepthStencilContext(IMgGraphicsConfiguration configuration)
        {
            mConfiguration = configuration;
        }

        private IMgImageView mDepthView;
        public IMgImageView View { get => mDepthView; private set => mDepthView = value; }

        private IMgDeviceMemory mDepthStencilMemory;
        private IMgImage mDepthStencilImage;

        ~MgOffscreenDepthStencilContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool mIsDisposed = false;



        private void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        public void ReleaseUnmanagedResources()
        {
            if (mDepthView != null)
            {
                mDepthView.DestroyImageView(mConfiguration.Device, null);
                mDepthView = null;
            }
            if (mDepthStencilImage != null)
            {
                mDepthStencilImage.DestroyImage(mConfiguration.Device, null);
                mDepthStencilImage = null;
            }
            if (mDepthStencilMemory != null)
            {
                mDepthStencilMemory.FreeMemory(mConfiguration.Device, null);
                mDepthStencilMemory = null;
            }
        }

        public void Initialize(MgFormat depthFormat, uint width, uint height)
        {
            // Color attachment
            var image = new MgImageCreateInfo
            {
                ImageType = MgImageType.TYPE_2D,
                Format = depthFormat,
                Extent = new MgExtent3D
                {
                    Width = width,
                    Height = height,
                    Depth = 1,
                },
                MipLevels = 1,
                ArrayLayers = 1,
                // TODO : multisampling
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Tiling = MgImageTiling.OPTIMAL,
                Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT,
                InitialLayout = MgImageLayout.COLOR_ATTACHMENT_OPTIMAL,                
            };

            var err = mConfiguration.Device.CreateImage(image, null, out IMgImage offscreenImage);
            Debug.Assert(err == Result.SUCCESS);
            mDepthStencilImage = offscreenImage;
            mConfiguration.Device.GetImageMemoryRequirements(offscreenImage, out MgMemoryRequirements memReqs);

            bool isValidMemoryType = mConfiguration.MemoryProperties.GetMemoryType(
                memReqs.MemoryTypeBits,
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT,
                out uint typeIndex);
            Debug.Assert(isValidMemoryType);
            var memAlloc = new MgMemoryAllocateInfo
            {
                AllocationSize = memReqs.Size,
                MemoryTypeIndex = typeIndex,
            };

            err = mConfiguration.Device.AllocateMemory(memAlloc, null, out IMgDeviceMemory memory);
            Debug.Assert(err == Result.SUCCESS);
            mDepthStencilMemory = memory;
            err = offscreenImage.BindImageMemory(mConfiguration.Device, mDepthStencilMemory, 0);
            Debug.Assert(err == Result.SUCCESS);

            var colorImageView = new MgImageViewCreateInfo
            {
                ViewType = MgImageViewType.TYPE_2D,
                Format = depthFormat,
                SubresourceRange = new MgImageSubresourceRange
                {
                    AspectMask = MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                },
                Image = offscreenImage,                
            };
            err = mConfiguration.Device.CreateImageView(colorImageView, null, out IMgImageView offscreenView);
            Debug.Assert(err == Result.SUCCESS);
            mDepthView = offscreenView;
        }
    }
}
