using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetDeviceQueueSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetDeviceQueue(IntPtr device, UInt32 queueFamilyIndex, UInt32 queueIndex, ref IntPtr pQueue);

        public static void GetDeviceQueue(VkDeviceInfo info, UInt32 queueFamilyIndex, UInt32 queueIndex, out IMgQueue pQueue)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var queueHandle = IntPtr.Zero;
            vkGetDeviceQueue(info.Handle, queueFamilyIndex, queueIndex, ref queueHandle);
            pQueue = new VkQueue(queueHandle);
        }
	}
}
