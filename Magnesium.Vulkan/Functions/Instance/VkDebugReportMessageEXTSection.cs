using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkDebugReportMessageEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static void vkDebugReportMessageEXT(IntPtr instance, VkDebugReportFlagsExt flags, VkDebugReportObjectTypeExt objectType, UInt64 @object, UIntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage);

        public static void DebugReportMessageEXT(VkInstanceInfo info, MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
        {
            Debug.Assert(!info.IsDisposed);

            throw new NotImplementedException();
        }
    }
}
