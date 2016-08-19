using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkRect2D
	{
		public VkOffset2D offset { get; set; }
		public VkExtent2D extent { get; set; }
	}
}
