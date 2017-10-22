using Magnesium;
using Magnesium.Utilities;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class OffscreenPipeline
    {
        private IOffscreenPipelineMediaPath mTrianglePath;
        public OffscreenPipeline(IOffscreenPipelineMediaPath path)
        {
            mTrianglePath = path;
        }

        private static IMgDescriptorSetLayout SetupDescriptorSetLayout(IMgDevice device)
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
                {
                    // Binding 0: Uniform buffer (Vertex shader)
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

            var err = device.CreateDescriptorSetLayout(descriptorLayout, null, out IMgDescriptorSetLayout setLayout);
            Debug.Assert(err == Result.SUCCESS);
            return setLayout;
        }

        private static IMgPipelineLayout SetupPipelineLayout(IMgDevice device, IMgDescriptorSetLayout descSetLayout)
        {
            var pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                SetLayouts = new IMgDescriptorSetLayout[]
                 {
                     descSetLayout,
                 }
            };

            var err = device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out IMgPipelineLayout pipelineLayout);
            Debug.Assert(err == Result.SUCCESS);
            return pipelineLayout;
        }

        public static IMgDescriptorPool SetupDescriptorPool(IMgGraphicsConfiguration configuration)
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
            var err = configuration.Device.CreateDescriptorPool(descriptorPoolInfo, null, out IMgDescriptorPool descriptorPool);
            Debug.Assert(err == Result.SUCCESS);
            return descriptorPool;
        }

        private static IMgDescriptorSet AllocateDescriptorSet(IMgGraphicsConfiguration configuration, IMgDescriptorPool pool, IMgDescriptorSetLayout setLayout)
        {
            // Allocate a new descriptor set from the global descriptor pool
            var allocInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = pool,
                DescriptorSetCount = 1,
                SetLayouts = new[] { setLayout },
            };

            var err = configuration.Device.AllocateDescriptorSets(allocInfo, out IMgDescriptorSet[] dSets);
            Debug.Assert(err == Result.SUCCESS);

            var result = dSets[0];
            return result;
        }

        private IMgDescriptorSetLayout mDescriptorSetLayout;
        private IMgPipelineLayout mPipelineLayout;
        private IMgPipeline mPipeline;
        private IMgDescriptorPool mDescriptorPool;
        private IMgDescriptorSet mDescriptorSet;

        public void Initialize(IMgGraphicsConfiguration configuration, IMgEffectFramework framework)
        {
            var device = configuration.Device;
            Debug.Assert(device != null);

            mDescriptorSetLayout = SetupDescriptorSetLayout(device);
            mPipelineLayout = SetupPipelineLayout(device, mDescriptorSetLayout);
            mPipeline = BuildPipeline(device, mPipelineLayout, framework, mTrianglePath);
            mDescriptorPool = SetupDescriptorPool(configuration);
            mDescriptorSet = AllocateDescriptorSet(configuration, mDescriptorPool, mDescriptorSetLayout);
        }

        internal void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            var device = configuration.Device;
            Debug.Assert(device != null);

            if (mDescriptorPool != null)
            {
                if (mDescriptorSet != null)
                {
                    device.FreeDescriptorSets(mDescriptorPool, new[] { mDescriptorSet });
                    mDescriptorSet = null;
                }

                mDescriptorPool.DestroyDescriptorPool(device, null);
                mDescriptorPool = null;
            }
            if (mPipeline != null)
            {
                mPipeline.DestroyPipeline(device, null);
                mPipeline = null;
            }
            if (mPipelineLayout != null)
            {
                mPipelineLayout.DestroyPipelineLayout(device, null);
                mPipelineLayout = null;
            }
            if (mDescriptorSetLayout != null)
            {
                mDescriptorSetLayout.DestroyDescriptorSetLayout(device, null);
                mDescriptorSetLayout = null;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TriangleVertex
        {
            public Vector3 position;
            public Vector3 color;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct UniformBufferObject
        {
            public Matrix4 projectionMatrix;
            public Matrix4 modelMatrix;
            public Matrix4 viewMatrix;
        };

        private int mVertexDataPosition;
        private int mIndexDataPosition;
        private int mUniformDataPosition;
        public void Reserve(MgBlockAllocationList slots)
        {
            var structSize = Marshal.SizeOf(typeof(TriangleVertex));
            var vertices = new MgStorageBlockAllocationInfo
            {
                Size = (ulong) (3 * structSize),
                ElementByteSize = (uint) structSize,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT,
                Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT 
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
            };
            mVertexDataPosition = slots.Insert(vertices);

            var indexElementSize = (uint) sizeof(uint);
            var indices = new MgStorageBlockAllocationInfo
            {
                Size = (ulong) (3 * indexElementSize),
                ElementByteSize = indexElementSize,
                Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                | MgBufferUsageFlagBits.TRANSFER_DST_BIT,
            };
            mIndexDataPosition = slots.Insert(indices);

            var uniformSize = (uint)Marshal.SizeOf(typeof(UniformBufferObject));
            var uniforms = new MgStorageBlockAllocationInfo
            {
                Size = (ulong) uniformSize,
                ElementByteSize = uniformSize,
                MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,                
                Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
            };
            mUniformDataPosition = slots.Insert(uniforms);
        }

        public MgCommandBuildOrder GenerateBuildOrder(MgOptimizedStorage storage, MgOptimizedStorageMap storageMap)
        {
            var vertexDest = storageMap.Allocations[mVertexDataPosition];
            var vertexInstance = storage.Blocks[vertexDest.BlockIndex];

            var indexDest = storageMap.Allocations[mIndexDataPosition];
            var indexInstance = storage.Blocks[indexDest.BlockIndex];

            return new MgCommandBuildOrder
            {
                Vertices = new MgCommandBuildOrderBufferInfo
                {
                    Buffer = vertexInstance.Buffer,
                    ByteOffset = vertexDest.Offset,
                },
                Indices = new MgCommandBuildOrderBufferInfo
                {
                    Buffer = indexInstance.Buffer,
                    ByteOffset = indexDest.Offset,
                },
                IndexCount = 3,
                InstanceCount = 1,
            };
        }

        public void SetupUniforms(
            IMgGraphicsConfiguration configuration,
            MgOptimizedStorageContainer container
        )
        {
            SetupDescriptorSet(configuration, mDescriptorSet, container, mUniformDataPosition);
        }

        public static void SetupDescriptorSet(
            IMgGraphicsConfiguration configuration,
            IMgDescriptorSet dest,
            MgOptimizedStorageContainer src,
            int allocationIndex)
        {

            var allocationInfo = src.Map.Allocations[allocationIndex];
            var uniformInstance = src.Storage.Blocks[allocationInfo.BlockIndex];

            var structSize = Marshal.SizeOf(typeof(UniformBufferObject));
            var descriptor = new MgDescriptorBufferInfo
            {
                Buffer = uniformInstance.Buffer,
                Offset = allocationInfo.Offset,
                Range = (ulong)structSize,
            };

            configuration.Device.UpdateDescriptorSets(
                new[]
                {
                    // Binding 0 : Uniform buffer
                    new MgWriteDescriptorSet
                    {
                        DstSet = dest,
                        DescriptorCount = 1,
                        DescriptorType =  MgDescriptorType.UNIFORM_BUFFER,
                        BufferInfo = new []
                        {
                            descriptor,
                        },
                        DstBinding = 0,
                    },
                }, null);            
        }

        private UniformBufferObject mUBOVS;
        public void UpdateUniformBuffers(IMgGraphicsConfiguration configuration, MgOptimizedStorageContainer container, IMgEffectFramework framework)
        {
            // Update matrices
            mUBOVS.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                DegreesToRadians(60.0f),
                (framework.Viewport.Width / framework.Viewport.Height),
                framework.Viewport.MinDepth,
                framework.Viewport.MaxDepth);

            const float ZOOM = -2.5f;

            mUBOVS.viewMatrix = Matrix4.CreateTranslation(0, 0, ZOOM);

            // TODO : track down rotation
            mUBOVS.modelMatrix = Matrix4.Identity;
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.x), glm::vec3(1.0f, 0.0f, 0.0f));
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.y), glm::vec3(0.0f, 1.0f, 0.0f));
            //uboVS.modelMatrix = glm::rotate(uboVS.modelMatrix, glm::radians(rotation.z), glm::vec3(0.0f, 0.0f, 1.0f));


            var structSize = (ulong)Marshal.SizeOf(typeof(UniformBufferObject));

            // Map uniform buffer and update it

            var allocationInfo = container.Map.Allocations[mUniformDataPosition];
            var uniformInstance = container.Storage.Blocks[allocationInfo.BlockIndex];

            var err = uniformInstance.DeviceMemory.MapMemory(configuration.Device, allocationInfo.Offset, allocationInfo.Size, 0, out IntPtr pData);

            Marshal.StructureToPtr(mUBOVS, pData, false);
            // Unmap after data has been copied
            // Note: Since we requested a host coherent memory type for the uniform buffer, the write is instantly visible to the GPU
            uniformInstance.DeviceMemory.UnmapMemory(configuration.Device);
        }

        public static float DegreesToRadians(float degrees)
        {
            const double degToRad = System.Math.PI / 180.0;
            return (float)(degrees * degToRad);
        }

        public void Populate(MgOptimizedStorageContainer container, IMgGraphicsConfiguration configuration, IMgCommandBuffer copyCmd)
        {
            var corners = new []
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
            var vertexBufferSize = (ulong)(corners.Length * structSize);

            // Setup indices
            UInt32[] indexBuffer = { 0, 1, 2 };
            var indexBufferSize = (ulong) indexBuffer.Length * sizeof(UInt32);

            // DEVICE_LOCAL vertex buffer
            var vertexDest = container.Map.Allocations[mVertexDataPosition];
            var vertexInstance = container.Storage.Blocks[vertexDest.BlockIndex];
            var vertices = new MgStagingBuffer(            
                vertexInstance.Buffer,
                vertexDest.Offset
            );
            vertices.Initialize(configuration, vertexBufferSize);

            // DEVICE_LOCAL index buffer 
            var indexDest = container.Map.Allocations[mIndexDataPosition];
            var indexInstance = container.Storage.Blocks[indexDest.BlockIndex];
            var indices = new MgStagingBuffer(
                indexInstance.Buffer,
                indexDest.Offset
            );
            indices.Initialize(configuration, indexBufferSize);

            var stagingBuffers = new[] { vertices, indices };

            // TRANSFER DATA
            var cmdBufInfo = new MgCommandBufferBeginInfo { };

            var err = copyCmd.BeginCommandBuffer(cmdBufInfo);
            Debug.Assert(err == Result.SUCCESS);

            foreach (var stage in stagingBuffers)
            {
                stage.Transfer(copyCmd);
            }

            err = copyCmd.EndCommandBuffer();
            Debug.Assert(err == Result.SUCCESS);

            var fenceCreateInfo = new MgFenceCreateInfo { };
            err = configuration.Device.CreateFence(fenceCreateInfo, null, out IMgFence fence);
            Debug.Assert(err == Result.SUCCESS);
            var submitInfo = new MgSubmitInfo
            {
                CommandBuffers = new[] { copyCmd }
            };

            // Submit to the queue
            err = configuration.Queue.QueueSubmit(new[] { submitInfo }, fence);
            Debug.Assert(err == Result.SUCCESS);

            // Mg.OpenGL
            err = configuration.Queue.QueueWaitIdle();
            Debug.Assert(err == Result.SUCCESS);

            // Wait for the fence to signal that command buffer has finished executing
            err = configuration.Device.WaitForFences(new[] { fence }, true, ulong.MaxValue);
            Debug.Assert(err == Result.SUCCESS);

            foreach (var stage in stagingBuffers)
            {
                stage.Destroy(configuration.Device);
            }
        }

        public void BuildCommandBuffers(
            MgCommandBuildOrder order
            //IMgEffectFramework framework,             
            //IMgCommandBuffer[] drawCmdBuffers,
            //IMgDescriptorSet descriptorSet,
            //IMgBuffer vertices,
            //IMgBuffer indices,
            //uint indexCount
        )
        {            
            var colorFormat = order.Framework.RenderpassInfo.Attachments[0].Format;

            var renderPassBeginInfo = new MgRenderPassBeginInfo
            {
                RenderPass = order.Framework.Renderpass,
                RenderArea = order.Framework.Scissor,
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(colorFormat, new MgColor4f(0f, 0f, 0f, 0f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue( 1.0f, 0) },
                },
            };

            for (var i = 0; i < order.Count; ++i)
            {
                int index = order.First + i;
                renderPassBeginInfo.Framebuffer = order.Framework.Framebuffers[index];

                var cmdBuf = order.CommandBuffers[index];

                var cmdBufInfo = new MgCommandBufferBeginInfo { };
                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);


                cmdBuf.CmdSetViewport(0, new[] {order.Framework.Viewport});

                cmdBuf.CmdSetScissor(0, new[] { order.Framework.Scissor });

                cmdBuf.CmdBindDescriptorSets(MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, new[] { order.DescriptorSet }, null);

                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, mPipeline);

                cmdBuf.CmdBindVertexBuffers(0, new[] { order.Vertices.Buffer }, new[] { order.Vertices.ByteOffset });

                cmdBuf.CmdBindIndexBuffer(order.Indices.Buffer, order.Indices.ByteOffset, MgIndexType.UINT32);

                cmdBuf.CmdDrawIndexed(order.IndexCount, order.InstanceCount, 0, 0, 0);

                cmdBuf.CmdEndRenderPass();

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        private static IMgPipeline BuildPipeline(
            IMgDevice device,
            IMgPipelineLayout pipelineLayout,
            IMgEffectFramework framework,
            IOffscreenPipelineMediaPath path)
        {
            using (var vertFs = path.OpenVertexShader())
            using (var fragFs = path.OpenFragmentShader())
            {
                IMgShaderModule vsModule;
                {
                    var vsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = vertFs,
                        CodeSize = new UIntPtr((ulong)vertFs.Length),
                    };
                    device.CreateShaderModule(vsCreateInfo, null, out vsModule);
                }

                IMgShaderModule fsModule;
                {
                    var fsCreateInfo = new MgShaderModuleCreateInfo
                    {
                        Code = fragFs,
                        CodeSize = new UIntPtr((ulong)fragFs.Length),
                    };
                    device.CreateShaderModule(fsCreateInfo, null, out fsModule);
                }

                var pipelineCreateInfo = new MgGraphicsPipelineCreateInfo
                {
                    Stages = new MgPipelineShaderStageCreateInfo[]
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

                    VertexInputState = new MgPipelineVertexInputStateCreateInfo
                    {
                        VertexBindingDescriptions = new[]
                        {
                            new MgVertexInputBindingDescription
                            {
                                Binding = 0,
                                Stride = 24U,
                                InputRate = MgVertexInputRate.VERTEX,
                            }
                        },
                        VertexAttributeDescriptions = new[]
                        {
                            new MgVertexInputAttributeDescription
                            {
                                // Attribute location 0: Position
                                Binding = 0,
                                Location = 0,
                                Format =  MgFormat.R32G32B32_SFLOAT,
                                Offset = 0,
                            },
                            new MgVertexInputAttributeDescription
                            {
                                // Attribute location 1: Color
                                Binding = 1,
                                Location = 1,
                                Format = MgFormat.R32G32B32_SFLOAT,
                                Offset = 12U,
                            }
                        }
                    },

                    InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                    {
                        Topology = MgPrimitiveTopology.TRIANGLE_LIST,
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
                        Attachments = new[]
                        {
                            new MgPipelineColorBlendAttachmentState
                            {
                                ColorWriteMask =  MgColorComponentFlagBits.ALL_BITS,
                                BlendEnable = false,
                            }
                        },
                    },

                    MultisampleState = new MgPipelineMultisampleStateCreateInfo
                    {
                        RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
                        SampleMask = null,
                    },

                    Layout = pipelineLayout,

                    RenderPass = framework.Renderpass,

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

                var err = device.CreateGraphicsPipelines(
                    null,
                    new[] { pipelineCreateInfo },
                    null,
                    out IMgPipeline[] pipelines);

                Debug.Assert(err == Result.SUCCESS);

                vsModule.DestroyShaderModule(device, null);
                fsModule.DestroyShaderModule(device, null);

                return pipelines[0];
            }
        }
    }
}