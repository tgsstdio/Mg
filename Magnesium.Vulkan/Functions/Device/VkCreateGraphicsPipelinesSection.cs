using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkCreateGraphicsPipelinesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateGraphicsPipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkGraphicsPipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

        public static MgResult CreateGraphicsPipelines(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgGraphicsPipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var bPipelineCache = (VkPipelineCache)pipelineCache;
            var bPipelineCachePtr = bPipelineCache != null ? bPipelineCache.Handle : 0UL;

            var createInfoCount = (uint)pCreateInfos.Length;

            VkGraphicsPipelineCreateInfo[] createInfos = new VkGraphicsPipelineCreateInfo[createInfoCount];

            var attachedItems = new List<IntPtr>();
            var maintainedHandles = new List<GCHandle>();

            try
            {
                for (var i = 0; i < createInfoCount; ++i)
                {
                    var current = pCreateInfos[i];

                    var bRenderPass = (VkRenderPass)current.RenderPass;
                    Debug.Assert(bRenderPass != null);

                    var bLayout = (VkPipelineLayout)current.Layout;
                    Debug.Assert(bLayout != null);

                    var bBasePipelineHandle = (VkPipeline)current.BasePipelineHandle;
                    var bBasePipelineHandlePtr = bBasePipelineHandle != null ? bBasePipelineHandle.Handle : 0UL;

                    // STAGES
                    Debug.Assert(current.Stages != null);

                    var stageCount = (uint)current.Stages.Length;
                    Debug.Assert(stageCount > 0);

                    var stageStructSize = Marshal.SizeOf(typeof(VkPipelineShaderStageCreateInfo));
                    var pStages = Marshal.AllocHGlobal((int)(stageCount * stageStructSize));
                    attachedItems.Add(pStages);

                    {
                        var offset = 0;
                        foreach (var stage in current.Stages)
                        {
                            var stageInfo = VkPipelineCreationUtility.ExtractPipelineShaderStage(attachedItems, maintainedHandles, stage);
                            IntPtr dest = IntPtr.Add(pStages, offset);
                            Marshal.StructureToPtr(stageInfo, dest, false);
                            offset += stageStructSize;
                        }
                    }

                    // pVertexInputState must be a pointer to a valid VkPipelineVertexInputStateCreateInfo structure
                    Debug.Assert(current.VertexInputState != null);
                    var pVertexInputState = ExtractVertexInputState(attachedItems, current.VertexInputState);

                    // pInputAssemblyState must be a pointer to a valid VkPipelineInputAssemblyStateCreateInfo structure
                    Debug.Assert(current.InputAssemblyState != null);
                    var pInputAssemblyState = ExtractInputAssemblyState(attachedItems, current.InputAssemblyState);

                    // pRasterizationState must be a pointer to a valid VkPipelineRasterizationStateCreateInfo structure
                    Debug.Assert(current.RasterizationState != null);
                    var pRasterizationState = ExtractRasterizationState(attachedItems, current.RasterizationState);

                    var pTessellationState = ExtractTesselationState(attachedItems, current.TessellationState);

                    var pViewportState = ExtractViewportState(attachedItems, maintainedHandles, current.ViewportState);

                    var pMultisampleState = ExtractMultisampleState(attachedItems, current.MultisampleState);

                    var pDepthStencilState = ExtractDepthStencilState(attachedItems, current.DepthStencilState);

                    var pColorBlendState = ExtractColorBlendState(attachedItems, current.ColorBlendState);

                    var pDynamicState = ExtractDynamicState(attachedItems, current.DynamicState);

                    createInfos[i] = new VkGraphicsPipelineCreateInfo
                    {
                        sType = VkStructureType.StructureTypeGraphicsPipelineCreateInfo,
                        pNext = IntPtr.Zero,
                        flags = current.Flags,
                        stageCount = stageCount,
                        pStages = pStages,
                        pVertexInputState = pVertexInputState,
                        pInputAssemblyState = pInputAssemblyState,
                        pTessellationState = pTessellationState,
                        pViewportState = pViewportState,
                        pRasterizationState = pRasterizationState,
                        pMultisampleState = pMultisampleState,
                        pDepthStencilState = pDepthStencilState,
                        pColorBlendState = pColorBlendState,
                        pDynamicState = pDynamicState,
                        layout = bLayout.Handle,
                        renderPass = bRenderPass.Handle,
                        subpass = current.Subpass,
                        basePipelineHandle = bBasePipelineHandlePtr,
                        basePipelineIndex = current.BasePipelineIndex,
                    };
                }

                var handles = new ulong[createInfoCount];
                var result = vkCreateGraphicsPipelines(info.Handle, bPipelineCachePtr, createInfoCount, createInfos, allocatorPtr, handles);

                pPipelines = new VkPipeline[createInfoCount];
                for (var i = 0; i < createInfoCount; ++i)
                {
                    pPipelines[i] = new VkPipeline(handles[i]);
                }
                return result;
            }
            finally
            {
                foreach (var item in attachedItems)
                {
                    Marshal.FreeHGlobal(item);
                }

                foreach (var handle in maintainedHandles)
                {
                    handle.Free();
                }
            }
        }

        static IntPtr ExtractVertexInputState(List<IntPtr> attachedItems, MgPipelineVertexInputStateCreateInfo current)
        {
            var vertexBindingDescriptionCount = 0U;
            var pVertexBindingDescriptions = IntPtr.Zero;
            if (current.VertexBindingDescriptions != null)
            {
                vertexBindingDescriptionCount = (uint)current.VertexBindingDescriptions.Length;
                if (vertexBindingDescriptionCount > 0)
                {
                    pVertexBindingDescriptions = VkInteropsUtility.AllocateHGlobalArray(
                        current.VertexBindingDescriptions,
                        (currentBinding) =>
                        {
                            return new VkVertexInputBindingDescription
                            {
                                binding = currentBinding.Binding,
                                stride = currentBinding.Stride,
                                inputRate = (VkVertexInputRate)currentBinding.InputRate,
                            };
                        });
                    attachedItems.Add(pVertexBindingDescriptions);
                }
            }

            var vertexAttributeDescriptionCount = 0U;
            var pVertexAttributeDescriptions = IntPtr.Zero;

            if (current.VertexAttributeDescriptions != null)
            {
                vertexAttributeDescriptionCount = (uint)current.VertexAttributeDescriptions.Length;

                if (vertexAttributeDescriptionCount > 0)
                {
                    pVertexAttributeDescriptions = VkInteropsUtility.AllocateHGlobalArray(
                        current.VertexAttributeDescriptions,
                        (attr) =>
                        {
                            return new VkVertexInputAttributeDescription
                            {
                                location = attr.Location,
                                binding = attr.Binding,
                                format = attr.Format,
                                offset = attr.Offset,
                            };
                        });
                    attachedItems.Add(pVertexAttributeDescriptions);
                }
            }

            var dataItem = new VkPipelineVertexInputStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineVertexInputStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = current.Flags,
                vertexBindingDescriptionCount = vertexBindingDescriptionCount,
                pVertexBindingDescriptions = pVertexBindingDescriptions,
                vertexAttributeDescriptionCount = vertexAttributeDescriptionCount,
                pVertexAttributeDescriptions = pVertexAttributeDescriptions,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractInputAssemblyState(List<IntPtr> attachedItems, MgPipelineInputAssemblyStateCreateInfo inputAssemblyState)
        {
            var dataItem = new VkPipelineInputAssemblyStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineInputAssemblyStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = inputAssemblyState.Flags,
                topology = (VkPrimitiveTopology)inputAssemblyState.Topology,
                primitiveRestartEnable = VkBool32.ConvertTo(inputAssemblyState.PrimitiveRestartEnable),
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractRasterizationState(List<IntPtr> attachedItems, MgPipelineRasterizationStateCreateInfo rasterizationState)
        {
            var dataItem = new VkPipelineRasterizationStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineRasterizationStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = rasterizationState.Flags,
                depthClampEnable = VkBool32.ConvertTo(rasterizationState.DepthClampEnable),
                rasterizerDiscardEnable = VkBool32.ConvertTo(rasterizationState.RasterizerDiscardEnable),
                polygonMode = (VkPolygonMode)rasterizationState.PolygonMode,
                cullMode = (VkCullModeFlags)rasterizationState.CullMode,
                frontFace = (VkFrontFace)rasterizationState.FrontFace,
                depthBiasEnable = VkBool32.ConvertTo(rasterizationState.DepthBiasEnable),
                depthBiasConstantFactor = rasterizationState.DepthBiasConstantFactor,
                depthBiasClamp = rasterizationState.DepthBiasClamp,
                depthBiasSlopeFactor = rasterizationState.DepthBiasSlopeFactor,
                lineWidth = rasterizationState.LineWidth,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractTesselationState(List<IntPtr> attachedItems, MgPipelineTessellationStateCreateInfo tessellationState)
        {
            if (tessellationState == null)
                return IntPtr.Zero;

            var dataItem = new VkPipelineTessellationStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineTessellationStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = tessellationState.Flags,
                patchControlPoints = tessellationState.PatchControlPoints,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractViewportState(List<IntPtr> attachedItems, List<GCHandle> maintainedHandles, MgPipelineViewportStateCreateInfo viewportState)
        {
            if (viewportState == null)
                return IntPtr.Zero;

            var viewportCount = 0U;
            var pViewports = IntPtr.Zero;

            if (viewportState.Viewports != null)
            {
                viewportCount = (uint)viewportState.Viewports.Length;
                if (viewportCount > 0)
                {
                    var pinnedArray = GCHandle.Alloc(viewportState.Viewports, GCHandleType.Pinned);
                    maintainedHandles.Add(pinnedArray);
                    pViewports = pinnedArray.AddrOfPinnedObject();
                }
            }

            var scissorCount = 0U;
            var pScissors = IntPtr.Zero;

            if (viewportState.Scissors != null)
            {
                scissorCount = (uint)viewportState.Scissors.Length;
                if (scissorCount > 0)
                {
                    var pinnedArray = GCHandle.Alloc(viewportState.Scissors, GCHandleType.Pinned);
                    maintainedHandles.Add(pinnedArray);
                    pScissors = pinnedArray.AddrOfPinnedObject();
                }
            }

            var dataItem = new VkPipelineViewportStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineViewportStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = viewportState.Flags,
                viewportCount = viewportCount,
                pViewports = pViewports,
                scissorCount = scissorCount,
                pScissors = pScissors,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractMultisampleState(List<IntPtr> attachedItems, MgPipelineMultisampleStateCreateInfo multisample)
        {
            if (multisample == null)
                return IntPtr.Zero;

            var pSampleMask = IntPtr.Zero;
            if (multisample.SampleMask != null)
            {
                if (multisample.SampleMask.Length > 0)
                {
                    pSampleMask = VkInteropsUtility.AllocateUInt32Array(multisample.SampleMask);
                    attachedItems.Add(pSampleMask);
                }
            }

            var dataItem = new VkPipelineMultisampleStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineMultisampleStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = multisample.Flags,
                rasterizationSamples = multisample.RasterizationSamples,
                sampleShadingEnable = VkBool32.ConvertTo(multisample.SampleShadingEnable),
                minSampleShading = multisample.MinSampleShading,
                pSampleMask = pSampleMask,
                alphaToCoverageEnable = VkBool32.ConvertTo(multisample.AlphaToCoverageEnable),
                alphaToOneEnable = VkBool32.ConvertTo(multisample.AlphaToOneEnable),
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractDepthStencilState(List<IntPtr> attachedItems, MgPipelineDepthStencilStateCreateInfo depthStencilState)
        {
            if (depthStencilState == null)
                return IntPtr.Zero;

            var dataItem = new VkPipelineDepthStencilStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineDepthStencilStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = depthStencilState.Flags,
                depthTestEnable = VkBool32.ConvertTo(depthStencilState.DepthTestEnable),
                depthWriteEnable = VkBool32.ConvertTo(depthStencilState.DepthWriteEnable),
                depthCompareOp = (VkCompareOp)depthStencilState.DepthCompareOp,
                depthBoundsTestEnable = VkBool32.ConvertTo(depthStencilState.DepthBoundsTestEnable),
                stencilTestEnable = VkBool32.ConvertTo(depthStencilState.StencilTestEnable),
                front = new VkStencilOpState
                {
                    failOp = (VkStencilOp)depthStencilState.Front.FailOp,
                    passOp = (VkStencilOp)depthStencilState.Front.PassOp,
                    depthFailOp = (VkStencilOp)depthStencilState.Front.DepthFailOp,
                    compareOp = (VkCompareOp)depthStencilState.Front.CompareOp,
                    compareMask = depthStencilState.Front.CompareMask,
                    writeMask = depthStencilState.Front.WriteMask,
                    reference = depthStencilState.Front.Reference,
                },
                back = new VkStencilOpState
                {
                    failOp = (VkStencilOp)depthStencilState.Back.FailOp,
                    passOp = (VkStencilOp)depthStencilState.Back.PassOp,
                    depthFailOp = (VkStencilOp)depthStencilState.Back.DepthFailOp,
                    compareOp = (VkCompareOp)depthStencilState.Back.CompareOp,
                    compareMask = depthStencilState.Back.CompareMask,
                    writeMask = depthStencilState.Back.WriteMask,
                    reference = depthStencilState.Back.Reference,
                },
                minDepthBounds = depthStencilState.MinDepthBounds,
                maxDepthBounds = depthStencilState.MaxDepthBounds,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractColorBlendState(List<IntPtr> attachedItems, MgPipelineColorBlendStateCreateInfo colorBlendState)
        {
            if (colorBlendState == null)
                return IntPtr.Zero;

            var pAttachments = IntPtr.Zero;
            var attachmentCount = 0U;

            if (colorBlendState.Attachments != null)
            {
                attachmentCount = (uint)colorBlendState.Attachments.Length;
                if (attachmentCount > 0)
                {
                    pAttachments = VkInteropsUtility.AllocateHGlobalArray(
                        colorBlendState.Attachments,
                        (item) =>
                        {
                            return new VkPipelineColorBlendAttachmentState
                            {
                                blendEnable = VkBool32.ConvertTo(item.BlendEnable),
                                srcColorBlendFactor = (VkBlendFactor)item.SrcColorBlendFactor,
                                dstColorBlendFactor = (VkBlendFactor)item.DstColorBlendFactor,
                                colorBlendOp = (VkBlendOp)item.ColorBlendOp,
                                srcAlphaBlendFactor = (VkBlendFactor)item.SrcAlphaBlendFactor,
                                dstAlphaBlendFactor = (VkBlendFactor)item.DstAlphaBlendFactor,
                                alphaBlendOp = (VkBlendOp)item.AlphaBlendOp,
                                colorWriteMask = (VkColorComponentFlags)item.ColorWriteMask,
                            };
                        });
                    attachedItems.Add(pAttachments);
                }
            }

            var dataItem = new VkPipelineColorBlendStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineColorBlendStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = colorBlendState.Flags,
                logicOpEnable = VkBool32.ConvertTo(colorBlendState.LogicOpEnable),
                logicOp = (VkLogicOp)colorBlendState.LogicOp,
                attachmentCount = attachmentCount,
                pAttachments = pAttachments,
                blendConstants = colorBlendState.BlendConstants,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }

        static IntPtr ExtractDynamicState(List<IntPtr> attachedItems, MgPipelineDynamicStateCreateInfo dynamicState)
        {
            if (dynamicState == null)
                return IntPtr.Zero;

            var dynamicStateCount = 0U;
            var pDynamicStates = IntPtr.Zero;

            if (dynamicState.DynamicStates != null)
            {
                dynamicStateCount = (uint)dynamicState.DynamicStates.Length;
                if (dynamicStateCount > 0)
                {
                    var bufferSize = (int)(dynamicStateCount * sizeof(int));
                    pDynamicStates = Marshal.AllocHGlobal(bufferSize);

                    var tempData = new int[dynamicStateCount];
                    for (var i = 0; i < dynamicStateCount; ++i)
                    {
                        tempData[i] = (int)dynamicState.DynamicStates[i];
                    }

                    Marshal.Copy(tempData, 0, pDynamicStates, (int)dynamicStateCount);

                    attachedItems.Add(pDynamicStates);
                }
            }

            var dataItem = new VkPipelineDynamicStateCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineDynamicStateCreateInfo,
                pNext = IntPtr.Zero,
                flags = dynamicState.Flags,
                dynamicStateCount = dynamicStateCount,
                pDynamicStates = pDynamicStates,
            };

            {
                var structSize = Marshal.SizeOf(dataItem);
                var dest = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(dataItem, dest, false);
                attachedItems.Add(dest);
                return dest;
            }
        }
    }
}
