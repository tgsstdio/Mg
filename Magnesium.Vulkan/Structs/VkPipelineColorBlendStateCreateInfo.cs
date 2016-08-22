using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineColorBlendStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkBool32 logicOpEnable { get; set; }
		public VkLogicOp logicOp { get; set; }
		public UInt32 attachmentCount { get; set; }
		public VkPipelineColorBlendAttachmentState pAttachments { get; set; }
		public float blendConstants { get; set; }
	}
}
