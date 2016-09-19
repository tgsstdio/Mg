using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPipelineColorBlendStateCreateInfo
	{
		public VkStructureType sType {get; set;}
		public IntPtr pNext {get; set;}
		public UInt32 flags {get; set;}
		public VkBool32 logicOpEnable {get; set;}
		public VkLogicOp logicOp {get; set;}
		public UInt32 attachmentCount {get; set;}
		public IntPtr pAttachments {get; set;} // VkPipelineColorBlendAttachmentState
		public MgColor4f blendConstants {get; set;}
	}
}
