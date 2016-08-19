using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkViewport
	{
		public float x { get; set; }
		public float y { get; set; }
		public float width { get; set; }
		public float height { get; set; }
		public float minDepth { get; set; }
		public float maxDepth { get; set; }
	}
}
