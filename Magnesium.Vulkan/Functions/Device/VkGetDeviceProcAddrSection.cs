using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceProcAddrSection
	{
        [DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static IntPtr vkGetDeviceProcAddr(IntPtr device, IntPtr pName);

        public static IntPtr GetDeviceProcAddr(VkDeviceInfo info, string pName)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            IntPtr fnNamePtr = IntPtr.Zero;
            try
            {
                fnNamePtr = VkInteropsUtility.NativeUtf8FromString(pName);
                return vkGetDeviceProcAddr(info.Handle, fnNamePtr);
            }
            finally
            {
                if (fnNamePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(fnNamePtr);
                }
            }
        }
	}
}
