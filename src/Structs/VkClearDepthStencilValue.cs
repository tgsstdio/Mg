using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkClearDepthStencilValue
	{
		public float depth { get; set; }
		public UInt32 stencil { get; set; }
	}
}
