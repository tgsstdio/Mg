using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkBindAccelerationStructureMemoryNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkBindAccelerationStructureMemoryNV(IntPtr device, UInt32 bindInfoCount, [In, Out] VkBindAccelerationStructureMemoryInfoNV[] pBindInfos);

        public static MgResult BindAccelerationStructureMemoryNV(VkDeviceInfo info, MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
        {
            var attachedItems = new List<IntPtr>();

            try
            {
                var bindInfoCount = (UInt32)pBindInfos.Length;

                var bBindInfos = new VkBindAccelerationStructureMemoryInfoNV[bindInfoCount];

                for (var i = 0; i < bindInfoCount; i += 1)
                {
                    var currentInfo = pBindInfos[i];

                    var bAccelerationStructure = (VkAccelerationStructureNV)currentInfo.AccelerationStructure;
                    var bAccelerationStructurePtr = bAccelerationStructure != null ? bAccelerationStructure.Handle : 0UL;

                    var bDeviceMemory = (VkDeviceMemory)currentInfo.Memory;
                    var bDeviceMemoryPtr = bDeviceMemory != null ? bDeviceMemory.Handle : 0UL;

                    var pDeviceIndices = VkInteropsUtility.AllocateUInt32Array(currentInfo.DeviceIndices);
                    if (pDeviceIndices != IntPtr.Zero)
                        attachedItems.Add(pDeviceIndices);

                    var deviceIndexCount = currentInfo.DeviceIndices != null ? (uint)currentInfo.DeviceIndices.Length : 0U;

                    bBindInfos[i] = new VkBindAccelerationStructureMemoryInfoNV
                    {
                        sType = VkStructureType.StructureTypeBindAccelerationStructureMemoryInfoNv,
                        // TODO: extensible
                        pNext = IntPtr.Zero,
                        accelerationStructure = bAccelerationStructurePtr,
                        memory = bDeviceMemoryPtr,
                        memoryOffset = currentInfo.MemoryOffset,
                        deviceIndexCount = deviceIndexCount,
                        pDeviceIndices = pDeviceIndices,
                    };
                }

                return vkBindAccelerationStructureMemoryNV(info.Handle, bindInfoCount, bBindInfos);
            }
            finally
            {
                foreach (var handle in attachedItems)
                {
                    Marshal.FreeHGlobal(handle);
                }
            }
        }
    }
}
