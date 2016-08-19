using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkRect3D
	{
		public VkOffset3D offset { get; set; }
		public VkExtent3D extent { get; set; }
	}
}
