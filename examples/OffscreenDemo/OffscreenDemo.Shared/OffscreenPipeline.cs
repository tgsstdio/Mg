﻿using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    public class OffscreenPipeline : IScreenQuadPipeline
    {
        private IOffscreenDemoShaderPath mTrianglePath;
        public OffscreenPipeline(IOffscreenDemoShaderPath path)
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

        private IMgDescriptorSetLayout mDescriptorSetLayout;
        private IMgPipelineLayout mPipelineLayout;
        private IMgPipeline mPipeline;
        public void Initialize(IMgGraphicsConfiguration configuration, IMgEffectFramework framework)
        {
            var device = configuration.Device;
            Debug.Assert(device != null);

            mDescriptorSetLayout = SetupDescriptorSetLayout(device);
            mPipelineLayout = SetupPipelineLayout(device, mDescriptorSetLayout);
            mPipeline = BuildPipeline(device, mPipelineLayout, framework, mTrianglePath);
        }

        class DrawItem
        {
            public IMgEffectFramework Framework { get; set; }
            public IMgCommandBuffer[] drawCmdBuffers { get; set; }
            public IMgDescriptorSet DescriptorSet { get; set; }
            public IMgBuffer Vertices { get; set; }
            public IMgBuffer Indices { get; set; }
            public uint IndexCount { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TriangleVertex
        {
            public Vector3 position;
            public Vector3 color;
        };

        class StagingLevel
        {
            public MgStagingBuffer Vertices { get; set; }
            public MgStagingBuffer Indices { get; set; }
        }

        void PrepareVertices(IMgGraphicsConfiguration configuration)
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

            var stagingBuffers = new StagingLevel
            {
                Vertices = new MgStagingBuffer(configuration, vertexBufferSize),
                Indices = new MgStagingBuffer(configuration, indexBufferSize),
            };

            // DEVICE_LOCAL vertex buffer

            // DEVICE_LOCAL index buffer 

            TransferToDeviceLocal(configuration, stagingBuffers);
        }

        void TransferToDeviceLocal(IMgGraphicsConfiguration configuration, StagingLevel stagingBuffers)
        {
            // TRANSFER DATA
            IMgCommandBuffer copyCmd = null;
            IMgBuffer dstBuffer = null;

            var cmdBufInfo = new MgCommandBufferBeginInfo { };

            var err = copyCmd.BeginCommandBuffer(cmdBufInfo);
            Debug.Assert(err == Result.SUCCESS);

            ulong vertexOffset = 0UL;
            stagingBuffers.Vertices.Transfer(
                copyCmd,
                dstBuffer,
                vertexOffset);

            ulong indexOffset = 0UL;
            stagingBuffers.Indices.Transfer(
                copyCmd,
                dstBuffer,
                indexOffset);

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

            stagingBuffers.Vertices.Destroy(configuration.Device);
            stagingBuffers.Indices.Destroy(configuration.Device);
        }

        public void BuildCommandBuffers(
            IMgEffectFramework framework,             
            IMgCommandBuffer[] drawCmdBuffers,
            IMgDescriptorSet descriptorSet,
            IMgBuffer vertices,
            IMgBuffer indices,
            uint indexCount
         )
        {
            var colorFormat = framework.RenderpassInfo.Attachments[0].Format;

            var renderPassBeginInfo = new MgRenderPassBeginInfo
            {
                RenderPass = framework.Renderpass,
                RenderArea = framework.Scissor,
                ClearValues = new MgClearValue[]
                {
                    MgClearValue.FromColorAndFormat(colorFormat, new MgColor4f(0f, 0f, 0f, 0f)),
                    new MgClearValue { DepthStencil = new MgClearDepthStencilValue( 1.0f, 0) },
                },
            };

            for (var i = 0; i < drawCmdBuffers.Length; ++i)
            {
                renderPassBeginInfo.Framebuffer = framework.Framebuffers[i];

                var cmdBuf = drawCmdBuffers[i];

                var cmdBufInfo = new MgCommandBufferBeginInfo { };
                var err = cmdBuf.BeginCommandBuffer(cmdBufInfo);
                Debug.Assert(err == Result.SUCCESS);

                cmdBuf.CmdBeginRenderPass(renderPassBeginInfo, MgSubpassContents.INLINE);


                cmdBuf.CmdSetViewport(0, new[] {framework.Viewport});

                cmdBuf.CmdSetScissor(0, new[] { framework.Scissor });

                cmdBuf.CmdBindDescriptorSets(MgPipelineBindPoint.GRAPHICS, mPipelineLayout, 0, new[] { descriptorSet }, null);

                cmdBuf.CmdBindPipeline(MgPipelineBindPoint.GRAPHICS, mPipeline);

                cmdBuf.CmdBindVertexBuffers(0, new[] { vertices }, new[] { 0UL });

                cmdBuf.CmdBindIndexBuffer(indices, 0, MgIndexType.UINT32);

                cmdBuf.CmdDrawIndexed(indexCount, 1, 0, 0, 1);

                cmdBuf.CmdEndRenderPass();

                err = cmdBuf.EndCommandBuffer();
                Debug.Assert(err == Result.SUCCESS);
            }
        }

        private static IMgPipeline BuildPipeline(
            IMgDevice device,
            IMgPipelineLayout pipelineLayout,
            IMgEffectFramework framework,
            IOffscreenDemoShaderPath path)
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