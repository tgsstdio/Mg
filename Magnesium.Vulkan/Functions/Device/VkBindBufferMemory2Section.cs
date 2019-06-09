using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkBindBufferMemory2Section
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static unsafe MgResult vkBindBufferMemory2(IntPtr device, UInt32 bindInfoCount, VkBindBufferMemoryInfo* pBindInfos);

		public static MgResult BindBufferMemory2(VkDeviceInfo info, MgBindBufferMemoryInfo[] pBindInfos)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
