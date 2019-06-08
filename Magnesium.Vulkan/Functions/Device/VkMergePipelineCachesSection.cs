using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkMergePipelineCachesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkMergePipelineCaches(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, UInt64[] pSrcCaches);

		public static MgResult MergePipelineCaches(VkDeviceInfo info, IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
		{
			// TODO: add implementation
		}
	}
}
