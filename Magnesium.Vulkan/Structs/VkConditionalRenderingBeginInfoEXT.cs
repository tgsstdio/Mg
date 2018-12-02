using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkConditionalRenderingBeginInfoEXT
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt64 buffer { get; set; }
		public UInt64 offset { get; set; }
		public UInt32 flags { get; set; }
	}
}
