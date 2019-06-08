using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetAccelerationStructureMemoryRequirementsNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetAccelerationStructureMemoryRequirementsNV(IntPtr device, VkAccelerationStructureMemoryRequirementsInfoNV pInfo, VkMemoryRequirements2KHR* pMemoryRequirements);

		public static void GetAccelerationStructureMemoryRequirementsNV(VkDeviceInfo info, MgAccelerationStructureMemoryRequirementsInfoNV pInfo, out MgMemoryRequirements2 pMemoryRequirements)
		{
			// TODO: add implementation
		}
	}
}
