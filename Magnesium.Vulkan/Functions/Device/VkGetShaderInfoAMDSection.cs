using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetShaderInfoAMDSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
		internal extern static VkResult vkGetShaderInfoAMD(IntPtr device, UInt64 pipeline, VkShaderStageFlagBits shaderStage, VkShaderInfoTypeAMD infoType, ref UIntPtr pInfoSize, IntPtr[] pInfo);

		public static MgResult GetShaderInfoAMD(VkDeviceInfo info, IMgPipeline pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, out UIntPtr pInfoSize, out IntPtr pInfo)
		{
			// TODO: add implementation
		}
	}
}
