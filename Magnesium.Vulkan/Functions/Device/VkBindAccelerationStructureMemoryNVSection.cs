using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkBindAccelerationStructureMemoryNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkBindAccelerationStructureMemoryNV(IntPtr device, UInt32 bindInfoCount, VkBindAccelerationStructureMemoryInfoNV* pBindInfos);

		public static MgResult BindAccelerationStructureMemoryNV(VkDeviceInfo info, MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
