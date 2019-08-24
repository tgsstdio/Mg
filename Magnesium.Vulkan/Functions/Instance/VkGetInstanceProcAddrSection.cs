using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkGetInstanceProcAddrSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static IntPtr vkGetInstanceProcAddr(IntPtr instance, IntPtr pName);

        public static IntPtr GetInstanceProcAddr(VkInstanceInfo info, string pName)
        {
            Debug.Assert(!info.IsDisposed);

            var fnNamePtr = IntPtr.Zero;
            try
            {
                fnNamePtr = VkInteropsUtility.NativeUtf8FromString(pName);
                return vkGetInstanceProcAddr(info.Handle, fnNamePtr);
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
