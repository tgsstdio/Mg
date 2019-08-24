using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkGetInstanceProcAddrSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static PFN_vkVoidFunction vkGetInstanceProcAddr(IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string pName);

        public static PFN_vkVoidFunction GetInstanceProcAddr(VkInstanceInfo info, string pName)
        {
            Debug.Assert(!info.IsDisposed);

            return vkGetInstanceProcAddr(info.Handle, pName);
        }
    }
}
