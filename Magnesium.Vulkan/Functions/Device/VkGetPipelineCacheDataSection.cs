using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetPipelineCacheDataSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetPipelineCacheData(IntPtr device, UInt64 pipelineCache, ref UIntPtr pDataSize, IntPtr[] pData);

		public static MgResult GetPipelineCacheData(VkDeviceInfo info, IMgPipelineCache pipelineCache, out Byte[] pData)
		{
			// TODO: add implementation
		}
	}
}
