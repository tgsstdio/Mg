using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkRayTracingPipelineCreateInfoNV
	{
		public VkStructureType sType;
		public IntPtr pNext;
        // Pipeline creation flags
        public MgPipelineCreateFlagBits flags;
		public UInt32 stageCount;
		// One entry for each active shader stage
		public IntPtr pStages;
		public UInt32 groupCount;
		public IntPtr pGroups;
		public UInt32 maxRecursionDepth;
		// Interface layout of the pipeline
		public UInt64 layout;
		// If VK_PIPELINE_CREATE_DERIVATIVE_BIT is set and this value is nonzero, it specifies the handle of the base pipeline this is a derivative of
		public UInt64 basePipelineHandle;
		// If VK_PIPELINE_CREATE_DERIVATIVE_BIT is set and this value is not -1, it specifies an index into pCreateInfos of the base pipeline this is a derivative of
		public Int32 basePipelineIndex;
	}
}
