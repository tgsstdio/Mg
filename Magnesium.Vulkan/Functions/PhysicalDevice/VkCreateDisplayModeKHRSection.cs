using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkCreateDisplayModeKHRSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDisplayModeKHR(IntPtr physicalDevice, UInt64 display, ref VkDisplayModeCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pMode);

        public static MgResult CreateDisplayModeKHR(VkPhysicalDeviceInfo info, IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
        {
            var bDisplay = (VkDisplayModeKHR)display;
            Debug.Assert(bDisplay != null);

            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

            var createInfo = new VkDisplayModeCreateInfoKHR
            {
                sType = VkStructureType.StructureTypeDisplayModeCreateInfoKhr,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.flags,
                parameters = pCreateInfo.parameters,
            };

            var modeHandle = 0UL;
            var result = vkCreateDisplayModeKHR(info.Handle, bDisplay.Handle, ref createInfo, allocatorPtr, ref modeHandle);
            pMode = new VkDisplayModeKHR(modeHandle);

            return result;
        }
    }
}
