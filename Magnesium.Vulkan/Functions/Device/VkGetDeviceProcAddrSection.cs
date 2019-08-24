using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceProcAddrSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static PFN_vkVoidFunction vkGetDeviceProcAddr(IntPtr device, [MarshalAs(UnmanagedType.LPStr)] string pName);

        public static PFN_vkVoidFunction GetDeviceProcAddr(VkDeviceInfo info, string pName)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            return vkGetDeviceProcAddr(info.Handle, pName);
        }
	}
}
