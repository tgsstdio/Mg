using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
    internal class VkPipelineCreationUtility
    {
        internal static VkPipelineShaderStageCreateInfo ExtractPipelineShaderStage(List<IntPtr> attachedItems, List<GCHandle> handles, MgPipelineShaderStageCreateInfo currentStage)
        {
            Debug.Assert(currentStage != null);

            var bModule = (VkShaderModule)currentStage.Module;
            Debug.Assert(bModule != null);

            // pointer to a null-terminated UTF-8 string specifying the entry point name of the shader for this stage
            Debug.Assert(!string.IsNullOrWhiteSpace(currentStage.Name));
            var pName = VkInteropsUtility.NativeUtf8FromString(currentStage.Name);
            attachedItems.Add(pName);

            var pSpecializationInfo = IntPtr.Zero;

            if (currentStage.SpecializationInfo != null)
            {
                var mapEntryCount = 0U;
                var pMapEntries = IntPtr.Zero;

                if (currentStage.SpecializationInfo.MapEntries != null)
                {
                    mapEntryCount = (uint)currentStage.SpecializationInfo.MapEntries.Length;
                    if (mapEntryCount > 0)
                    {
                        var pinnedArray = GCHandle.Alloc(currentStage.SpecializationInfo.MapEntries, GCHandleType.Pinned);
                        handles.Add(pinnedArray);

                        pMapEntries = pinnedArray.AddrOfPinnedObject();
                    }
                }

                pSpecializationInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VkSpecializationInfo)));
                attachedItems.Add(pSpecializationInfo);

                var spInfo = new VkSpecializationInfo
                {
                    mapEntryCount = mapEntryCount,
                    pMapEntries = pMapEntries,
                    dataSize = currentStage.SpecializationInfo.DataSize,
                    pData = currentStage.SpecializationInfo.Data,
                };

                Marshal.StructureToPtr(spInfo, pSpecializationInfo, false);
            }

            var pStage = new VkPipelineShaderStageCreateInfo
            {
                sType = VkStructureType.StructureTypePipelineShaderStageCreateInfo,
                pNext = IntPtr.Zero,
                flags = currentStage.Flags,
                stage = currentStage.Stage,
                module = bModule.Handle,
                pName = pName,
                pSpecializationInfo = pSpecializationInfo,
            };
            return pStage;
        }
    }
}
