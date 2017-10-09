/*
* Translation into C# and Magnesium interface 2016
* Vulkan Example - Basic indexed triangle rendering by 2016 by Copyright (C) Sascha Willems - www.saschawillems.de
*
* This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
*/

using Magnesium;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TriangleDemo
{
    public class VulkanExample : IDisposable
    {
        // Vertex buffer and attributes
        class VertexBufferInfo
        {
            public IMgDeviceMemory memory;	// Handle to the device memory for this buffer
            public IMgBuffer buffer; // Handle to the Vulkan buffer object that the memory is bound to
            public MgPipelineVertexInputStateCreateInfo inputState;
            public MgVertexInputBindingDescription inputBinding;
            public MgVertexInputAttributeDescription[] inputAttributes;
        }

        VertexBufferInfo vertices = new VertexBufferInfo();

        struct IndicesInfo
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
            public UInt32 count;
        }

        IndicesInfo indices = new IndicesInfo();

        private IMgGraphicsConfiguration mConfiguration;

        // Uniform block object
        struct UniformData
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
            public MgDescriptorBufferInfo descriptor;
        }

        struct UniformBufferObject
        {
            public Matrix4 projectionMatrix;
            public Matrix4 modelMatrix;
            public Matrix4 viewMatrix;
        };

        UniformBufferObject uboVS;


        UniformData uniformDataVS = new UniformData();
        IMgPipelineLayout mPipelineLayout;
        IMgPipeline mPipeline;
        IMgDescriptorSetLayout mDescriptorSetLayout;
        IMgDescriptorSet mDescriptorSet;
        IMgSemaphore mPresentCompleteSemaphore;
        IMgSemaphore mRenderCompleteSemaphore;

        List<IMgFence> mWaitFences = new List<IMgFence>();

        private uint mWidth;
        private uint mHeight;
        private IMgGraphicsDevice mGraphicsDevice;
        private IMgDescriptorPool mDescriptorPool;

        private IMgCommandBuffer mPrePresentCmdBuffer;
        private IMgCommandBuffer mPostPresentCmdBuffer;
        private IMgPresentationLayer mPresentationLayer;

        public VulkanExample
        (
            IMgGraphicsConfiguration configuration,
            IMgSwapchainCollection swapchains,
            IMgGraphicsDevice graphicsDevice,
            IMgPresentationLayer presentationLayer,
            ITriangleDemoShaderPath shaderPath
        )
        {
            mConfiguration = configuration;
            mSwapchains = swapchains;
            mGraphicsDevice = graphicsDevice;
            mPresentationLayer = presentationLayer;
            mTrianglePath = shaderPath;

            mWidth = 1280U;
            mHeight = 720U;

            try
            {
                mConfiguration.Initialize(mWidth, mHeight);
                InitSwapchain(mWidth, mHeight);
                Prepare();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private IMgSwapchainCollection mSwapchains;
        private void InitSwapchain(uint width, uint height)
        {
            Debug.Assert(mConfiguration.Partition != null);


            const int NO_OF_BUFFERS = 1;
            var buffers = new IMgCommandBuffer[NO_OF_BUFFERS];
            var pAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandBufferCount = NO_OF_BUFFERS,
                CommandPool = mConfiguration.Partition.CommandPool,
                Level = MgCommandBufferLevel.PRIMARY,
            };

            mConfiguration.Device.AllocateCommandBuffers(pAllocateInfo, buffers);

            var createInfo = new MgGraphicsDeviceCreateInfo
            {
                Samples = MgSampleCountFlagBits.COUNT_1_BIT,
                Color = MgFormat.R8G8B8A8_UINT,
                DepthStencil = MgFormat.D24_UNORM_S8_UINT,
                Width = mWidth,
                Height = mHeight,
            };

            var setupCmdBuffer = buffers[0];

            var cmdBufInfo = new MgCommandBufferBeginInfo();

            var err = setupCmdBuffer.BeginCommandBuffer(cmdBufInfo);
            Debug.Assert(err == Result.SUCCESS);

            mGraphicsDevice.Create(setupCmdBuffer, mSwapchains, createInfo);

            err = setupCmdBuffer.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);


            var submission = new[] {
                new MgSubmitInfo
                {
                    CommandBuffers = new IMgCommandBuffer[]
                    {
                        buffers[0],
                    },
                }
            };

            err = mConfiguration.Queue.QueueSubmit(submission, null);
            Debug.Assert(err == Result.SUCCESS);

            mConfiguration.Queue.QueueWaitIdle();

            mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, buffers);
        }

        #region prepare

        private bool mPrepared = false;
        void Prepare()
        {
            BeforePrepare();

            PrepareSynchronizationPrimitives();
            PrepareVertices();
            PrepareUniformBuffers();
            SetupDescriptorSetLayout();
            PreparePipelines();
            SetupDescriptorPool();
            SetupDescriptorSet();
            BuildCommandBuffers();
            mPrepared = true;
        }

        private void BeforePrepare()
        {
            CreateCommandBuffers();
        }

        void PrepareSynchronizationPrimitives()
        {
            var semaphoreCreateInfo = new MgSemaphoreCreateInfo { };

            Debug.Assert(mConfiguration.Device != null);

            var err = mConfiguration.Device.CreateSemaphore(semaphoreCreateInfo, null, out mPresentCompleteSemaphore);
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Device.CreateSemaphore(semaphoreCreateInfo, null, out mRenderCompleteSemaphore);
            Debug.Assert(err == Result.SUCCESS);

            var fenceCreateInfo = new MgFenceCreateInfo {
                Flags = MgFenceCreateFlagBits.SIGNALED_BIT,
            };

            var noOfCommandBuffers = drawCmdBuffers.Length; 
            for (var i = 0; i < noOfCommandBuffers; ++i)
            {
                IMgFence fence;
                err = mConfiguration.Device.CreateFence(fenceCreateInfo, null, out fence);
                Debug.Assert(err == Result.SUCCESS);
                mWaitFences.Add(fence);
            }
        }

        struct TriangleVertex
        {
            public Vector3 position;
            public Vector3 color;
        };

        class StagingBuffer
        {
            public IMgDeviceMemory memory;
            public IMgBuffer buffer;
        };

        void PrepareVertices()
        {
            TriangleVertex[] vertexBuffer =
            {
                new TriangleVertex{
                    position =new Vector3(1.0f,  1.0f, 0.0f),
                    color = new Vector3( 1.0f, 0.0f, 0.0f )
                },

                new TriangleVertex{
                    position =new Vector3(-1.0f,  1.0f, 0.0f ),
                    color = new Vector3( 0.0f, 1.0f, 0.0f )
                },

                new TriangleVertex{
                    position =new Vector3( 0.0f, -1.0f, 0.0f ),
                    color = new Vector3( 0.0f, 0.0f, 1.0f )
                },
            };

            var structSize = Marshal.SizeOf(typeof(TriangleVertex));
            var vertexBufferSize = (ulong)(vertexBuffer.Length * structSize);

            UInt32[] indexBuffer = { 0, 1, 2 };
            indices.count = (uint)indexBuffer.Length;
            var indexBufferSize = indices.count * sizeof(UInt32);

            var stagingBuffers = new
            {
                vertices = new StagingBuffer(),
                indices = new StagingBuffer(),
            };

            {
                var vertexBufferInfo = new MgBufferCreateInfo
                {
                    Size = vertexBufferSize,
                    Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(vertexBufferInfo, null, out stagingBuffers.vertices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                mConfiguration.Device.GetBufferMemoryRequirements(stagingBuffers.vertices.buffer, out MgMemoryRequirements memReqs);

                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    out uint typeIndex);

                Debug.Assert(isValid);

                MgMemoryAllocateInfo memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out stagingBuffers.vertices.memory);
                Debug.Assert(err == Result.SUCCESS);

                // Map and copy
                err = stagingBuffers.vertices.memory.MapMemory(mConfiguration.Device, 0, memAlloc.AllocationSize, 0, out IntPtr data);
                Debug.Assert(err == Result.SUCCESS);

                var offset = 0;
                foreach (var vertex in vertexBuffer)
                {
                    IntPtr dest = IntPtr.Add(data, offset);
                    Marshal.StructureToPtr(vertex, dest, false);
                    offset += structSize;
                }

                stagingBuffers.vertices.memory.UnmapMemory(mConfiguration.Device);

                stagingBuffers.vertices.buffer.BindBufferMemory(mConfiguration.Device, stagingBuffers.vertices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var vertexBufferInfo = new MgBufferCreateInfo
                {
                    Size = vertexBufferSize,
                    Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT | MgBufferUsageFlagBits.TRANSFER_DST_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(vertexBufferInfo, null, out vertices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                mConfiguration.Device.GetBufferMemoryRequirements(vertices.buffer, out MgMemoryRequirements memReqs);

                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out uint typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out vertices.memory);
                Debug.Assert(err == Result.SUCCESS);

                err = vertices.buffer.BindBufferMemory(mConfiguration.Device, vertices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var indexbufferInfo = new MgBufferCreateInfo
                {
                    Size = indexBufferSize,
                    Usage = MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(indexbufferInfo, null, out stagingBuffers.indices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                mConfiguration.Device.GetBufferMemoryRequirements(stagingBuffers.indices.buffer, out MgMemoryRequirements memReqs);

                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits,
                    MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    out uint typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out stagingBuffers.indices.memory);
                Debug.Assert(err == Result.SUCCESS);

                err = stagingBuffers.indices.memory.MapMemory(mConfiguration.Device, 0, indexBufferSize, 0, out IntPtr data);
                Debug.Assert(err == Result.SUCCESS);

                var uintBuffer = new byte[indexBufferSize];

                var bufferSize = (int)indexBufferSize;
                Buffer.BlockCopy(indexBuffer, 0, uintBuffer, 0, bufferSize);
                Marshal.Copy(uintBuffer, 0, data, bufferSize);

                stagingBuffers.indices.memory.UnmapMemory(mConfiguration.Device);

                err = stagingBuffers.indices.buffer.BindBufferMemory(mConfiguration.Device, stagingBuffers.indices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var indexbufferInfo = new MgBufferCreateInfo
                {
                    Size = indexBufferSize,
                    Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT | MgBufferUsageFlagBits.TRANSFER_DST_BIT,
                };

                var err = mConfiguration.Device.CreateBuffer(indexbufferInfo, null, out indices.buffer);
                Debug.Assert(err == Result.SUCCESS);

                mConfiguration.Device.GetBufferMemoryRequirements(indices.buffer, out MgMemoryRequirements memReqs);

                var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out uint typeIndex);
                Debug.Assert(isValid);

                var memAlloc = new MgMemoryAllocateInfo
                {
                    AllocationSize = memReqs.Size,
                    MemoryTypeIndex = typeIndex,
                };

                err = mConfiguration.Device.AllocateMemory(memAlloc, null, out indices.memory);
                Debug.Assert(err == Result.SUCCESS);

                err = indices.buffer.BindBufferMemory(mConfiguration.Device, indices.memory, 0);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var cmdBufferBeginInfo = new MgCommandBufferBeginInfo
                {

                };

                IMgCommandBuffer copyCmd = getCommandBuffer(true);

                copyCmd.CmdCopyBuffer(
                    stagingBuffers.vertices.buffer,
                    vertices.buffer,
                    new[]
                    {
                        new MgBufferCopy
                        {
                            Size = vertexBufferSize,
                        }
                    }
                );

                copyCmd.CmdCopyBuffer(stagingBuffers.indices.buffer, indices.buffer,
                    new[]
                    {
                        new MgBufferCopy
                        {
                            Size = indexBufferSize,
                        }
                    });

                flushCommandBuffer(copyCmd);

                stagingBuffers.vertices.buffer.DestroyBuffer(mConfiguration.Device, null);
                stagingBuffers.vertices.memory.FreeMemory(mConfiguration.Device, null);
                stagingBuffers.indices.buffer.DestroyBuffer(mConfiguration.Device, null);
                stagingBuffers.indices.memory.FreeMemory(mConfiguration.Device, null);
            }

            const uint VERTEX_BUFFER_BIND_ID = 0;

            vertices.inputBinding = new MgVertexInputBindingDescription
            {
                Binding = VERTEX_BUFFER_BIND_ID,
                Stride = (uint) structSize,
                InputRate = MgVertexInputRate.VERTEX,
            };

            var vertexSize = (uint) Marshal.SizeOf(typeof(Vector3));

            vertices.inputAttributes = new MgVertexInputAttributeDescription[]
            {
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 0,
                    Format =  MgFormat.R32G32B32_SFLOAT,
                    Offset = 0,
                },                             
                new MgVertexInputAttributeDescription
                {
                    Binding = VERTEX_BUFFER_BIND_ID,
                    Location = 1,
                    Format = MgFormat.R32G32B32_SFLOAT,
                    Offset = vertexSize,
                }
            };

            vertices.inputState = new MgPipelineVertexInputStateCreateInfo
            {
                VertexBindingDescriptions = new MgVertexInputBindingDescription[]
                {
                    vertices.inputBinding,
                },
                VertexAttributeDescriptions = vertices.inputAttributes,
            };                
        }
        IMgCommandBuffer getCommandBuffer(bool begin)
        {
            var buffers = new IMgCommandBuffer[1];

            var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
            {
                CommandPool = mConfiguration.Partition.CommandPool,
                Level =  MgCommandBufferLevel.PRIMARY,
                CommandBufferCount = 1,
            };

            var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, buffers);
            Debug.Assert(err == Result.SUCCESS);

            var cmdBuf = buffers[0];

            if (begin)
            {
                var cmdBufInfo = new MgCommandBufferBeginInfo();

                err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);
            }

            return cmdBuf;
        }

        void flushCommandBuffer(IMgCommandBuffer commandBuffer)
        {
            Debug.Assert(commandBuffer != null);

            var err = commandBuffer.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);

            var submitInfos = new [] 
            {
                new MgSubmitInfo
                {
                    CommandBuffers = new []
                    {
                        commandBuffer
                    }
                }
            };

            var fenceCreateInfo = new MgFenceCreateInfo();

            err = mConfiguration.Device.CreateFence(fenceCreateInfo, null, out IMgFence fence);
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Queue.QueueSubmit(submitInfos, fence);
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Device.WaitForFences(new[] { fence }, true, ulong.MaxValue);
            Debug.Assert(err == Result.SUCCESS);

            fence.DestroyFence(mConfiguration.Device, null);
            mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { commandBuffer } );
        }

        void PrepareUniformBuffers()
        {
            var structSize = (uint)Marshal.SizeOf(typeof(UniformBufferObject));

            MgBufferCreateInfo bufferInfo = new MgBufferCreateInfo
            {
                Size = structSize,
                Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
            };

            var err = mConfiguration.Device.CreateBuffer(bufferInfo, null, out uniformDataVS.buffer);
            Debug.Assert(err == Result.SUCCESS);

            mConfiguration.Device.GetBufferMemoryRequirements(uniformDataVS.buffer, out MgMemoryRequirements memReqs);

            var isValid = mConfiguration.Partition.GetMemoryType(memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT, out uint typeIndex);
            Debug.Assert(isValid);

            MgMemoryAllocateInfo allocInfo = new MgMemoryAllocateInfo
            {
                AllocationSize = memReqs.Size,
                MemoryTypeIndex = typeIndex,
            };

            err = mConfiguration.Device.AllocateMemory(allocInfo, null, out uniformDataVS.memory);
            Debug.Assert(err == Result.SUCCESS);

            err = uniformDataVS.buffer.BindBufferMemory(mConfiguration.Device, uniformDataVS.memory, 0);
            Debug.Assert(err == Result.SUCCESS);

            uniformDataVS.descriptor = new MgDescriptorBufferInfo
            {
                Buffer = uniformDataVS.buffer,
                Offset = 0,
                Range = structSize,
            };

            UpdateUniformBuffers();
        }


        void SetupDescriptorSetLayout()
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
                {
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorCount = 1,
                        StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
                        ImmutableSamplers = null,
                        DescriptorType = MgDescriptorType.UNIFORM_BUFFER,
                        Binding = 0,                         
                    }
                },
            };

            var err = mConfiguration.Device.CreateDescriptorSetLayout(descriptorLayout, null, out mDescriptorSetLayout);
            Debug.Assert(err == Result.SUCCESS);

            var pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                 SetLayouts = new IMgDescriptorSetLayout[]
                 {
                     mDescriptorSetLayout,
                 }
            };

            err = mConfiguration.Device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out mPipelineLayout);
            Debug.Assert(err == Result.SUCCESS);
        }

        void PreparePipelines()
        {
            using (var vertFs = mTrianglePath.OpenVertexShader())
            using (var fragFs = mTrianglePath.OpenFragmentShader())
            {
                IMgShaderModule vsModule;
                {
                    var vsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = vertFs,
                        CodeSize = new UIntPtr((ulong)vertFs.Length),
                    };
                    mConfiguration.Device.CreateShaderModule(vsCreateInfo, null, out vsModule);
                }

                IMgShaderModule fsModule;
                {
                    var fsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = fragFs,
                        CodeSize = new UIntPtr((ulong)fragFs.Length),
                    };
                    mConfiguration.Device.CreateShaderModule(fsCreateInfo, null, out fsModule);
                }

                var pipelineCreateInfo = new MgGraphicsPipelineCreateInfo
                {
                    Stages = new []
                    {
                        new MgPipelineShaderStageCreateInfo
                        {
                            Stage = MgShaderStageFlagBits.VERTEX_BIT,
                            Module = vsModule,
                            Name = "vertFunc",
                        },
                        new MgPipelineShaderStageCreateInfo
                        {
                            Stage = MgShaderStageFlagBits.FRAGMENT_BIT,
                            Module = fsModule,
                            Name = "fragFunc",
                        },
                    },
                    VertexInputState = vertices.inputState,
                    InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                    {
                        // GL002 - TRIANGLE STRIP TEST
                        Topology = MgPrimitiveTopology.TRIANGLE_STRIP,
                    },
                    RasterizationState = new MgPipelineRasterizationStateCreateInfo
                    {
                        PolygonMode = MgPolygonMode.FILL,
                        CullMode = MgCullModeFlagBits.NONE,
                        FrontFace = MgFrontFace.COUNTER_CLOCKWISE,
                        DepthClampEnable = false,
                        RasterizerDiscardEnable = false,
                        DepthBiasEnable = false,
                        LineWidth = 1.0f,
                    },
                    ColorBlendState = new MgPipelineColorBlendStateCreateInfo
                    {
                        Attachments = new []
                        {
                            new MgPipelineColorBlendAttachmentState
                            {
                                ColorWriteMask =  MgColorComponentFlagBits.R_BIT | MgColorComponentFlagBits.G_BIT | MgColorComponentFlagBits.B_BIT | MgColorComponentFlagBits.A_BIT,
                                BlendEnable = false,
                            }
                        },
                    },
                    MultisampleState = new MgPipelineMultisampleStateCreateInfo
                    {
                        RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
                        SampleMask = null,
                    },
                    Layout = mPipelineLayout,
                    RenderPass = mGraphicsDevice.Renderpass,
                    ViewportState = null,
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
                        DynamicStates = new[]
                        {
                            MgDynamicState.VIEWPORT,
                            MgDynamicState.SCISSOR,
                        }
                    },
                };

                var err = mConfiguration.Device.CreateGraphicsPipelines(null, new[] { pipelineCreateInfo }, null, out IMgPipeline[] pipelines);
                Debug.Assert(err == Result.SUCCESS);

                vsModule.DestroyShaderModule(mConfiguration.Device, null);
                fsModule.DestroyShaderModule(mConfiguration.Device, null);

                mPipeline = pipelines[0];
            }

        }

        void SetupDescriptorPool()
        {
            var descriptorPoolInfo = new MgDescriptorPoolCreateInfo
            {
                PoolSizes = new MgDescriptorPoolSize[]
                {
                    new MgDescriptorPoolSize
                    {
                        Type = MgDescriptorType.UNIFORM_BUFFER,
                        DescriptorCount = 1,
                    },
                },
                MaxSets = 1,
            };
            var err = mConfiguration.Device.CreateDescriptorPool(descriptorPoolInfo, null, out mDescriptorPool);
            Debug.Assert(err == Result.SUCCESS);
        }

        void SetupDescriptorSet()
        {
            var allocInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = mDescriptorPool,
                DescriptorSetCount = 1,
                SetLayouts = new[] { mDescriptorSetLayout },
            };

            IMgDescriptorSet[] dSets;
            var err = mConfiguration.Device.AllocateDescriptorSets(allocInfo, out dSets);
            mDescriptorSet = dSets[0];

            Debug.Assert(err == Result.SUCCESS);
            mConfiguration.Device.UpdateDescriptorSets(
                new []
                {
                    new MgWriteDescriptorSet
                    {
                        DstSet = mDescriptorSet,
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.UNIFORM_BUFFER,
                        BufferInfo = new MgDescriptorBufferInfo[]
                        {
                            uniformDataVS.descriptor,
                        },
                        DstBinding = 0,
                    },
                }, null);
        }

        IMgCommandBuffer[] drawCmdBuffers;

        void CreateCommandBuffers()
        {
            drawCmdBuffers = new IMgCommandBuffer[mGraphicsDevice.Framebuffers.Length];

            {
                var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = (uint)mGraphicsDevice.Framebuffers.Length,
                    CommandPool = mConfiguration.Partition.CommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };

                var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, drawCmdBuffers);
                Debug.Assert(err == Result.SUCCESS);
            }

            {
                var cmdBufAllocateInfo = new MgCommandBufferAllocateInfo
                {
                    CommandBufferCount = 2,
                    CommandPool = mConfiguration.Partition.CommandPool,
                    Level = MgCommandBufferLevel.PRIMARY,
                };
  
                var presentBuffers = new IMgCommandBuffer[2];
                var err = mConfiguration.Device.AllocateCommandBuffers(cmdBufAllocateInfo, presentBuffers);
                Debug.Assert(err == Result.SUCCESS);

                mPrePresentCmdBuffer = presentBuffers[0];
                mPostPresentCmdBuffer = presentBuffers[1];
            }
        }

        void BuildCommandBuffers()
        {
            var renderPassBeginInfo = new MgRenderPassBeginInfo {
                RenderPass = mGraphicsDevice.Renderpass,
                RenderArea = new MgRect2D
                {
                    Offset = new MgOffset2D {  X = 0, Y = 0 },
                    Extent = new MgExtent2D { Width = mWidth, Height = mHeight },
                },
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(mSwapchains.Format, new MgColor4f(0f, 0f, 0f, 0f)),                    
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue( 1.0f, 0) },
                },
            };
            
            for (var i = 0; i < drawCmdBuffers.Length; ++i)
            {
                renderPassBeginInfo.Framebuffer = mGraphicsDevice.Framebuffers[i];

                var cmdBuf = drawCmdBuffers[i];

                var cmdBufInfo = new MgCommandBufferBeginInfo { };
                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);

                cmdBuf.CmdSetViewport(0, 
                    new[] {
                        new MgViewport {
                            Height = (float) mHeight,
                            Width = (float) mWidth,
                            MinDepth = 0.0f,
                            MaxDepth = 1.0f,
                        }
                    }
                );

                cmdBuf.CmdSetScissor(0,
                    new[] {
                        new MgRect2D {
                            Extent = new MgExtent2D { Width = mWidth, Height = mHeight },
                            Offset = new MgOffset2D { X = 0, Y = 0 },
                        }
                    }
                );

                cmdBuf.CmdBindDescriptorSets( MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, 1, new[] { mDescriptorSet }, null);

                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, mPipeline);

                cmdBuf.CmdBindVertexBuffers(0, new[] { vertices.buffer }, new [] { 0UL });

                cmdBuf.CmdBindIndexBuffer(indices.buffer, 0, MgIndexType.UINT32);

                cmdBuf.CmdDrawIndexed(indices.count, 1, 0, 0, 1);

                cmdBuf.CmdEndRenderPass();

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        #endregion

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">An angle in degrees</param>
        /// <returns>The angle expressed in radians</returns>
        public static float DegreesToRadians(float degrees)
        {
            const double degToRad = System.Math.PI / 180.0;
            return (float) (degrees * degToRad);
        }

        void UpdateUniformBuffers()
        {
            // Update matrices
            uboVS.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                DegreesToRadians(60.0f), 
                (mWidth / mHeight), 
                1.0f,
                256.0f);

            const float ZOOM = -2.5f;

            uboVS.viewMatrix = Matrix4.CreateTranslation(0, 0, ZOOM);

            uboVS.modelMatrix = Matrix4.Identity;

            var structSize = (ulong) Marshal.SizeOf(typeof(UniformBufferObject));

            var err = uniformDataVS.memory.MapMemory(mConfiguration.Device, 0, structSize, 0, out IntPtr pData);

            Marshal.StructureToPtr(uboVS, pData, false);
            uniformDataVS.memory.UnmapMemory(mConfiguration.Device);
        }

        public void RenderLoop()
        {
            render();
        }

        void render()
        {
            if (!mPrepared)
                return;
            Draw();
        }

        void Draw()
        {
            var currentBufferIndex = mPresentationLayer.BeginDraw(mPostPresentCmdBuffer, mPresentCompleteSemaphore);

            var fence = mWaitFences[(int) currentBufferIndex];
            var err = mConfiguration.Device.WaitForFences(new[] { fence } , true, ulong.MaxValue);
            Debug.Assert(err == Result.SUCCESS);

            err = mConfiguration.Device.ResetFences(new[] { fence });

            var submitInfos = new MgSubmitInfo[]
            {
                new MgSubmitInfo
                {
                    WaitSemaphores = new []
                    {
                        new MgSubmitInfoWaitSemaphoreInfo
                        {
                            WaitDstStageMask =  MgPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                            WaitSemaphore = mPresentCompleteSemaphore,
                        }
                    },
                    CommandBuffers = new []
                    {
                        drawCmdBuffers[currentBufferIndex]
                    },
                    SignalSemaphores = new []
                    {
                        mRenderCompleteSemaphore
                    },                    
                }
            };                                        

            err = mConfiguration.Queue.QueueSubmit(submitInfos, fence);
            Debug.Assert(err == Result.SUCCESS);
            mPresentationLayer.EndDraw(new[] { currentBufferIndex }, mPrePresentCmdBuffer, new[] { mRenderCompleteSemaphore });
        }

        void ViewChanged()
        {
            UpdateUniformBuffers();
        }

        #region IDisposable Support
        private bool mIsDisposed = false; // To detect redundant calls
        private ITriangleDemoShaderPath mTrianglePath;

        protected virtual void Dispose(bool disposing)
        {
            if (mIsDisposed)
            {
                return;
            }

            ReleaseUnmanagedResources();

            if (disposing)
            {  
                ReleaseManagedResources();
            }

            mIsDisposed = true;            
        }

        private void ReleaseManagedResources()
        {
           
        }

        private void ReleaseUnmanagedResources()
        {
            var device = mConfiguration.Device;
            if (device != null)
            {
                if (mPipeline != null)
                    mPipeline.DestroyPipeline(device, null);

                if (mPipelineLayout != null)
                    mPipelineLayout.DestroyPipelineLayout(device, null);

                if (mDescriptorSetLayout != null)
                    mDescriptorSetLayout.DestroyDescriptorSetLayout(device, null);

                if (vertices.buffer != null)
                    vertices.buffer.DestroyBuffer(device, null);

                if (vertices.memory != null)
                    vertices.memory.FreeMemory(device, null);

                if (indices.buffer != null)
                    indices.buffer.DestroyBuffer(device, null);

                if (indices.memory != null)
                    indices.memory.FreeMemory(device, null);

                if (uniformDataVS.buffer != null)
                    uniformDataVS.buffer.DestroyBuffer(device, null);

                if (uniformDataVS.memory != null)
                    uniformDataVS.memory.FreeMemory(device, null);

                if (mPresentCompleteSemaphore != null)
                    mPresentCompleteSemaphore.DestroySemaphore(device, null);


                if (mRenderCompleteSemaphore != null)
                    mRenderCompleteSemaphore.DestroySemaphore(device, null);

                foreach (var fence in mWaitFences)
                {
                    fence.DestroyFence(device, null);
                }

                if (mDescriptorPool != null)
                    mDescriptorPool.DestroyDescriptorPool(device, null);

                if (drawCmdBuffers != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, drawCmdBuffers);

                if (mPostPresentCmdBuffer != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { mPostPresentCmdBuffer });


                if (mPrePresentCmdBuffer != null)
                    mConfiguration.Device.FreeCommandBuffers(mConfiguration.Partition.CommandPool, new[] { mPrePresentCmdBuffer });

                if (mGraphicsDevice != null)
                    mGraphicsDevice.Dispose();
            }
        }

        ~VulkanExample()
        {
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