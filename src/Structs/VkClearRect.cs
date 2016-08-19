using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkClearRect
	{
		public VkRect2D rect { get; set; }
		public UInt32 baseArrayLayer { get; set; }
		public UInt32 layerCount { get; set; }
	}
}
