using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubpassDependency
	{
		public UInt32 srcSubpass { get; set; }
		public UInt32 dstSubpass { get; set; }
		public MgPipelineStageFlagBits srcStageMask { get; set; }
		public MgPipelineStageFlagBits dstStageMask { get; set; }
		public MgAccessFlagBits srcAccessMask { get; set; }
		public MgAccessFlagBits dstAccessMask { get; set; }
		public MgDependencyFlagBits dependencyFlags { get; set; }
	}
}
