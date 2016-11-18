using Magnesium;
using OpenTK;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/*
* Vulkan Example - Texture loading (and display) example (including mip maps)
*
* Copyright (C) 2016 by Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

namespace TextureDemo
{
    public class TextureExample : IDisposable
    {
        private MgGraphicsConfigurationManager mManager;

        private float mZoom;

        public TextureExample(MgGraphicsConfigurationManager manager)
        {
            mManager = manager;

            mZoom = -2.5f;
            //rotation = { 0.0f, 15.0f, 0.0f };
            //title = "Vulkan Example - Texturing";
            //enableTextOverlay = true;

            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Color = MgFormat.R8G8B8A8_UINT,
                DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = 1280,
                Height = 720,
            };

            mManager.Initialize(createInfo);
        }
//#define VERTEX_BUFFER_BIND_ID 0

        // Vertex layout for this example
        struct VertexData
        {
            public Vector3 pos;
            public Vector2 uv;
            public Vector3 normal;
        };

	    // Contains all Vulkan objects that are required to store and use a texture
	    // Note that this repository contains a texture loader (vulkantextureloader.h)
	    // that encapsulates texture loading functionality in a class that is used
	    // in subsequent demos
	    struct Texture
        {
            public IMgSampler sampler;
            public IMgImage image;
            public MgImageLayout imageLayout;
            public IMgDeviceMemory deviceMemory;
            public IMgImageView view;
            public MgDescriptorImageInfo descriptor;
            public uint width, height;
            public uint mipLevels;
        };

        Texture texture;

	    struct VerticesInfo
        {

            public MgPipelineVertexInputStateCreateInfo inputState;
            public MgVertexInputBindingDescription[] bindingDescriptions;
            public MgVertexInputAttributeDescription[] attributeDescriptions;
        };

        VerticesInfo vertices;

	    BufferInfo vertexBuffer;
        BufferInfo indexBuffer;
        uint indexCount;

        BufferInfo uniformBufferVS;

        struct UniformBufferObject
        {
            public Matrix4 projection;
            public Matrix4 model;
            public Vector4 viewPos;
            public float lodBias;
        }

        UniformBufferObject uboVS;

        IMgPipeline mSolidPipeline;

	    IMgPipelineLayout mPipelineLayout;
        IMgDescriptorSet[] mDescriptorSets;
        IMgDescriptorSetLayout mDescriptorSetLayout;
        IMgDescriptorPool mDescriptorPool;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool mIsDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;


            ReleaseManagedResources();

            mIsDisposed = true;
        }


        void ReleaseManagedResources()
        {
            // Clean up used Vulkan resources 
            // Note : Inherited destructor cleans up resources stored in base class

            destroyTextureImage(texture);

            if (mManager != null)
            {
                Debug.Assert(mManager.Configuration != null);
                var device = mManager.Configuration.Device;

                if (mSolidPipeline != null)
                {
                    mSolidPipeline.DestroyPipeline(device, null);
                }

                if (mPipelineLayout != null)
                {
                    mPipelineLayout.DestroyPipelineLayout(device, null);
                }

                if (mDescriptorSetLayout != null)
                {
                    mDescriptorSetLayout.DestroyDescriptorSetLayout(device, null);
                }

                if (vertexBuffer != null)
                {
                    vertexBuffer.Destroy();
                }

                if (indexBuffer != null)
                {
                    indexBuffer.Destroy();
                }

                if (uniformBufferVS != null)
                {
                    uniformBufferVS.Destroy();
                }

                if (mManager.Configuration.Partition.CommandPool != null)
                {
                    device.FreeCommandBuffers(
                        mManager.Configuration.Partition.CommandPool,
                        mPresentBuffers);
                }

                if (mDescriptorPool != null)
                {
                    if (mDescriptorSets != null)
                    {
                        device.FreeDescriptorSets(mDescriptorPool, mDescriptorSets);
                    }

                    mDescriptorPool.DestroyDescriptorPool(device, null);
                }                
            }
            mManager.Dispose();
        }

        // Create an image memory barrier for changing the layout of
        // an image and put it into an active command buffer
        void setImageLayout(IMgCommandBuffer cmdBuffer, IMgImage image, MgImageAspectFlagBits aspectMask, MgImageLayout oldImageLayout, MgImageLayout newImageLayout, MgImageSubresourceRange subresourceRange)
        {
            // Create an image barrier object
            var imageMemoryBarrier = new MgImageMemoryBarrier
            {
                OldLayout = oldImageLayout,
                NewLayout = newImageLayout,
                Image = image, 
                SubresourceRange = subresourceRange,
            };


            // Only sets masks for layouts used in this example
            // For a more complete version that can be used with other layouts see IMgTools::setImageLayout

            // Source layouts (old)
            switch (oldImageLayout)
            {
                case MgImageLayout.UNDEFINED:
                    // Only valid as initial layout, memory contents are not preserved
                    // Can be accessed directly, no source dependency required
                    imageMemoryBarrier.SrcAccessMask = 0;
                    break;
                case MgImageLayout.PREINITIALIZED:
                    // Only valid as initial layout for linear images, preserves memory contents
                    // Make sure host writes to the image have been finished
                    imageMemoryBarrier.SrcAccessMask =  MgAccessFlagBits.HOST_WRITE_BIT;
                    break;
                case MgImageLayout.TRANSFER_DST_OPTIMAL:
                    // Old layout is transfer destination
                    // Make sure any writes to the image have been finished
                    imageMemoryBarrier.SrcAccessMask = MgAccessFlagBits.TRANSFER_WRITE_BIT;
                    break;
            }

            // Target layouts (new)
            switch (newImageLayout)
            {
                case MgImageLayout.TRANSFER_SRC_OPTIMAL:
                    // Transfer source (copy, blit)
                    // Make sure any reads from the image have been finished
                    imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_READ_BIT;
                    break;
                case MgImageLayout.TRANSFER_DST_OPTIMAL:
                    // Transfer destination (copy, blit)
                    // Make sure any writes to the image have been finished
                    imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.TRANSFER_WRITE_BIT;
                    break;
                case MgImageLayout.SHADER_READ_ONLY_OPTIMAL:
                    // Shader read (sampler, input attachment)
                    imageMemoryBarrier.DstAccessMask = MgAccessFlagBits.SHADER_READ_BIT;
                    break;
            }

            // Put barrier on top of pipeline
            var srcStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;
            var destStageFlags = MgPipelineStageFlagBits.TOP_OF_PIPE_BIT;

            // Put barrier inside setup command buffer
            cmdBuffer.CmdPipelineBarrier(
                srcStageFlags,
                destStageFlags,
                0,
                null,
                null,
                new[] { imageMemoryBarrier });
        }

    //void loadTexture(std::string fileName, IMgFormat format, bool forceLinearTiling)
    //{
    //#if defined(__ANDROID__)
		  //  // Textures are stored inside the apk on Android (compressed)
		  //  // So they need to be loaded via the asset manager
		  //  AAsset* asset = AAssetManager_open(androidApp->activity->assetManager, fileName.c_str(), AASSET_MODE_STREAMING);
		  //  assert(asset);
		  //  size_t size = AAsset_getLength(asset);
		  //  assert(size > 0);

		  //  void *textureData = malloc(size);
		  //  AAsset_read(asset, textureData, size);
		  //  AAsset_close(asset);

		  //  gli::texture2D tex2D(gli::load((const char*)textureData, size));
    //#else
    //    gli::texture2D tex2D(gli::load(fileName));
    //#endif

    //    assert(!tex2D.empty());

    //    IMgFormatProperties formatProperties;

    //    texture.width = static_cast<uint32_t>(tex2D[0].dimensions().x);
    //    texture.height = static_cast<uint32_t>(tex2D[0].dimensions().y);
    //    texture.mipLevels = static_cast<uint32_t>(tex2D.levels());

    //    // Get device properites for the requested texture format
    //    IMgGetPhysicalDeviceFormatProperties(physicalDevice, format, &formatProperties);

    //    // Only use linear tiling if requested (and supported by the device)
    //    // Support for linear tiling is mostly limited, so prefer to use
    //    // optimal tiling instead
    //    // On most implementations linear tiling will only support a very
    //    // limited amount of formats and features (mip maps, cubemaps, arrays, etc.)
    //    IMgBool32 useStaging = true;

    //    // Only use linear tiling if forced
    //    if (forceLinearTiling)
    //    {
    //        // Don't use linear if format is not supported for (linear) shader sampling
    //        useStaging = !(formatProperties.linearTilingFeatures & IMg_FORMAT_FEATURE_SAMPLED_IMAGE_BIT);
    //    }

    //    IMgMemoryAllocateInfo memAllocInfo = IMgTools::initializers::memoryAllocateInfo();
    //    IMgMemoryRequirements memReqs = { };

    //    if (useStaging)
    //    {
    //        // Create a host-visible staging buffer that contains the raw image data
    //        IMgBuffer stagingBuffer;
    //        IMgDeviceMemory stagingMemory;

    //        IMgBufferCreateInfo bufferCreateInfo = IMgTools::initializers::bufferCreateInfo();
    //        bufferCreateInfo.size = tex2D.size();
    //        // This buffer is used as a transfer source for the buffer copy
    //        bufferCreateInfo.usage = IMg_BUFFER_USAGE_TRANSFER_SRC_BIT;
    //        bufferCreateInfo.sharingMode = IMg_SHARING_MODE_EXCLUSIVE;

    //        Debug.Assert(IMgCreateBuffer(device, &bufferCreateInfo, null, &stagingBuffer));

    //        // Get memory requirements for the staging buffer (alignment, memory type bits)
    //        IMgGetBufferMemoryRequirements(device, stagingBuffer, &memReqs);

    //        memAllocInfo.allocationSize = memReqs.size;
    //        // Get memory type index for a host visible buffer
    //        memAllocInfo.memoryTypeIndex = vulkanDevice->getMemoryType(memReqs.memoryTypeBits, IMg_MEMORY_PROPERTY_HOST_VISIBLE_BIT);

    //        Debug.Assert(IMgAllocateMemory(device, &memAllocInfo, null, &stagingMemory));
    //        Debug.Assert(IMgBindBufferMemory(device, stagingBuffer, stagingMemory, 0));

    //        // Copy texture data into staging buffer
    //        uint8_t* data;
    //        Debug.Assert(IMgMapMemory(device, stagingMemory, 0, memReqs.size, 0, (void**)&data));
    //        memcpy(data, tex2D.data(), tex2D.size());
    //        IMgUnmapMemory(device, stagingMemory);

    //        // Setup buffer copy regions for each mip level
    //        std::vector<IMgBufferImageCopy> bufferCopyRegions;
    //        uint32_t offset = 0;

    //        for (uint32_t i = 0; i < texture.mipLevels; i++)
    //        {
    //            IMgBufferImageCopy bufferCopyRegion = { };
    //            bufferCopyRegion.imageSubresource.aspectMask = IMg_IMAGE_ASPECT_COLOR_BIT;
    //            bufferCopyRegion.imageSubresource.mipLevel = i;
    //            bufferCopyRegion.imageSubresource.baseArrayLayer = 0;
    //            bufferCopyRegion.imageSubresource.layerCount = 1;
    //            bufferCopyRegion.imageExtent.width = static_cast<uint32_t>(tex2D[i].dimensions().x);
    //            bufferCopyRegion.imageExtent.height = static_cast<uint32_t>(tex2D[i].dimensions().y);
    //            bufferCopyRegion.imageExtent.depth = 1;
    //            bufferCopyRegion.bufferOffset = offset;

    //            bufferCopyRegions.push_back(bufferCopyRegion);

    //            offset += static_cast<uint32_t>(tex2D[i].size());
    //        }

    //        // Create optimal tiled target image
    //        IMgImageCreateInfo imageCreateInfo = IMgTools::initializers::imageCreateInfo();
    //        imageCreateInfo.imageType = IMg_IMAGE_TYPE_2D;
    //        imageCreateInfo.format = format;
    //        imageCreateInfo.mipLevels = texture.mipLevels;
    //        imageCreateInfo.arrayLayers = 1;
    //        imageCreateInfo.samples = IMg_SAMPLE_COUNT_1_BIT;
    //        imageCreateInfo.tiling = IMg_IMAGE_TILING_OPTIMAL;
    //        imageCreateInfo.usage = IMg_IMAGE_USAGE_SAMPLED_BIT;
    //        imageCreateInfo.sharingMode = IMg_SHARING_MODE_EXCLUSIVE;
    //        // Set initial layout of the image to undefined
    //        imageCreateInfo.initialLayout = IMg_IMAGE_LAYOUT_UNDEFINED;
    //        imageCreateInfo.extent = { texture.width, texture.height, 1 };
    //        imageCreateInfo.usage = IMg_IMAGE_USAGE_TRANSFER_DST_BIT | IMg_IMAGE_USAGE_SAMPLED_BIT;

    //        Debug.Assert(IMgCreateImage(device, &imageCreateInfo, null, &texture.image));

    //        IMgGetImageMemoryRequirements(device, texture.image, &memReqs);

    //        memAllocInfo.allocationSize = memReqs.size;
    //        memAllocInfo.memoryTypeIndex = vulkanDevice->getMemoryType(memReqs.memoryTypeBits, IMg_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);

    //        Debug.Assert(IMgAllocateMemory(device, &memAllocInfo, null, &texture.deviceMemory));
    //        Debug.Assert(IMgBindImageMemory(device, texture.image, texture.deviceMemory, 0));

    //        IMgCommandBuffer copyCmd = VulkanExampleBase::createCommandBuffer(IMg_COMMAND_BUFFER_LEVEL_PRIMARY, true);

    //        // Image barrier for optimal image

    //        // The sub resource range describes the regions of the image we will be transition
    //        IMgImageSubresourceRange subresourceRange = { };
    //        // Image only contains color data
    //        subresourceRange.aspectMask = IMg_IMAGE_ASPECT_COLOR_BIT;
    //        // Start at first mip level
    //        subresourceRange.baseMipLevel = 0;
    //        // We will transition on all mip levels
    //        subresourceRange.levelCount = texture.mipLevels;
    //        // The 2D texture only has one layer
    //        subresourceRange.layerCount = 1;

    //        // Optimal image will be used as destination for the copy, so we must transfer from our
    //        // initial undefined image layout to the transfer destination layout
    //        setImageLayout(
    //            copyCmd,
    //            texture.image,
    //            IMg_IMAGE_ASPECT_COLOR_BIT,
    //            IMg_IMAGE_LAYOUT_UNDEFINED,
    //            IMg_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
    //            subresourceRange);

    //        // Copy mip levels from staging buffer
    //        IMgCmdCopyBufferToImage(
    //            copyCmd,
    //            stagingBuffer,
    //            texture.image,
    //            IMg_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
    //            static_cast<uint32_t>(bufferCopyRegions.size()),
    //            bufferCopyRegions.data());

    //        // Change texture image layout to shader read after all mip levels have been copied
    //        texture.imageLayout = IMg_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
    //        setImageLayout(
    //            copyCmd,
    //            texture.image,
    //            IMg_IMAGE_ASPECT_COLOR_BIT,
    //            IMg_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
    //            texture.imageLayout,
    //            subresourceRange);

    //        VulkanExampleBase::flushCommandBuffer(copyCmd, queue, true);

    //        // Clean up staging resources
    //        IMgFreeMemory(device, stagingMemory, null);
    //        IMgDestroyBuffer(device, stagingBuffer, null);
    //    }
    //    else
    //    {
    //        // Prefer using optimal tiling, as linear tiling 
    //        // may support only a small set of features 
    //        // depending on implementation (e.g. no mip maps, only one layer, etc.)

    //        IMgImage mappableImage;
    //        IMgDeviceMemory mappableMemory;

    //        // Load mip map level 0 to linear tiling image
    //        IMgImageCreateInfo imageCreateInfo = IMgTools::initializers::imageCreateInfo();
    //        imageCreateInfo.imageType = IMg_IMAGE_TYPE_2D;
    //        imageCreateInfo.format = format;
    //        imageCreateInfo.mipLevels = 1;
    //        imageCreateInfo.arrayLayers = 1;
    //        imageCreateInfo.samples = IMg_SAMPLE_COUNT_1_BIT;
    //        imageCreateInfo.tiling = IMg_IMAGE_TILING_LINEAR;
    //        imageCreateInfo.usage = IMg_IMAGE_USAGE_SAMPLED_BIT;
    //        imageCreateInfo.sharingMode = IMg_SHARING_MODE_EXCLUSIVE;
    //        imageCreateInfo.initialLayout = IMg_IMAGE_LAYOUT_PREINITIALIZED;
    //        imageCreateInfo.extent = { texture.width, texture.height, 1 };
    //        Debug.Assert(IMgCreateImage(device, &imageCreateInfo, null, &mappableImage));

    //        // Get memory requirements for this image 
    //        // like size and alignment
    //        IMgGetImageMemoryRequirements(device, mappableImage, &memReqs);
    //        // Set memory allocation size to required memory size
    //        memAllocInfo.allocationSize = memReqs.size;

    //        // Get memory type that can be mapped to host memory
    //        memAllocInfo.memoryTypeIndex = vulkanDevice->getMemoryType(memReqs.memoryTypeBits, IMg_MEMORY_PROPERTY_HOST_VISIBLE_BIT);

    //        // Allocate host memory
    //        Debug.Assert(IMgAllocateMemory(device, &memAllocInfo, null, &mappableMemory));

    //        // Bind allocated image for use
    //        Debug.Assert(IMgBindImageMemory(device, mappableImage, mappableMemory, 0));

    //        // Get sub resource layout
    //        // Mip map count, array layer, etc.
    //        MgImageSubresource subRes = { };
    //        subRes.aspectMask = IMg_IMAGE_ASPECT_COLOR_BIT;

    //        MgSubresourceLayout subResLayout;
    //        IntPtr data;

    //        // Get sub resources layout 
    //        // Includes row pitch, size offsets, etc.
    //        IMgGetImageSubresourceLayout(device, mappableImage, &subRes, &subResLayout);

    //        // Map image memory
    //        Debug.Assert(IMgMapMemory(device, mappableMemory, 0, memReqs.size, 0, &data));

    //        // Copy image data into memory
    //        memcpy(data, tex2D[subRes.mipLevel].data(), tex2D[subRes.mipLevel].size());

    //        IMgUnmapMemory(device, mappableMemory);

    //        // Linear tiled images don't need to be staged
    //        // and can be directly used as textures
    //        texture.image = mappableImage;
    //        texture.deviceMemory = mappableMemory;
    //        texture.imageLayout = IMg_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;

    //        IMgCommandBuffer copyCmd = VulkanExampleBase::createCommandBuffer(IMg_COMMAND_BUFFER_LEVEL_PRIMARY, true);

    //        // Setup image memory barrier transfer image to shader read layout

    //        // The sub resource range describes the regions of the image we will be transition
    //        IMgImageSubresourceRange subresourceRange = { };
    //        // Image only contains color data
    //        subresourceRange.aspectMask = IMg_IMAGE_ASPECT_COLOR_BIT;
    //        // Start at first mip level
    //        subresourceRange.baseMipLevel = 0;
    //        // Only one mip level, most implementations won't support more for linear tiled images
    //        subresourceRange.levelCount = 1;
    //        // The 2D texture only has one layer
    //        subresourceRange.layerCount = 1;

    //        setImageLayout(
    //            copyCmd,
    //            texture.image,
    //            IMg_IMAGE_ASPECT_COLOR_BIT,
    //            IMg_IMAGE_LAYOUT_PREINITIALIZED,
    //            texture.imageLayout,
    //            subresourceRange);

    //        VulkanExampleBase::flushCommandBuffer(copyCmd, queue, true);
    //    }

    //    // Create sampler
    //    // In Vulkan textures are accessed by samplers
    //    // This separates all the sampling information from the 
    //    // texture data
    //    // This means you could have multiple sampler objects
    //    // for the same texture with different settings
    //    // Similar to the samplers available with OpenGL 3.3
    //    MgSamplerCreateInfo sampler = IMgTools::initializers::samplerCreateInfo();
    //    sampler.magFilter = IMg_FILTER_LINEAR;
    //    sampler.minFilter = IMg_FILTER_LINEAR;
    //    sampler.mipmapMode = IMg_SAMPLER_MIPMAP_MODE_LINEAR;
    //    sampler.addressModeU = IMg_SAMPLER_ADDRESS_MODE_REPEAT;
    //    sampler.addressModeV = IMg_SAMPLER_ADDRESS_MODE_REPEAT;
    //    sampler.addressModeW = IMg_SAMPLER_ADDRESS_MODE_REPEAT;
    //    sampler.mipLodBias = 0.0f;
    //    sampler.compareOp = IMg_COMPARE_OP_NEVER;
    //    sampler.minLod = 0.0f;
    //    // Set max level-of-detail to mip level count of the texture
    //    sampler.maxLod = (useStaging) ? (float)texture.mipLevels : 0.0f;
    //    // Enable anisotropic filtering
    //    // This feature is optional, so we must check if it's supported on the device
    //    if (vulkanDevice->features.samplerAnisotropy)
    //    {
    //        // Use max. level of anisotropy for this example
    //        sampler.maxAnisotropy = vulkanDevice->properties.limits.maxSamplerAnisotropy;
    //        sampler.anisotropyEnable = IMg_TRUE;
    //    }
    //    else
    //    {
    //        // The device does not support anisotropic filtering
    //        sampler.maxAnisotropy = 1.0;
    //        sampler.anisotropyEnable = IMg_FALSE;
    //    }
    //    sampler.borderColor = IMg_BORDER_COLOR_FLOAT_OPAQUE_WHITE;
    //    Debug.Assert(IMgCreateSampler(device, &sampler, null, &texture.sampler));

    //    // Create image view
    //    // Textures are not directly accessed by the shaders and
    //    // are abstracted by image views containing additional
    //    // information and sub resource ranges
    //    MgImageViewCreateInfo view = IMgTools::initializers::imageViewCreateInfo();
    //    view.image = IMg_NULL_HANDLE;
    //    view.viewType = IMg_IMAGE_VIEW_TYPE_2D;
    //    view.format = format;
    //    view.components = { IMg_COMPONENT_SWIZZLE_R, IMg_COMPONENT_SWIZZLE_G, IMg_COMPONENT_SWIZZLE_B, IMg_COMPONENT_SWIZZLE_A };
    //    // The subresource range describes the set of mip levels (and array layers) that can be accessed through this image view
    //    // It's possible to create multiple image views for a single image referring to different (and/or overlapping) ranges of the image
    //    view.subresourceRange.aspectMask = IMg_IMAGE_ASPECT_COLOR_BIT;
    //    view.subresourceRange.baseMipLevel = 0;
    //    view.subresourceRange.baseArrayLayer = 0;
    //    view.subresourceRange.layerCount = 1;
    //    // Linear tiling usually won't support mip maps
    //    // Only set mip map count if optimal tiling is used
    //    view.subresourceRange.levelCount = (useStaging) ? texture.mipLevels : 1;
    //    view.image = texture.image;
    //    Debug.Assert(IMgCreateImageView(device, &view, null, &texture.view));

    //    // Fill image descriptor image info that can be used during the descriptor set setup
    //    texture.descriptor.imageLayout = IMg_IMAGE_LAYOUT_GENERAL;
    //    texture.descriptor.imageView = texture.view;
    //    texture.descriptor.sampler = texture.sampler;
    //}

        // Free all Vulkan resources used a texture object
        void destroyTextureImage(Texture texture)
        {
            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            texture.view.DestroyImageView(device, null);
            texture.image.DestroyImage(device, null);
            texture.sampler.DestroySampler(device, null);
            texture.deviceMemory.FreeMemory(device, null);
        }

        IMgCommandBuffer[] drawCmdBuffers;
        void buildCommandBuffers()
        {
            var cmdBufInfo = new MgCommandBufferBeginInfo
            {

            };

            var renderPassBeginInfo = new MgRenderPassBeginInfo
            {
                RenderPass = mManager.Graphics.Renderpass,
                RenderArea = new MgRect2D
                {
                    Offset = new MgOffset2D { X = 0, Y = 0},
                    Extent = new MgExtent2D {
                        Width = mManager.Width,
                        Height = mManager.Height
                    },                    
                },
                ClearValues = new MgClearValue[]
                {
                    new MgClearValue { Color = new MgClearColorValue() },
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1f, 0) }
                },
            };

            var cmdBufferCount = (uint) mManager.Graphics.Framebuffers.Length;

            var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = cmdBufferCount,
                CommandPool = mManager.Configuration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            var err = device.AllocateCommandBuffers(cmdBufAllocateInfo, drawCmdBuffers);
            Debug.Assert(err == Result.SUCCESS);

            for (var i = 0; i < cmdBufferCount; ++i)
            {
                // Set target frame buffer
                renderPassBeginInfo.Framebuffer = mManager.Graphics.Framebuffers[i];

                var cmdBuf = drawCmdBuffers[i];

                err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo,  MgSubpassContents.INLINE);

                var viewport = new MgViewport
                {
                    Width = mManager.Width,
                    Height = mManager.Height,
                    MinDepth = 0f,
                    MaxDepth = 1f,
                };

                cmdBuf.CmdSetViewport(0, new[] { viewport });

                var scissor = new MgRect2D {
                    Extent = new MgExtent2D
                    {
                        Height = mManager.Height,
                        Width = mManager.Width,                        
                    },
                    Offset = new MgOffset2D
                    {
                        X = 0,
                        Y = 0,
                    }
                };
                cmdBuf.CmdSetScissor(0, new [] { scissor });

                cmdBuf.CmdBindDescriptorSets( MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, 1, mDescriptorSets, null);
                cmdBuf.CmdBindPipeline( MgPipelineBindPoint.GRAPHICS, mSolidPipeline);

                cmdBuf.CmdBindVertexBuffers(0, new[] { vertexBuffer.InternalBuffer }, new[] { 0UL });
                cmdBuf.CmdBindIndexBuffer(indexBuffer.InternalBuffer, 0, MgIndexType.UINT32);

                cmdBuf.CmdDrawIndexed(indexCount, 1, 0, 0, 0);

                cmdBuf.CmdEndRenderPass();

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        IMgCommandBuffer[] mPresentBuffers;
        IMgCommandBuffer mPrePresentCmdBuffer;
        IMgCommandBuffer mPostPresentCmdBuffer;
        // Command buffers for submitting present barriers
        void setupPresentationBarries()
        {
            var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = 2,
                CommandPool = mManager.Configuration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            mPresentBuffers = new IMgCommandBuffer[2];

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            var err = device.AllocateCommandBuffers(cmdBufAllocateInfo, mPresentBuffers);
            Debug.Assert(err == Result.SUCCESS);

            // Pre present
            mPrePresentCmdBuffer = mPresentBuffers[0];

            // Post present
            mPostPresentCmdBuffer = mPresentBuffers[1];
        }

        void draw()
        {
            // VulkanExampleBase::prepareFrame();
            var currentBuffer = mManager.Layer.BeginDraw(mPostPresentCmdBuffer, null);

            // Command buffer to be sumitted to the queue

            var submitInfos = new []
            {
                new MgSubmitInfo
                {
                    CommandBuffers = new [] {
                        drawCmdBuffers[currentBuffer]
                    },
                }
            };
            
            // Submit to queue
            var err = mManager.Configuration.Queue.QueueSubmit(submitInfos, null);  
            Debug.Assert(err == Result.SUCCESS);

            //VulkanExampleBase::submitFrame();
            mManager.Layer.EndDraw(new[] { currentBuffer }, mPrePresentCmdBuffer, null);
        }

        void generateQuad()
        {
            // Setup vertices for a single uv-mapped quad made from two triangles
            VertexData[] quadCorners =
            {
                new VertexData
                {
                    pos = new Vector3(  1.0f,  1.0f, 0.0f ),
                    uv = new Vector2( 1.0f, 1.0f ),
                    normal = new Vector3( 0.0f, 0.0f, 1.0f )
                },

                new VertexData
                {
                    pos = new Vector3(   -1.0f,  1.0f, 0.0f ),
                    uv = new Vector2(  0.0f, 1.0f ),
                    normal = new Vector3( 0.0f, 0.0f, 1.0f  )
                },

                new VertexData
                {
                    pos = new Vector3(  -1.0f, -1.0f, 0.0f ),
                    uv = new Vector2( 0.0f, 0.0f ),
                    normal = new Vector3( 0.0f, 0.0f, 1.0f )
                },


                new VertexData
                {
                    pos = new Vector3( 1.0f, -1.0f, 0.0f  ),
                    uv = new Vector2( 1.0f, 0.0f ),
                    normal = new Vector3( 0.0f, 0.0f, 1.0f )
                },
            };

            // Setup indices
            var indices = new uint[] { 0, 1, 2, 2, 3, 0 };
            indexCount = (uint) indices.Length;

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            // Create buffers
            // For the sake of simplicity we won't stage the vertex data to the gpu memory

            // Vertex buffer
            {
                var bufferSize = (uint)(Marshal.SizeOf<VertexData>() * quadCorners.Length);
                vertexBuffer = new BufferInfo(
                    mManager.Configuration.Partition,
                    MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    bufferSize);
                vertexBuffer.SetData<VertexData>(bufferSize, quadCorners, 0, quadCorners.Length);
            }


            // Index buffer
            {
                var bufferSize = indexCount * sizeof(uint);
                indexBuffer = new BufferInfo(
                    mManager.Configuration.Partition,
                    MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    bufferSize);
                indexBuffer.SetData(bufferSize, indices, 0, (int) indexCount);

            }
        }

        void setupVertexDescriptions()
        {
            const uint VERTEX_BUFFER_BIND_ID = 0;

             // Binding description
            vertices.bindingDescriptions = new[]
            {
                new MgVertexInputBindingDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    InputRate = MgVertexInputRate.VERTEX,
                    Stride = (uint) Marshal.SizeOf<VertexData>(),
                }
            };

            // Attribute descriptions
            // Describes memory layout and shader positions
            vertices.attributeDescriptions = new MgVertexInputAttributeDescription[]
            {
                // Location 0 : Position
                new MgVertexInputAttributeDescription
                {                    
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 0,
                    Format = MgFormat.R32G32B32_SFLOAT,
                    Offset = (uint) Marshal.OffsetOf<VertexData>("pos"),
                },
                // Location 1 : Texture coordinates
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 1,
                    Format = MgFormat.R32G32_SFLOAT,
                    Offset = (uint) Marshal.OffsetOf<VertexData>("uv"),
                },
                // Location 2 : Vertex normal
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 2,
                    Format = MgFormat.R32G32B32_SFLOAT,
                    Offset = (uint) Marshal.OffsetOf<VertexData>("normal"),
                },
            };

            vertices.inputState = new MgPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = vertices.bindingDescriptions,
                VertexAttributeDescriptions = vertices.attributeDescriptions,
            };
        }

        void setupDescriptorPool()
        {
            // Example uses one ubo and one image sampler
            var descriptorPoolInfo = new MgDescriptorPoolCreateInfo
            {
                MaxSets = 2,
                PoolSizes = new[]
                {
                    new MgDescriptorPoolSize
                    {
                        Type = MgDescriptorType.UNIFORM_BUFFER,
                        DescriptorCount = 1,
                    },
                    new MgDescriptorPoolSize
                    {
                        Type = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                        DescriptorCount = 1,
                    },
                },
            };

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);
            var err = device.CreateDescriptorPool(descriptorPoolInfo, null, out mDescriptorPool);
            Debug.Assert(err == Result.SUCCESS);
        }

        void setupDescriptorSetLayout()
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new MgDescriptorSetLayoutBinding[]
                {
			        // Binding 0 : Vertex shader uniform buffer
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorType = MgDescriptorType.UNIFORM_BUFFER,
                        StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
                        Binding = 0,
                        DescriptorCount = 1,
                    },
                    // Binding 1 : Fragment shader image sampler
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                        StageFlags = MgShaderStageFlagBits.FRAGMENT_BIT,
                        Binding = 1,
                        DescriptorCount = 1,
                    },
                },                
            };

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            var err = device.CreateDescriptorSetLayout(descriptorLayout, null, out mDescriptorSetLayout);
            Debug.Assert(err == Result.SUCCESS);

            MgPipelineLayoutCreateInfo pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                SetLayouts = new [] { mDescriptorSetLayout },
            };  
        
            err = device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out mPipelineLayout);
            Debug.Assert(err == Result.SUCCESS);
        }

        void setupDescriptorSet()
        {
            var allocInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = mDescriptorPool,
                DescriptorSetCount = 1,
                SetLayouts = new IMgDescriptorSetLayout[] {mDescriptorSetLayout },
            };

            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);

            var err = device.AllocateDescriptorSets(allocInfo, out mDescriptorSets);
            Debug.Assert(err == Result.SUCCESS);

            var writeDescriptorSets = new []
            {
			    // Binding 0 : Vertex shader uniform buffer
                new MgWriteDescriptorSet
                {
                    DstSet = mDescriptorSets[0],
                    DescriptorType = MgDescriptorType.UNIFORM_BUFFER,                    
                    DstBinding = 0,
                    DescriptorCount = 1,
                    BufferInfo = new []
                    {
                        uniformBufferVS.Descriptor,
                    }
                },
                // Binding 1 : Fragment shader texture sampler
                new MgWriteDescriptorSet
                {
                    DstSet = mDescriptorSets[0],
                    DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                    DstBinding = 1,
                    DescriptorCount = 1,
                    ImageInfo = new []
                    {
                        texture.descriptor,
                    }
                },
            };

            device.UpdateDescriptorSets(writeDescriptorSets, null);
        }

        void preparePipelines()
        {
            Debug.Assert(mManager.Configuration != null);
            var device = mManager.Configuration.Device;

            using (var vertFs = System.IO.File.OpenRead("shaders/texture/texture.vert.spv"))
            using (var fragFs = System.IO.File.OpenRead("shaders/texture/texture.frag.spv"))
            {
                // Load shaders
                IMgShaderModule vertSM;
                {
                    var vsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = vertFs,
                        CodeSize = new UIntPtr((ulong)vertFs.Length),
                    };
                    // shaderStages[0] = loadShader(getAssetPath() + "shaders/texture/texture.vert.spv", IMg_SHADER_STAGE_VERTEX_BIT);
                    var localErr = device.CreateShaderModule(vsCreateInfo, null, out vertSM);
                    Debug.Assert(localErr == Result.SUCCESS);
                }

                IMgShaderModule fragSM;
                {
                    var fsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = fragFs,
                        CodeSize = new UIntPtr((ulong) fragFs.Length),
                    };
                    //shaderStages[1] = loadShader(getAssetPath() + "shaders/texture/texture.frag.spv", IMg_SHADER_STAGE_FRAGMENT_BIT);
                    var localErr = device.CreateShaderModule(fsCreateInfo, null, out fragSM);
                    Debug.Assert(localErr == Result.SUCCESS);
                }

                var pipelineCreateInfo = new MgGraphicsPipelineCreateInfo
                {
                    Stages = new MgPipelineShaderStageCreateInfo[]
                    {
                        new MgPipelineShaderStageCreateInfo
                        {
                            Module = vertSM,
                            Stage = MgShaderStageFlagBits.VERTEX_BIT,
                            Name = "vertFunc",
                        },
                        new MgPipelineShaderStageCreateInfo
                        {
                            Module = fragSM,
                            Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
                            Name = "fragFunc",
                        }
                    },

                    Layout = mPipelineLayout,
                    RenderPass = mManager.Graphics.Renderpass,
                    VertexInputState = vertices.inputState,

                    InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                    {
                        Topology = MgPrimitiveTopology.TRIANGLE_LIST,
                        PrimitiveRestartEnable = false,
                    },

                    RasterizationState = new MgPipelineRasterizationStateCreateInfo
                    {
                        PolygonMode = MgPolygonMode.FILL,
                        CullMode = MgCullModeFlagBits.NONE,
                        FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
                    },

                    ColorBlendState = new MgPipelineColorBlendStateCreateInfo
                    {
                        Attachments = new MgPipelineColorBlendAttachmentState[]
                        {
                    new MgPipelineColorBlendAttachmentState
                    {
                        ColorWriteMask = MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
                        BlendEnable = false,
                    },
                        }
                    },

                    MultisampleState = new MgPipelineMultisampleStateCreateInfo
                    {
                        RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
                    },

                    // pipelineCreateInfo.pViewportState = &viewportState;
                    // MgPipelineViewportStateCreateInfo viewportState =
                    //    IMgTools::initializers::pipelineViewportStateCreateInfo(1, 1, 0);

                    DepthStencilState = new MgPipelineDepthStencilStateCreateInfo
                    {
                        DepthTestEnable = true,
                        DepthWriteEnable = true,
                        DepthCompareOp = MgCompareOp.LESS_OR_EQUAL,
                        Front = new MgStencilOpState
                        {
                            CompareOp = MgCompareOp.ALWAYS,
                        },
                        Back = new MgStencilOpState
                        {
                            CompareOp = MgCompareOp.ALWAYS,
                        },
                    },

                    DynamicState = new MgPipelineDynamicStateCreateInfo
                    {
                        DynamicStates = new[] {
                            MgDynamicState.VIEWPORT,
                            MgDynamicState.SCISSOR
                        },
                    }
                };

                IMgPipeline[] pipelines;
                var err = device.CreateGraphicsPipelines(null, new[] { pipelineCreateInfo }, null, out pipelines);
                Debug.Assert(err == Result.SUCCESS);
                mSolidPipeline = pipelines[0];
            }
        }

        // Prepare and initialize uniform buffer containing shader uniforms
        void prepareUniformBuffers()
        {
            // Vertex shader uniform buffer block
            uniformBufferVS = new BufferInfo(
                mManager.Configuration.Partition,
                MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                (uint) Marshal.SizeOf<UniformBufferObject>()
                );
            updateUniformBuffers();
        }

        void updateUniformBuffers()
        {
            // Vertex shader
            uboVS.projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60.0f),
                (float) mManager.Width / (float) mManager.Height,
                0.001f,
                256.0f
            );                
 
            //glm::mat4 viewMatrix = glm::translate(glm::mat4(), glm::vec3(0.0f, 0.0f, zoom));

            //uboVS.model = viewMatrix * glm::translate(glm::mat4(), cameraPos);
            //uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            //uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            //uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));

            uboVS.model = Matrix4.Identity;

            uboVS.viewPos = new Vector4(0.0f, 0.0f, -mZoom, 0.0f);

            var bufferSize = (uint) Marshal.SizeOf<UniformBufferObject>();
            uniformBufferVS.SetData<UniformBufferObject>(bufferSize, new[] { uboVS }, 0, 1);
        }

        void prepare()
        {
            //VulkanExampleBase::prepare();
            generateQuad();
            setupVertexDescriptions();
            prepareUniformBuffers();
            //loadTexture(
            //    getAssetPath() + "textures/pattern_02_bc2.ktx",
            //    IMg_FORMAT_BC2_UNORM_BLOCK,
            //    false);
            setupDescriptorSetLayout();
            preparePipelines();
            setupDescriptorPool();
            setupDescriptorSet();
            buildCommandBuffers();
            mPrepared = true;
        }

        private bool mPrepared = false;
        public void render()
        {
            if (!mPrepared)
                return;
            draw();
        }

        void viewChanged()
        {
            updateUniformBuffers();
        }

        void changeLodBias(float delta)
        {
            uboVS.lodBias += delta;
            if (uboVS.lodBias < 0.0f)
            {
                uboVS.lodBias = 0.0f;
            }
            if (uboVS.lodBias > texture.mipLevels)
            {
                uboVS.lodBias = (float)texture.mipLevels;
            }
            updateUniformBuffers();
        }

        void keyPressed(uint keyCode)
        {
            switch (keyCode)
            {
                case KEY_KPADD:
                case GAMEPAD_BUTTON_R1:
                    changeLodBias(0.1f);
                    break;
                case KEY_KPSUB:
                case GAMEPAD_BUTTON_L1:
                    changeLodBias(-0.1f);
                    break;
            }
        }
    }
}
