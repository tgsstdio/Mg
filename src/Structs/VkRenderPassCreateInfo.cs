using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkRenderPassCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public UInt32 attachmentCount { get; set; }
		public VkAttachmentDescription pAttachments { get; set; }
		public UInt32 subpassCount { get; set; }
		public VkSubpassDescription pSubpasses { get; set; }
		public UInt32 dependencyCount { get; set; }
		public VkSubpassDependency pDependencies { get; set; }
	}
}
