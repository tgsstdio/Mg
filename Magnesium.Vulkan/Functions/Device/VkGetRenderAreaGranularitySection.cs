using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetRenderAreaGranularitySection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetRenderAreaGranularity(IntPtr device, UInt64 renderPass, MgExtent2D* pGranularity);

        public static void GetRenderAreaGranularity(VkDeviceInfo info, IMgRenderPass renderPass, out MgExtent2D pGranularity)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bRenderPass = (VkRenderPass)renderPass;
            Debug.Assert(bRenderPass != null);

            unsafe
            {
                var grans = stackalloc MgExtent2D[1];
                vkGetRenderAreaGranularity(info.Handle, bRenderPass.Handle, grans);
                pGranularity = grans[0];
            }
        }
    }
}
