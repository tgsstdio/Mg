using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkDebugReportMessageEXTSection
	{
        public void DebugReportMessageEXT(VkInstanceInfo info, MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
        {
            Debug.Assert(!info.IsDisposed);

            throw new NotImplementedException();
        }
    }
}
