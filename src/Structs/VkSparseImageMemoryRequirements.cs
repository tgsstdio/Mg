using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkSparseImageMemoryRequirements
	{
		public VkSparseImageFormatProperties formatProperties { get; set; }
		public UInt32 imageMipTailFirstLod { get; set; }
		public UInt64 imageMipTailSize { get; set; }
		public UInt64 imageMipTailOffset { get; set; }
		public UInt64 imageMipTailStride { get; set; }
	}
}
