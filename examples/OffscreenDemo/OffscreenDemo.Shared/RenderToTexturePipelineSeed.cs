using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    internal class RenderToTexturePipelineSeed : IMgPipelineSeed
    {
        private IRenderToTexturePipelineMediaPath mPath;
        public RenderToTexturePipelineSeed(IRenderToTexturePipelineMediaPath path)
        {
            mPath = path;
        }

        public IMgPipeline BuildPipeline(IMgDevice device, IMgPipelineLayout layout, IMgEffectFramework framework)
        {
            var dataStride = Marshal.SizeOf(typeof(VertexT2fN3fV3f));

            const int POSITION_ATTRIB = 0;
            const int NORMAL_ATTRIB = 1;
            const int UV_ATTRIB = 2;

            using (var vertFs = mPath.OpenVertexShader())
            using (var fragFs = mPath.OpenFragmentShader())
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
                                Binding = 0U,
                                InputRate = MgVertexInputRate.VERTEX,
                                Stride = (uint) dataStride,
                            }
                        },
                        VertexAttributeDescriptions = new[]
                        {
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0U,
                                Location = POSITION_ATTRIB,
                                Format= MgFormat.R32G32B32_SFLOAT,
                                Offset =  (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "Position"),
                            },
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0U,
                                Location = NORMAL_ATTRIB,
                                Format= MgFormat.R32G32B32_SFLOAT,
                                Offset = (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "Normal"),
                            },
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0U,
                                Location = UV_ATTRIB,
                                Format= MgFormat.R32G32_SFLOAT,
                                Offset = (uint) Marshal.OffsetOf(typeof(VertexT2fN3fV3f), "TexCoord"),
                            },
                         }
                    },

                    InputAssemblyState = new MgPipelineInputAssemblyStateCreateInfo
                    {
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

                    Layout = layout,

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

        public IMgDescriptorPool SetupDescriptorPool(IMgDevice device)
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
            var err = device.CreateDescriptorPool(descriptorPoolInfo, null, out IMgDescriptorPool descriptorPool);
            Debug.Assert(err == Result.SUCCESS);
            return descriptorPool;
        }

        public IMgDescriptorSetLayout SetupDescriptorSetLayout(IMgDevice device)
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
                {
                    // Binding 2: Uniform buffer (Vertex shader)
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorCount = 1,
                        StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
                        ImmutableSamplers = null,
                        DescriptorType = MgDescriptorType.UNIFORM_BUFFER,
                        Binding = 2,
                    }
                },
            };

            var err = device.CreateDescriptorSetLayout(descriptorLayout, null, out IMgDescriptorSetLayout setLayout);
            Debug.Assert(err == Result.SUCCESS);
            return setLayout;
        }
    }
}