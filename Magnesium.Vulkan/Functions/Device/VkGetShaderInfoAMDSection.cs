using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetShaderInfoAMDSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
		internal extern static MgResult vkGetShaderInfoAMD(IntPtr device, UInt64 pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, ref UIntPtr pInfoSize, IntPtr[] pInfo);

		public static MgResult GetShaderInfoAMD(VkDeviceInfo info, IMgPipeline pipeline, MgShaderStageFlagBits shaderStage, MgShaderInfoTypeAMD infoType, out UIntPtr pInfoSize, out IntPtr pInfo)
		{
            // TODO: add implementation
            throw new NotImplementedException();
		}
	}
}
