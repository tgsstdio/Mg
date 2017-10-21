using System;
using Magnesium;
using System.Diagnostics;
using System.IO;
using Magnesium.Utilities;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class ToScreenPipeline
    {
        private IToScreenPipelineMediaPath mPath;

        public ToScreenPipeline(IToScreenPipelineMediaPath path)
        {
            mPath = path;
        }

        private IMgDescriptorSetLayout mDescriptorSetLayout;
        private IMgPipelineLayout mPipelineLayout;
        private IMgPipeline mPipeline;

        public void Initialize(IMgGraphicsConfiguration configuration, IMgEffectFramework framework)
        {
            var device = configuration.Device;
            Debug.Assert(device != null);

            mDescriptorSetLayout = SetupDescriptorSetLayout(device);
            mPipelineLayout = SetupPipelineLayout(device, mDescriptorSetLayout);
            mPipeline = BuildPipeline(device, framework, mPath, mPipelineLayout);
        }

        internal void ReleaseUnmanagedResources(IMgGraphicsConfiguration configuration)
        {
            var device = configuration.Device;
            Debug.Assert(device != null);

            if (mPipeline != null)
                mPipeline.DestroyPipeline(device, null);
            if (mPipelineLayout != null)
                mPipelineLayout.DestroyPipelineLayout(device, null);
            if (mDescriptorSetLayout != null)
                mDescriptorSetLayout.DestroyDescriptorSetLayout(device, null);
        }

        private static IMgDescriptorSetLayout SetupDescriptorSetLayout(IMgDevice device)
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
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

            var err = device.CreateDescriptorSetLayout(descriptorLayout, null, out IMgDescriptorSetLayout dSetLayout);
            Debug.Assert(err == Result.SUCCESS);
            return dSetLayout;
        }

        private static IMgPipelineLayout SetupPipelineLayout(IMgDevice device, IMgDescriptorSetLayout dSetLayout)
        {
            var pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                SetLayouts = new[] { dSetLayout },
            };

            var err = device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out IMgPipelineLayout pLayout);
            Debug.Assert(err == Result.SUCCESS);
            return pLayout;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct VertexData
        {
            public Vector3 pos;
            public Vector2 uv;
            public Vector3 normal;
        };

        public void Populate(MgOptimizedStorage storage, IMgGraphicsConfiguration configuration)
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

            // DEVICE_LOCAL vertex buffer
            SetData(storage, configuration.Device, mVertexDataPosition, quadCorners, 0, quadCorners.Length);

            // Setup indices
            var indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            SetIndicesU32(storage, configuration.Device, mIndexDataPosition, indices, 0, indices.Length);
        }

        public Result SetData<TData>(
            MgOptimizedStorage storage,
            IMgDevice device,
            int location,
            TData[] srcData,
            int srcFirst,
            int count)
            where TData : struct
        {
            ValidateParameters(device, srcData);

            int stride = Marshal.SizeOf(typeof(TData));

            var allocationInfo = storage.Allocations[location];
            ulong sizeInBytes = (ulong)(count * stride);
            ValidateRange(allocationInfo, sizeInBytes);

            var block = storage.Blocks[allocationInfo.BlockIndex];

            var err = block.DeviceMemory.MapMemory(
                device,
                allocationInfo.Offset,
                allocationInfo.Size,
                0, out IntPtr dest);
            if (err != Result.SUCCESS)
            {
                return err;
            }

            // Copy the struct to unmanaged memory.	
            int offset = 0;
            for (int i = 0; i < count; ++i)
            {
                IntPtr localDest = IntPtr.Add(dest, offset);
                Marshal.StructureToPtr(srcData[i + srcFirst], localDest, false);
                offset += stride;
            }

            block.DeviceMemory.UnmapMemory(device);

            return Result.SUCCESS;
        }

        private static void ValidateParameters<TData>(IMgDevice device, TData[] srcData) where TData : struct
        {
            if (device == null)
                throw new ArgumentNullException("device");

            if (srcData == null)
                throw new ArgumentNullException("srcData");
        }

        private Result SetIndicesU32(
            MgOptimizedStorage storage,
            IMgDevice device, 
            int location,
            uint[] srcData,
            int first,
            int count)
        {
            ValidateParameters(device, srcData);

            int stride = Marshal.SizeOf(typeof(uint));

            var allocationInfo = storage.Allocations[location];
            ulong sizeInBytes = (ulong)(count * stride);
            ValidateRange(allocationInfo, sizeInBytes);

            var block = storage.Blocks[allocationInfo.BlockIndex];

            var err = block.DeviceMemory.MapMemory(
                device,
                allocationInfo.Offset,
                allocationInfo.Size,
                0, out IntPtr dest);

            if (err != Result.SUCCESS)
            {
                return err;
            }

            var localData = new byte[sizeInBytes];

            var startOffset = first * stride;
            var totalBytesToCopy = (int)sizeInBytes;
            Buffer.BlockCopy(srcData, startOffset, localData, 0, totalBytesToCopy);

            Marshal.Copy(localData, first, dest, totalBytesToCopy);

            block.DeviceMemory.UnmapMemory(device);

            return Result.SUCCESS;
        }

        private static void ValidateRange(MgOptimizedStorageAllocation allocationInfo, ulong sizeInBytes)
        {
            if (allocationInfo.Size < sizeInBytes)
            {
                throw new ArgumentOutOfRangeException("sizeInBytes");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UniformBufferObject
        {
            public Vector3 viewPos;
            public float lodBias;
            public Matrix4 projection;
            public Matrix4 model;
        }

        private int mVertexDataPosition;
        private int mIndexDataPosition;
        private int mUniformDataPosition;
        public void Reserve(MgBlockAllocationList slots)
        {
            // Vertex buffer
            {
                var structSize = Marshal.SizeOf(typeof(VertexData));
                var vertices = new MgStorageBlockAllocationInfo
                {
                    Size = (ulong)(4 * structSize),
                    ElementByteSize = (uint)structSize,
                    MemoryPropertyFlags =
                        MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                        | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT,
                };
                mVertexDataPosition = slots.Insert(vertices);
            }

            // index buffer
            {
                var indexElementSize = (uint)sizeof(uint);
                var indices = new MgStorageBlockAllocationInfo
                {
                    Size = (ulong)(6 * indexElementSize),
                    ElementByteSize = indexElementSize,
                    MemoryPropertyFlags = 
                        MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                        | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                };
                mIndexDataPosition = slots.Insert(indices);
            }

            // Index buffer
            {
                var uniformSize = (uint)Marshal.SizeOf(typeof(UniformBufferObject));
                var uniforms = new MgStorageBlockAllocationInfo
                {
                    Size = (ulong)uniformSize,
                    ElementByteSize = uniformSize,
                    MemoryPropertyFlags = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                    | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT,
                    Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                };
                mUniformDataPosition = slots.Insert(uniforms);
            }
        }

        public void BuildCommandBuffers(object secondOrder)
        {
            throw new NotImplementedException();
        }

        public void BuildCommandBuffers(
            MgCommandBuildOrder order
            //IMgEffectFramework framework,
            //IMgCommandBuffer[] drawCmdBuffers,
            //IMgDevice device,
            //IMgPipeline pipeline,
            //IMgPipelineLayout pipelineLayout,
            //IMgDescriptorSet descSet,
            //IMgBuffer vertices,
            //IMgBuffer indices,
            //uint indexCount
        )
        {
            var cmdBufInfo = new MgCommandBufferBeginInfo
            {
                
            };

            var colorFormat = order.Framework.RenderpassInfo.Attachments[0].Format;

            var renderPassBeginInfo = new MgRenderPassBeginInfo
            {
                RenderPass = order.Framework.Renderpass,
                RenderArea = order.Framework.Scissor,
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(colorFormat, new MgColor4f(0f, 0f, 0f, 0f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue(1024f, 0) }
                },
            };

            for (var i = 0; i < order.Count; ++i)
            {
                var index = order.First + i;

                // Set target frame buffer
                renderPassBeginInfo.Framebuffer = order.Framework.Framebuffers[index];

                var cmdBuf = order.CommandBuffers[index];

                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);

                cmdBuf.CmdSetViewport(0, new[] { order.Framework.Viewport });

                cmdBuf.CmdSetScissor(0, new[] { order.Framework.Scissor });

                cmdBuf.CmdBindDescriptorSets(MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, new [] { order.DescriptorSet }, null);
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
            IMgEffectFramework framework,
            IToScreenPipelineMediaPath path,
            IMgPipelineLayout pipelineLayout
        )
        {
            IMgShaderModule vertSM = null;
            IMgShaderModule fragSM = null;

            try
            {                
                // System.IO.File.OpenRead("Shaders/texture1.vert.spv")
                using (var vertFs = path.OpenVertexShader())
                // System.IO.File.OpenRead("Shaders/texture1.frag.spv")
                using (var fragFs = path.OpenFragmentShader())
                {
                    // Load shaders
                    vertSM = InitShaderModule(device, vertFs);
                    fragSM = InitShaderModule(device, fragFs);

                    var pipelineCreateInfo = new MgGraphicsPipelineCreateInfo
                    {
                        Stages = new[]
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

                        Layout = pipelineLayout,
                        RenderPass = framework.Renderpass,
                        VertexInputState = new MgPipelineVertexInputStateCreateInfo
                        {
                            VertexBindingDescriptions = new[]
                            {
                            new MgVertexInputBindingDescription
                            {
                                Binding = 0,
                                InputRate = MgVertexInputRate.VERTEX,
                                Stride = 32U,
                            }
                        },
                            VertexAttributeDescriptions = new[]
                            {
                            // Location 0 : Position
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0,
                                Location = 0,
                                Format = MgFormat.R32G32B32_SFLOAT,
                                Offset = 0U,
                            },
                            // Location 1 : Texture coordinates
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0,
                                Location = 1,
                                Format = MgFormat.R32G32_SFLOAT,
                                Offset = 12U,
                            },
                            // Location 2 : Vertex normal
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0,
                                Location = 2,
                                Format = MgFormat.R32G32B32_SFLOAT,
                                Offset = 20U,
                            },
                        },
                        },

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
                            Attachments = new[]
                            {
                            new MgPipelineColorBlendAttachmentState
                            {
                                ColorWriteMask = MgColorComponentFlagBits.ALL_BITS,
                                BlendEnable = false,
                            },
                        }
                        },

                        MultisampleState = new MgPipelineMultisampleStateCreateInfo
                        {
                            RasterizationSamples = MgSampleCountFlagBits.COUNT_1_BIT,
                        },

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

                    var err = device.CreateGraphicsPipelines(null, new[] { pipelineCreateInfo }, null, out IMgPipeline[] pipelines);
                    Debug.Assert(err == Result.SUCCESS);

                    return pipelines[0];
                }
            }
            finally
            {
                if (vertSM != null)
                    vertSM.DestroyShaderModule(device, null);

                if (fragSM != null)
                    fragSM.DestroyShaderModule(device, null);
            }
        }

        private static IMgShaderModule InitShaderModule(IMgDevice device, Stream vertFs)
        {
            IMgShaderModule vertSM;
            {
                var vsCreateInfo = new MgShaderModuleCreateInfo
                {
                    Code = vertFs,
                    CodeSize = new UIntPtr((ulong)vertFs.Length),
                };
                var localErr = device.CreateShaderModule(vsCreateInfo, null, out vertSM);
                Debug.Assert(localErr == Result.SUCCESS);
            }

            return vertSM;
        }

        public MgCommandBuildOrder GenerateBuildOrder(MgOptimizedStorage storage)
        {
            throw new NotImplementedException();
        }

        public void Populate(MgOptimizedStorage storage, IMgGraphicsConfiguration configuration, IMgCommandBuffer copyCmd)
        {
            throw new NotImplementedException();
        }
    }
}
