using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkWaitForFencesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkWaitForFences(IntPtr device, UInt32 fenceCount, UInt64[] pFences, VkBool32 waitAll, UInt64 timeout);

		public static MgResult WaitForFences(VkDeviceInfo info, IMgFence[] pFences, Boolean waitAll, UInt64 timeout)
		{
			// TODO: add implementation
		}
	}
}
