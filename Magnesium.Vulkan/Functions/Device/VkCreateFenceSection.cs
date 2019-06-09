using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateFenceSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateFence(IntPtr device, ref VkFenceCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pFence);

        public static MgResult CreateFence(VkDeviceInfo info, MgFenceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgFence fence)
        {
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var createInfo = new VkFenceCreateInfo
            {
                sType = VkStructureType.StructureTypeFenceCreateInfo,
                pNext = IntPtr.Zero,
                flags = (VkFenceCreateFlags)pCreateInfo.Flags,
            };

            ulong pFence = 0UL;
            var result = vkCreateFence(info.Handle, ref createInfo, allocatorPtr, ref pFence);
            fence = new VkFence(pFence);
            return result;
        }
    }
}
