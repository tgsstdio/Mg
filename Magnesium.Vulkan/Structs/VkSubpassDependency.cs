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
		public VkPipelineStageFlags srcStageMask { get; set; }
		public VkPipelineStageFlags dstStageMask { get; set; }
		public VkAccessFlags srcAccessMask { get; set; }
		public VkAccessFlags dstAccessMask { get; set; }
		public VkDependencyFlags dependencyFlags { get; set; }
	}
}
