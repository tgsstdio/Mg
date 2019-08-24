using System;
using System.Diagnostics;

namespace Magnesium.Toolkit
{
    public class MgDefaultGraphicsDeviceContext : IMgGraphicsDeviceContext
    {
        readonly IMgGraphicsConfiguration mGraphicsConfiguration;
        readonly IMgImageTools mImageTools;
        public MgDefaultGraphicsDeviceContext(
            IMgGraphicsConfiguration configuration,
            IMgImageTools imageTools)
        {
            mGraphicsConfiguration = configuration;
            mImageTools = imageTools;
        }

        MgPhysicalDeviceProperties mProperties;
        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {
            Debug.Assert(mGraphicsConfiguration.Partition != null);
            mGraphicsConfiguration.Partition.PhysicalDevice.GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties prop);
            mProperties = prop;

            //mGraphicsConfiguration.Partition.PhysicalDevice.GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties);
            //mDeviceMemoryProperties = pMemoryProperties;

            // Check if device supports requested sample count for color and depth frame buffer
            if (
                (mProperties.Limits.FramebufferColorSampleCounts < createInfo.Samples)
                || (mProperties.Limits.FramebufferDepthSampleCounts < createInfo.Samples))
            {
                throw new ArgumentOutOfRangeException("createInfo.Samples",
                    "MgDefaultDepthStencilBuffer : This physical device cannot fulfil the requested sample count for BOTH color and depth frame buffer");
            }
        }

        private IMgImageView mDepthStencilImageView;
        public void ReleaseDepthStencil()
        {
            if (mDepthStencilImageView != null)
            {
                mDepthStencilImageView.DestroyImageView(mGraphicsConfiguration.Partition.Device, null);
                mDepthStencilImageView = null;
            }
            if (mImage != null)
            {
                mImage.DestroyImage(mGraphicsConfiguration.Partition.Device, null);
                mImage = null;
            }
            if (mDeviceMemory != null)
            {
                mDeviceMemory.FreeMemory(mGraphicsConfiguration.Partition.Device, null);
                mDeviceMemory = null;
            }
        }

        public void SetupContext(MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
        {

        }

        private IMgImage mImage;
        private IMgDeviceMemory mDeviceMemory;
        public IMgImageView SetupDepthStencil(MgGraphicsDeviceCreateInfo createInfo, IMgCommandBuffer setupCmdBuffer, MgFormat depthFormat)
        {
            var image = new MgImageCreateInfo
            {
                ImageType = MgImageType.TYPE_2D,
                Format = depthFormat,
                Extent = new MgExtent3D
                {
                    Width = createInfo.Width,
                    Height = createInfo.Height,
                    Depth = 1
                },
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = createInfo.Samples,
                Tiling = MgImageTiling.OPTIMAL,
                // TODO : multisampled uses MgImageUsageFlagBits.TRANSIENT_ATTACHMENT_BIT | MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT;
                Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT | MgImageUsageFlagBits.TRANSFER_SRC_BIT,
                Flags = 0,
            };
            var mem_alloc = new MgMemoryAllocateInfo
            {
                AllocationSize = 0,
                MemoryTypeIndex = 0,
            };
            MgMemoryRequirements memReqs;

            Debug.Assert(mGraphicsConfiguration.Partition != null);

            MgResult err;
            {
                IMgImage dsImage;
                err = mGraphicsConfiguration.Partition.Device.CreateImage(image, null, out dsImage);
                Debug.Assert(err == MgResult.SUCCESS, err + " != Result.SUCCESS");
                mImage = dsImage;
            }
            mGraphicsConfiguration.Partition.Device.GetImageMemoryRequirements(mImage, out memReqs);
            mem_alloc.AllocationSize = memReqs.Size;
            bool memoryTypeFound = mGraphicsConfiguration.MemoryProperties.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out uint memTypeIndex);
            Debug.Assert(memoryTypeFound);
            mem_alloc.MemoryTypeIndex = memTypeIndex;
            {
                IMgDeviceMemory dsDeviceMemory;
                err = mGraphicsConfiguration.Partition.Device.AllocateMemory(mem_alloc, null, out dsDeviceMemory);
                Debug.Assert(err == MgResult.SUCCESS, err + " != Result.SUCCESS");
                mDeviceMemory = dsDeviceMemory;
            }
            err = mImage.BindImageMemory(mGraphicsConfiguration.Partition.Device, mDeviceMemory, 0);
            Debug.Assert(err == MgResult.SUCCESS, err + " != Result.SUCCESS");
            mImageTools.SetImageLayout(setupCmdBuffer, mImage, MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT, MgImageLayout.UNDEFINED, MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL);
            var depthStencilView = new MgImageViewCreateInfo
            {
                Image = mImage,
                ViewType = MgImageViewType.TYPE_2D,
                Format = depthFormat,
                Flags = 0,
                SubresourceRange = new MgImageSubresourceRange
                {
                    AspectMask = MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1,
                },
            };
            {
                IMgImageView dsView;
                err = mGraphicsConfiguration.Partition.Device.CreateImageView(depthStencilView, null, out dsView);
                Debug.Assert(err == MgResult.SUCCESS, err + " != Result.SUCCESS");
                mDepthStencilImageView = dsView;

                return dsView;
            }
        }
    }
}

