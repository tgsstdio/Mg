using System;
namespace Magnesium.Vulkan
{
	public class VkDisplayModeKHR : IMgDisplayModeKHR
	{
		internal UInt64 Handle { get; private set; }
		public VkDisplayModeKHR(UInt64 handle)
		{
			Handle = handle;
		}
	}
}

