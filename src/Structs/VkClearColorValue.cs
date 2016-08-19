using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkClearColorValue
	{
		public float float32 { get; set; }
		public Int32 int32 { get; set; }
		public UInt32 uint32 { get; set; }
	}
}
