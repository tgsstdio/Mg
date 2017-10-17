﻿using System;
using Magnesium;
using System.Diagnostics;
using System.IO;

namespace OffscreenDemo
{
    class ToScreenPipeline : IScreenQuadPipeline
    {
        private IToScreenPipelinePath mPath;

        public ToScreenPipeline(IToScreenPipelinePath path)
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

        private static IMgPipeline BuildPipeline(
            IMgDevice device,
            IMgEffectFramework framework,
            IToScreenPipelinePath path,
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
    }
}
