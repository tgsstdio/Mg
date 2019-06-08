using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetDeviceGroupPeerMemoryFeaturesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static void vkGetDeviceGroupPeerMemoryFeatures(IntPtr device, UInt32 heapIndex, UInt32 localDeviceIndex, UInt32 remoteDeviceIndex, ref VkPeerMemoryFeatureFlags pPeerMemoryFeatures);

		public static void GetDeviceGroupPeerMemoryFeatures(VkDeviceInfo info, UInt32 heapIndex, UInt32 localDeviceIndex, UInt32 remoteDeviceIndex, out MgPeerMemoryFeatureFlagBits pPeerMemoryFeatures)
		{
			// TODO: add implementation
		}
	}
}
