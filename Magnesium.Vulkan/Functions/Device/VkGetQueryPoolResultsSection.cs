using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetQueryPoolResultsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetQueryPoolResults(IntPtr device, UInt64 queryPool, UInt32 firstQuery, UInt32 queryCount, UIntPtr dataSize, IntPtr[] pData, VkDeviceSize stride, VkQueryResultFlags flags);

		public static MgResult GetQueryPoolResults(VkDeviceInfo info, IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags)
		{
			// TODO: add implementation
		}
	}
}
