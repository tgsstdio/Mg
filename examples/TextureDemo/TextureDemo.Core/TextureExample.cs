using Magnesium;
using Magnesium.Ktx;
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

namespace TextureDemo.Core
{
    public class TextureExample : IDisposable
    {
        private MgGraphicsConfigurationManager mManager;
        private MgPhysicalDeviceFeatures mFeatures;
        private MgPhysicalDeviceProperties mPhysicalDeviceProperties;
        private ITextureDemoContent mContent;
        private IMgTextureGenerator mOptimizer;

        private float mZoom;

        public TextureExample(
            ITextureDemoContent content,
            IMgTextureGenerator optimizer,
            MgGraphicsConfigurationManager manager
            )
        {
            mContent = content;
            mOptimizer = optimizer;
            mManager = manager;

            mZoom = -2.5f;
            //rotation = { 0.0f, 15.0f, 0.0f };
            //title = "Vulkan Example - Texturing";
            //enableTextOverlay = true;

            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                //Color = MgFormat.R8G8B8A8_UINT,
                //DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = 1280,
                Height = 720,
            };

            mManager.Initialize(createInfo);

            mManager.Configuration.Partition.PhysicalDevice.GetPhysicalDeviceProperties(out mPhysicalDeviceProperties);
            mManager.Configuration.Partition.PhysicalDevice.GetPhysicalDeviceFeatures(out mFeatures);

            prepare();
        }
//#define VERTEX_BUFFER_BIND_ID 0

        // Vertex layout for this example
        [StructLayout(LayoutKind.Sequential)]
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

        [StructLayout(LayoutKind.Sequential)]
        struct UniformBufferObject
        {
            public Vector3 viewPos;
            public float lodBias;
            public Matrix4 projection;
            public Matrix4 model;
        }

        UniformBufferObject uboVS;

        IMgPipeline mSolidPipeline;

	    IMgPipelineLayout mPipelineLayout;
        IMgDescriptorSet[] mDescriptorSets;
        IMgDescriptorSetLayout mDescriptorSetLayout;
        IMgDescriptorPool mDescriptorPool;

        ~TextureExample()
        {
            Dispose(false);
        }

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
                    mSolidPipeline = null;
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

                    if (drawCmdBuffers != null)
                    {
                        device.FreeCommandBuffers(
                            mManager.Configuration.Partition.CommandPool,
                            drawCmdBuffers);
                    }
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

        void loadTexture(MgFormat format)
        {
            IMgImageTools imageTools = new Magnesium.MgImageTools();
     
            IKTXTextureLoader loader = new KTXTextureManager(mOptimizer, mManager.Configuration);
            using (var fs = mContent.OpenTextureFile())
            {
                var result = loader.Load(fs);

                // Create sampler
                // In Vulkan textures are accessed by samplers
                // This separates all the sampling information from the 
                // texture data
                // This means you could have multiple sampler objects
                // for the same texture with different settings
                // Similar to the samplers available with OpenGL 3.3
                var samplerCreateInfo = new MgSamplerCreateInfo
                {
                    MagFilter = MgFilter.LINEAR,
                    MinFilter = MgFilter.LINEAR,
                    MipmapMode = MgSamplerMipmapMode.LINEAR,
                    AddressModeU = MgSamplerAddressMode.REPEAT,
                    AddressModeV = MgSamplerAddressMode.REPEAT,
                    AddressModeW = MgSamplerAddressMode.REPEAT,
                    MipLodBias = 0.0f,
                    CompareOp = MgCompareOp.NEVER,
                    MinLod = 0.0f,
                    BorderColor = MgBorderColor.FLOAT_OPAQUE_WHITE,
                };

                // Set max level-of-detail to mip level count of the texture
                var mipLevels = (uint)result.Source.Mipmaps.Length;
                samplerCreateInfo.MaxLod = (float)mipLevels;
                // Enable anisotropic filtering
                // This feature is optional, so we must check if it's supported on the device
                //mManager.Configuration.Partition.


                if (mFeatures.SamplerAnisotropy)
                {
                    // Use max. level of anisotropy for this example
                    samplerCreateInfo.MaxAnisotropy = mPhysicalDeviceProperties.Limits.MaxSamplerAnisotropy;
                    samplerCreateInfo.AnisotropyEnable = true;
                }
                else
                {
                    // The device does not support anisotropic filtering
                    samplerCreateInfo.MaxAnisotropy = 1.0f;
                    samplerCreateInfo.AnisotropyEnable = false;
                }

                IMgSampler sampler;
                var err = mManager.Configuration.Device.CreateSampler(samplerCreateInfo, null, out sampler);
                Debug.Assert(err == Result.SUCCESS);

                // Create image view
                // Textures are not directly accessed by the shaders and
                // are abstracted by image views containing additional
                // information and sub resource ranges
                var viewCreateInfo = new MgImageViewCreateInfo
                {
                    Image = result.TextureInfo.Image,
                    // TODO : FETCH VIEW TYPE FROM KTX 
                    ViewType = MgImageViewType.TYPE_2D,
                    Format = result.Source.Format,
                    Components = new MgComponentMapping
                    {
                        R = MgComponentSwizzle.R,
                        G = MgComponentSwizzle.G,
                        B = MgComponentSwizzle.B,
                        A = MgComponentSwizzle.A,
                    },
                    // The subresource range describes the set of mip levels (and array layers) that can be accessed through this image view
                    // It's possible to create multiple image views for a single image referring to different (and/or overlapping) ranges of the image
                    SubresourceRange = new MgImageSubresourceRange
                    {
                        AspectMask = MgImageAspectFlagBits.COLOR_BIT,
                        BaseMipLevel = 0,
                        BaseArrayLayer = 0,
                        LayerCount = 1,
                        LevelCount = mipLevels,
                    }
                };

                IMgImageView view;
                err = mManager.Configuration.Device.CreateImageView(viewCreateInfo, null, out view);
                Debug.Assert(err == Result.SUCCESS);

                texture = new Texture
                {
                    image = result.TextureInfo.Image,
                    imageLayout = result.TextureInfo.ImageLayout,
                    deviceMemory = result.TextureInfo.DeviceMemory,
                    sampler = sampler,
                    width = result.Source.Width,
                    height = result.Source.Height,
                    mipLevels = mipLevels,
                    view = view,
                    descriptor = new MgDescriptorImageInfo
                    {
                        ImageLayout = MgImageLayout.GENERAL,
                        ImageView = view,
                        Sampler = sampler,
                    }
                };
            }
        }

