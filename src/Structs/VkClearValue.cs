using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkClearValue
	{
		public VkClearColorValue color { get; set; }
		public VkClearDepthStencilValue depthStencil { get; set; }
	}
}
