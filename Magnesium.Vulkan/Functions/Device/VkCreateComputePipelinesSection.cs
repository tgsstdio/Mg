using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateComputePipelinesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateComputePipelines(IntPtr device, UInt64 pipelineCache, UInt32 createInfoCount, [In, Out] VkComputePipelineCreateInfo[] pCreateInfos, IntPtr pAllocator, [In, Out] UInt64[] pPipelines);

        public static MgResult CreateComputePipelines(VkDeviceInfo info, IMgPipelineCache pipelineCache, MgComputePipelineCreateInfo[] pCreateInfos, IMgAllocationCallbacks allocator, out IMgPipeline[] pPipelines)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var bPipelineCache = (VkPipelineCache)pipelineCache;
            var bPipelineCachePtr = bPipelineCache != null ? bPipelineCache.Handle : 0UL;

            var createInfoCount = (uint)pCreateInfos.Length;

            var attachedItems = new List<IntPtr>();
            var maintainedHandles = new List<GCHandle>();
            try
            {
                var createInfos = new VkComputePipelineCreateInfo[createInfoCount];
                for (var i = 0; i < createInfoCount; ++i)
                {
                    var currentCreateInfo = pCreateInfos[i];
                    var pStage = VkPipelineCreationUtility.ExtractPipelineShaderStage(attachedItems, maintainedHandles, currentCreateInfo.Stage);

                    var bBasePipeline = (VkPipeline)currentCreateInfo.BasePipelineHandle;
                    var basePipelineHandle = bBasePipeline != null ? bBasePipeline.Handle : 0UL;

                    var bPipelineLayout = (VkPipelineLayout)currentCreateInfo.Layout;
                    Debug.Assert(bPipelineLayout != null);

                    createInfos[i] = new VkComputePipelineCreateInfo
                    {
                        sType = VkStructureType.StructureTypeComputePipelineCreateInfo,
                        pNext = IntPtr.Zero,
                        flags = currentCreateInfo.Flags,
                        stage = pStage,
                        layout = bPipelineLayout.Handle,
                        basePipelineHandle = basePipelineHandle,
                        basePipelineIndex = currentCreateInfo.BasePipelineIndex,
                    };
                }

                var handles = new ulong[createInfoCount];
                var result = vkCreateComputePipelines(info.Handle, bPipelineCachePtr, createInfoCount, createInfos, allocatorPtr, handles);

                pPipelines = new VkPipeline[createInfoCount];
                for (var i = 0; i < createInfoCount; ++i)
                {
                    pPipelines[i] = new VkPipeline(handles[i]);
                }
                return result;
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }

                foreach (var pin in maintainedHandles)
                {
                    pin.Free();
                }
            }
        }
    }
}
