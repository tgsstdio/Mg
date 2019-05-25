using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkAttachmentReference
	{
		public UInt32 attachment { get; set; }
		public MgImageLayout layout { get; set; }
	}
}
