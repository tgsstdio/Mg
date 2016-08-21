using System;

namespace Magnesium.Vulkan
{
	public class VkDebugReportCallbackEXT : MgDebugReportCallbackEXT
	{
		internal UInt64 Handle { get; private set; }
		public VkDebugReportCallbackEXT(UInt64 callback)
		{
			this.Handle = callback;
		}
	}
}