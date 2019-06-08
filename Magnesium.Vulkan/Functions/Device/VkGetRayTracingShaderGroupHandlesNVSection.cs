using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetRayTracingShaderGroupHandlesNVSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetRayTracingShaderGroupHandlesNV(IntPtr device, UInt64 pipeline, UInt32 firstGroup, UInt32 groupCount, UIntPtr dataSize, IntPtr[] pData);

		public static MgResult GetRayTracingShaderGroupHandlesNV(VkDeviceInfo info, IMgPipeline pipeline, UInt32 firstGroup, UInt32 groupCount, UIntPtr dataSize, IntPtr[] pData)
		{
			// TODO: add implementation
		}
	}
}
