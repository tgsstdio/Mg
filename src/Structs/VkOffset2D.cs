using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkOffset2D
	{
		public Int32 x { get; set; }
		public Int32 y { get; set; }
	}
}
