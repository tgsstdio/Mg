using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateIndirectCommandsLayoutNVXSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkCreateIndirectCommandsLayoutNVX(IntPtr device, [In, Out] VkIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IntPtr pAllocator, ref UInt64 pIndirectCommandsLayout);

		public static MgResult CreateIndirectCommandsLayoutNVX(VkDeviceInfo info, MgIndirectCommandsLayoutCreateInfoNVX pCreateInfo, IMgAllocationCallbacks pAllocator, out IMgIndirectCommandsLayoutNVX pIndirectCommandsLayout)
		{
            // TODO: add implementation
            throw new NotImplementedException();
        }
	}
}
