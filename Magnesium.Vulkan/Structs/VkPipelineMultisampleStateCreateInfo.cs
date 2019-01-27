using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPipelineMultisampleStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public MgSampleCountFlagBits rasterizationSamples { get; set; }
		public VkBool32 sampleShadingEnable { get; set; }
		public float minSampleShading { get; set; }
		public IntPtr pSampleMask { get; set; } // UInt32
		public VkBool32 alphaToCoverageEnable { get; set; }
		public VkBool32 alphaToOneEnable { get; set; }
	}
}
