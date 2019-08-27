using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetDeviceGroupSurfacePresentModesKHRSection
	{
		//[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		//internal extern static MgResult vkGetDeviceGroupSurfacePresentModesKHR(IntPtr device, UInt64 surface, ref VkDeviceGroupPresentModeFlagsKHR pModes);

		public static MgResult GetDeviceGroupSurfacePresentModesKHR(VkDeviceInfo info, IMgSurfaceKHR surface, out MgDeviceGroupPresentModeFlagBitsKHR pModes)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
