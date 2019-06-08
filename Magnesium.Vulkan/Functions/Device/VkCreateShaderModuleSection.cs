using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateShaderModuleSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkCreateShaderModule(IntPtr device, [In, Out] VkShaderModuleCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pShaderModule);

		public static MgResult CreateShaderModule(VkDeviceInfo info, MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
		{
			// TODO: add implementation
		}
	}
}
