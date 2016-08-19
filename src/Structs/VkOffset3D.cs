using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkOffset3D
	{
		public Int32 x { get; set; }
		public Int32 y { get; set; }
		public Int32 z { get; set; }
	}
}