        // Free all Vulkan resources used a texture object
        void destroyTextureImage(Texture texture)
        {
            var device = mManager.Configuration.Device;
            Debug.Assert(device != null);
         
            if (texture.view != null)
                texture.view.DestroyImageView(device, null);

            if (texture.image != null)
                texture.image.DestroyImage(device, null);

            if (texture.sampler != null)
                texture.sampler.DestroySampler(device, null);

            if (texture.deviceMemory != null)
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
                    Offset = new MgOffset2D { X = 0, Y = 0 },
                    Extent = new MgExtent2D {
                        Width = mManager.Width,
                        Height = mManager.Height
                        
                    },                    
                },                
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(mManager.Swapchains.Format, new MgColor4f(1f, 0f, 1f, 0f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1f, 0) }
                },                
            };

            var cmdBufferCount = (uint) mManager.Graphics.Framebuffers.Length;
            drawCmdBuffers = new IMgCommandBuffer[cmdBufferCount];

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
                    MaxDepth = 2f,
                };

                cmdBuf.CmdSetViewport(0, new[] { viewport });

                //var scissor = new MgRect2D {
                //    Extent = new MgExtent2D
                //    {
                //        Height = mManager.Height,
                //        Width = mManager.Width,                        
                //    },
                //    Offset = new MgOffset2D
                //    {
                //        X = 0,
                //        Y = 0,
                //    }
                //};
                cmdBuf.CmdSetScissor(0, new [] { mManager.Graphics.Scissor });

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
        void setupPresentationBarriers()
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
            Debug.Assert(mPostPresentCmdBuffer != null);
            Debug.Assert(mPrePresentCmdBuffer != null);

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
                    pos = new Vector3(  1f,  1f, 0f ),
                    uv = new Vector2( 1.0f, 1.0f ),
                    normal = new Vector3( 0.0f, 0.0f, -1.0f )
                },

                new VertexData
                {
                    pos = new Vector3(   -1.0f,  1.0f, 0f ),
                    uv = new Vector2(  0.0f, 1.0f ),
                    normal = new Vector3( 0.0f, 0.0f, -1.0f  )
                },

                new VertexData
                {
                    pos = new Vector3(  -1.0f, -1.0f, 0f ),
                    uv = new Vector2( 0.0f, 0.0f ),
                    normal = new Vector3( 0.0f, 0.0f, -1.0f )
                },


                new VertexData
                {
                    pos = new Vector3( 1.0f, -1.0f, 0f ),
                    uv = new Vector2( 1.0f, 0.0f ),
                    normal = new Vector3( 0.0f, 0.0f, -1.0f )
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
                var bufferSize = (uint)(Marshal.SizeOf(typeof(VertexData)) * quadCorners.Length);
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
                    Stride = (uint) Marshal.SizeOf(typeof(VertexData)),
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
                    Offset = (uint) Marshal.OffsetOf(typeof(VertexData), "pos"),
                },
                // Location 1 : Texture coordinates
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 1,
                    Format = MgFormat.R32G32_SFLOAT,
                    Offset = (uint) Marshal.OffsetOf(typeof(VertexData),"uv"),
                },
                // Location 2 : Vertex normal
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 2,
                    Format = MgFormat.R32G32B32_SFLOAT,
                    Offset = (uint) Marshal.OffsetOf(typeof(VertexData),"normal"),
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

            using (var vertFs = mContent.OpenVertexShader())
            using (var fragFs = mContent.OpenFragmentShader())
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

                    //ViewportState = new MgPipelineViewportStateCreateInfo
                    //{
                    //    Scissors = new []
                    //    {
                    //        mManager.Graphics.Scissor,
                    //    },
                    //    Viewports = new []
                    //    {
                    //        mManager.Graphics.CurrentViewport,
                    //    }
                    //},

                    DepthStencilState = new MgPipelineDepthStencilStateCreateInfo
                    {
                        DepthTestEnable = true,
                        DepthWriteEnable = true,
                        DepthCompareOp = MgCompareOp.LESS_OR_EQUAL,
                        DepthBoundsTestEnable = false,
                        Back = new MgStencilOpState
                        {
                            FailOp = MgStencilOp.KEEP,
                            PassOp = MgStencilOp.KEEP,
                            CompareOp = MgCompareOp.ALWAYS,
                        },
                        StencilTestEnable = false,
                        Front = new MgStencilOpState
                        {
                            FailOp = MgStencilOp.KEEP,
                            PassOp = MgStencilOp.KEEP,
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

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">An angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        static float DegreesToRadians(float degrees)
        {
            const double degToRad = System.Math.PI / 180.0;
            return (float)(degrees * degToRad);
        }

        // Prepare and initialize uniform buffer containing shader uniforms
        void prepareUniformBuffers()
        {
            // Vertex shader uniform buffer block
            uniformBufferVS = new BufferInfo(
                mManager.Configuration.Partition,
                MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                (uint) Marshal.SizeOf(typeof(UniformBufferObject))
                );
            updateUniformBuffers();
        }

       // float sum = 0f;
        void updateUniformBuffers()
        {
            // Vertex shader
            uboVS.projection = Matrix4.CreatePerspectiveFieldOfView(
                DegreesToRadians(60.0f),
                ((float)mManager.Width / (float)mManager.Height),
                0.001f, 256.0f
            );
            // uboVS.projection = Matrix4.Identity;
            //uboVS.projection = Matrix4.CreateTranslation(1f,0f,0.5f);

            //var viewMatrix = Matrix4.CreateTranslation( 0f, 0f, mZoom);

            ////uboVS.model = viewMatrix * glm::translate(glm::mat4(), cameraPos);
            //var rotateX = Matrix4.RotateX(rotation.x);
            //var rotateY = Matrix4.RotateY(rotation.y);
            //var rotateZ = Matrix4.RotateZ(rotation.Z);
            ////uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            ////uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            ////uboVS.model = glm::rotate(uboVS.model, glm::radians(rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));
            //uboVS.model = rotateZ * rotateY * rotateX * viewMatrix;

            uboVS.lodBias = 0.5f;

            uboVS.model = Matrix4.Identity;

            uboVS.viewPos = new Vector3(0f, 0f, mZoom);
            //sum += 0.001f;

            //if (sum > 1f)
            //{
            //    sum = 0;
            //}

            var bufferSize = (uint) Marshal.SizeOf(typeof(UniformBufferObject));
            uniformBufferVS.SetData<UniformBufferObject>(bufferSize, new[] { uboVS }, 0, 1);
        }

        void prepare()
        {
            //VulkanExampleBase::prepare();
            generateQuad();
            setupVertexDescriptions();
            prepareUniformBuffers();
            loadTexture(MgFormat.BC2_UNORM_BLOCK);
            setupDescriptorSetLayout();
            preparePipelines();
            setupDescriptorPool();
            setupDescriptorSet();
            buildCommandBuffers();
            setupPresentationBarriers();
            mPrepared = true;
        }

        private bool mPrepared = false;
        public void Render()
        {
            if (!mPrepared)
                return;
           // updateUniformBuffers();
            draw();
        }

        void viewChanged()
        {
            updateUniformBuffers();
        }

        void changeLodBias(float delta)
        {
            float lodBias = uboVS.lodBias;

            lodBias += delta;
            if (lodBias < 0.0f)
            {
                lodBias = 0.0f;
            }
            if (lodBias > texture.mipLevels)
            {
                lodBias = (float)texture.mipLevels;
            }
            uboVS.lodBias = lodBias;

            updateUniformBuffers();
        }

        //void keyPressed(uint keyCode)
        //{
        //    switch (keyCode)
        //    {
        //        case KEY_KPADD:
        //        case GAMEPAD_BUTTON_R1:
        //            changeLodBias(0.1f);
        //            break;
        //        case KEY_KPSUB:
        //        case GAMEPAD_BUTTON_L1:
        //            changeLodBias(-0.1f);
        //            break;
        //    }
        //}
    }
}
