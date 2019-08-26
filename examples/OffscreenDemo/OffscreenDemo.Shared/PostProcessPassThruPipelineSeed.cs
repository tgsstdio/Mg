﻿using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OffscreenDemo
{
    [StructLayout(LayoutKind.Sequential)]
    public struct QuadVertexData
    {
        public TkVector2 TexCoord;
        public TkVector2 Position;
    }

    internal class PostProcessPassThruPipelineSeed : IMgPipelineSeed
    {
        private IPostProcessingPassThruMediaPath mPath;
        public PostProcessPassThruPipelineSeed(IPostProcessingPassThruMediaPath path)
        {
            mPath = path;
        }

        public IMgPipeline BuildPipeline(IMgDevice device, IMgPipelineLayout layout, IMgEffectFramework framework)
        {
            var dataStride = Marshal.SizeOf(typeof(QuadVertexData));

            const int POSITION_ATTRIB = 0;
            const int UV_ATTRIB = 1;

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
                                Format= MgFormat.R32G32_SFLOAT,
                                Offset =  (uint) Marshal.OffsetOf(typeof(QuadVertexData), "Position"),
                            },
                            new MgVertexInputAttributeDescription
                            {
                                Binding = 0U,
                                Location = UV_ATTRIB,
                                Format= MgFormat.R32G32_SFLOAT,
                                Offset = (uint) Marshal.OffsetOf(typeof(QuadVertexData), "TexCoord"),
                            },
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

                Debug.Assert(err == MgResult.SUCCESS);

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
                        Type = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                        DescriptorCount = 3,
                    },
                    new MgDescriptorPoolSize
                    {
                        Type = MgDescriptorType.UNIFORM_BUFFER,
                        DescriptorCount = 1,
                    },
                },
                MaxSets = 1,
            };
            var err = device.CreateDescriptorPool(descriptorPoolInfo, null, out IMgDescriptorPool descriptorPool);
            Debug.Assert(err == MgResult.SUCCESS);
            return descriptorPool;
        }

        public IMgDescriptorSetLayout SetupDescriptorSetLayout(IMgDevice device)
        {
            var descriptorLayout = new MgDescriptorSetLayoutCreateInfo
            {
                Bindings = new[]
                {
                    // Binding 0: diffuseTex
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorCount = 1,
                        StageFlags = MgShaderStageFlagBits.FRAGMENT_BIT,
                        ImmutableSamplers = null,
                        DescriptorType = MgDescriptorType.COMBINED_IMAGE_SAMPLER,
                        Binding = 0,
                    },
                    // Binding 1: Uniform buffer (Vertex shader)
                    new MgDescriptorSetLayoutBinding
                    {
                        DescriptorCount = 1,
                        StageFlags = MgShaderStageFlagBits.VERTEX_BIT,
                        ImmutableSamplers = null,
                        DescriptorType = MgDescriptorType.UNIFORM_BUFFER,
                        Binding = 1,
                    }
                },
            };

            var err = device.CreateDescriptorSetLayout(descriptorLayout, null, out IMgDescriptorSetLayout setLayout);
            Debug.Assert(err == MgResult.SUCCESS);
            return setLayout;
        }
    }
}