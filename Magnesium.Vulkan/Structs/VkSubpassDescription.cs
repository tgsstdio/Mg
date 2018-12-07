using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkSubpassDescription
	{
		public UInt32 flags { get; set; }
		public MgPipelineBindPoint pipelineBindPoint { get; set; }
		public UInt32 inputAttachmentCount { get; set; }
		public IntPtr pInputAttachments { get; set; } // VkAttachmentReference
		public UInt32 colorAttachmentCount { get; set; }
		public IntPtr pColorAttachments { get; set; } // VkAttachmentReference[colorAttachmentCount]
		public IntPtr pResolveAttachments { get; set; } // VkAttachmentReference[colorAttachmentCount]
		public IntPtr pDepthStencilAttachment { get; set; } // VkAttachmentReference
		public UInt32 preserveAttachmentCount { get; set; }
		public IntPtr pPreserveAttachments { get; set; } // UInt32
	}
}
