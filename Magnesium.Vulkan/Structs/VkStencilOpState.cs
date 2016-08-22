using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkStencilOpState
	{
		public VkStencilOp failOp { get; set; }
		public VkStencilOp passOp { get; set; }
		public VkStencilOp depthFailOp { get; set; }
		public VkCompareOp compareOp { get; set; }
		public UInt32 compareMask { get; set; }
		public UInt32 writeMask { get; set; }
		public UInt32 reference { get; set; }
	}
}
