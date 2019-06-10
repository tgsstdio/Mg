using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetAccelerationStructureHandleNVSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetAccelerationStructureHandleNV(IntPtr device, UInt64 accelerationStructure, UIntPtr dataSize, IntPtr[] pData);

		public static MgResult GetAccelerationStructureHandleNV(VkDeviceInfo info, IMgAccelerationStructureNV accelerationStructure, UIntPtr dataSize, out IntPtr pData)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
