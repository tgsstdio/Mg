using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceMemoryCommitmentSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceMemoryCommitment(IntPtr device, UInt64 memory, ref VkDeviceSize pCommittedMemoryInBytes);

		public static void GetDeviceMemoryCommitment(VkDeviceInfo info, IMgDeviceMemory memory, ref UInt64 pCommittedMemoryInBytes)
		{
			// TODO: add implementation
		}
	}
}
