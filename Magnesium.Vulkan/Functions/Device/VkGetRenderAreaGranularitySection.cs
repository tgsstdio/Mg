using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetRenderAreaGranularitySection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe void vkGetRenderAreaGranularity(IntPtr device, UInt64 renderPass, MgExtent2D* pGranularity);

		public static void GetRenderAreaGranularity(VkDeviceInfo info, IMgRenderPass renderPass, out MgExtent2D pGranularity)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
