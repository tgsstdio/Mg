using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateAccelerationStructureNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateAccelerationStructureNV(IntPtr device, [In, Out] VkAccelerationStructureCreateInfoNV pCreateInfo, IntPtr pAllocator, ref UInt64 pAccelerationStructure);

		public static MgResult CreateAccelerationStructureNV(VkDeviceInfo info, MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgAccelerationStructureNV pAccelerationStructure)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
