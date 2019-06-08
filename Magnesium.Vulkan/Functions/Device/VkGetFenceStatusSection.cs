using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetFenceStatusSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetFenceStatus(IntPtr device, UInt64 fence);

		public static MgResult GetFenceStatus(VkDeviceInfo info, IMgFence fence)
		{
			// TODO: add implementation
		}
	}
}
