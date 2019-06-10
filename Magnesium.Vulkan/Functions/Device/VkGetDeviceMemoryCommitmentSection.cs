using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceMemoryCommitmentSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetDeviceMemoryCommitment(IntPtr device, UInt64 memory, ref UInt64 pCommittedMemoryInBytes);

        public static void GetDeviceMemoryCommitment(VkDeviceInfo info, IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes)
		{
            var bDeviceMemory = (VkDeviceMemory)memory;
            Debug.Assert(bDeviceMemory != null);

            vkGetDeviceMemoryCommitment(info.Handle, bDeviceMemory.Handle, ref pCommittedMemoryInBytes);
        }
	}
}
