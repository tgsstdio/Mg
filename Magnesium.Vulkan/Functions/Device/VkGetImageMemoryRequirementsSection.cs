using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetImageMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetImageMemoryRequirements(IntPtr device, UInt64 image, Magnesium.MgMemoryRequirements* pMemoryRequirements);

        public static void GetImageMemoryRequirements(VkDeviceInfo info, IMgImage image, out MgMemoryRequirements memoryRequirements)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bImage = (VkImage)image;
            Debug.Assert(bImage != null);

            unsafe
            {
                var memReqs = stackalloc MgMemoryRequirements[1];
                vkGetImageMemoryRequirements(info.Handle, bImage.Handle, memReqs);
                memoryRequirements = memReqs[0];
            }
        }
	}
}
