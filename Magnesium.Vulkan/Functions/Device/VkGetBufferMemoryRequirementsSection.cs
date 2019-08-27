using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetBufferMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetBufferMemoryRequirements(IntPtr device, UInt64 buffer, Magnesium.MgMemoryRequirements* pMemoryRequirements);

        public static void GetBufferMemoryRequirements(VkDeviceInfo info, IMgBuffer buffer, out MgMemoryRequirements pMemoryRequirements)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bBuffer = (VkBuffer)buffer;
            Debug.Assert(bBuffer != null);

            unsafe
            {
                var memReqs = stackalloc MgMemoryRequirements[1];
                vkGetBufferMemoryRequirements(info.Handle, bBuffer.Handle, memReqs);
                pMemoryRequirements = memReqs[0];
            }
        }
	}
}
