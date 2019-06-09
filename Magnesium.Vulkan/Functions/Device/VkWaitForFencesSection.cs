using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkWaitForFencesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkWaitForFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences, VkBool32 waitAll, UInt64 timeout);

        public static MgResult WaitForFences(VkDeviceInfo info, IMgFence[] pFences, Boolean waitAll, UInt64 timeout)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var fenceCount = (uint)pFences.Length;

            var fenceHandles = new ulong[pFences.Length];
            for (var i = 0; i < fenceCount; ++i)
            {
                var bFence = (VkFence)pFences[i];
                Debug.Assert(bFence != null);
                fenceHandles[i] = bFence.Handle;
            }

            return vkWaitForFences(info.Handle, fenceCount, fenceHandles, VkBool32.ConvertTo(waitAll), timeout);
        }
    }
}
