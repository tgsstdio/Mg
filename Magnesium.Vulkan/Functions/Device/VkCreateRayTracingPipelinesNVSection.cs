using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateRayTracingPipelinesNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateRayTracingPipelinesNV(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkRayTracingPipelineCreateInfoNV[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

        public static MgResult CreateRayTracingPipelinesNV(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgRayTracingPipelineCreateInfoNV[] pCreateInfos, IMgAllocationCallbacks pAllocator, out IMgPipeline[] pPipelines)
        {
            var allocatedItems = new List<IntPtr>();
            var gcHandles = new List<GCHandle>();

            var bPipelineCache = (VkPipelineCache)pipelineCache;
            var bPipelineCachePtr = bPipelineCache != null ? bPipelineCache.Handle : 0UL;

            var createInfoCount = (uint)pCreateInfos.Length;

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(pAllocator);

            var bCreateInfos = new VkRayTracingPipelineCreateInfoNV[createInfoCount];

            try
            {
                for (var i = 0; i < createInfoCount; i += 1)
                {
                    var current = pCreateInfos[i];

                    var stageCount = (current.Stages != null)
                        ? (uint)current.Stages.Length
                        : 0U;
                    var pStages = IntPtr.Zero;

                    if (stageCount > 0)
                    {
                        var bStages = new VkPipelineShaderStageCreateInfo[stageCount];
                        for (var j = 0; j < stageCount; j += 1)
                        {
                            bStages[j] = VkPipelineCreationUtility.ExtractPipelineShaderStage(
                                allocatedItems,
                                gcHandles,
                                current.Stages[j]);
                        }

                        pStages = VkInteropsUtility.AllocateHGlobalStructArray(bStages);
                        allocatedItems.Add(pStages);
                    }

                    var groupCount = (current.Groups != null)
                        ? (uint)current.Groups.Length
                        : 0U;

                    var pGroups = IntPtr.Zero;

                    if (groupCount > 0)
                    {
                        pGroups = VkInteropsUtility.AllocateHGlobalArray(
                        current.Groups,
                        (src) => {
                            return new VkRayTracingShaderGroupCreateInfoNV
                            {
                                sType = VkStructureType.StructureTypeRayTracingShaderGroupCreateInfoNv,
                                pNext = IntPtr.Zero,
                                type = src.Type,
                                generalShader = src.GeneralShader,
                                closestHitShader = src.ClosestHitShader,
                                anyHitShader = src.AnyHitShader,
                                intersectionShader = src.IntersectionShader,
                            };
                        });
                        allocatedItems.Add(pGroups);
                    }

                    var bLayout = (VkPipelineLayout)current.Layout;
                    var bLayoutPtr = bLayout != null ? bLayout.Handle : 0UL;

                    var bBasePipelineHandle = (VkPipeline)current.BasePipelineHandle;
                    var bBasePipelineHandlePtr = bBasePipelineHandle != null
                        ? bBasePipelineHandle.Handle
                        : 0UL;

                    bCreateInfos[i] = new VkRayTracingPipelineCreateInfoNV
                    {
                        sType = VkStructureType.StructureTypeRayTracingPipelineCreateInfoNv,
                        pNext = IntPtr.Zero,
                        flags = current.Flags,
                        stageCount = stageCount,
                        pStages = pStages,
                        groupCount = groupCount,
                        pGroups = pGroups,
                        maxRecursionDepth = current.MaxRecursionDepth,
                        layout = bLayoutPtr,
                        basePipelineHandle = bBasePipelineHandlePtr,
                        basePipelineIndex = current.BasePipelineIndex,
                    };
                }

                var handles = new ulong[createInfoCount];
                var result = vkCreateRayTracingPipelinesNV(
                    info.Handle,
                    bPipelineCachePtr,
                    createInfoCount,
                    bCreateInfos,
                    allocatorPtr,
                    handles);

                pPipelines = new VkPipeline[createInfoCount];
                for (var i = 0; i < createInfoCount; ++i)
                {
                    pPipelines[i] = new VkPipeline(handles[i]);
                }
                return result;
            }
            finally
            {
                foreach (var item in allocatedItems)
                {
                    Marshal.FreeHGlobal(item);
                }

                foreach (var handle in gcHandles)
                {
                    handle.Free();
                }
            }
        }
    }
}
