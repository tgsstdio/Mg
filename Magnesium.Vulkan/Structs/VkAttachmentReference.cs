using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkAttachmentReference
	{
		public UInt32 attachment { get; set; }
		public VkImageLayout layout { get; set; }
	}
}
