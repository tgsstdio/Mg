using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkResetFencesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkResetFences(IntPtr device, UInt32 fenceCount, UInt64[] pFences);

		public static MgResult ResetFences(VkDeviceInfo info, IMgFence[] pFences)
		{
			// TODO: add implementation
		}
	}
}
