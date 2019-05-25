using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkRenderPassCreateInfo2KHR
	{
		public VkStructureType sType;
		public IntPtr pNext;
		public UInt32 flags;
		public UInt32 attachmentCount;
		public IntPtr pAttachments;
		public UInt32 subpassCount;
		public IntPtr pSubpasses;
		public UInt32 dependencyCount;
		public IntPtr pDependencies;
		public UInt32 correlatedViewMaskCount;
		public IntPtr pCorrelatedViewMasks;
	}
}
