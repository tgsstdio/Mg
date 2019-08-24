using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkResetFencesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkResetFences(IntPtr device, UInt32 fenceCount, [In] UInt64[] pFences);

        public static MgResult ResetFences(VkDeviceInfo info, IMgFence[] pFences)
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

            return vkResetFences(info.Handle, fenceCount, fenceHandles);
        }
	}
}
