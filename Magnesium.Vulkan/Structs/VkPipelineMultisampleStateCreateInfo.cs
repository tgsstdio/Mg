using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPipelineMultisampleStateCreateInfo
	{
		public VkStructureType sType { get; set; }
		public IntPtr pNext { get; set; }
		public UInt32 flags { get; set; }
		public VkSampleCountFlags rasterizationSamples { get; set; }
		public VkBool32 sampleShadingEnable { get; set; }
		public float minSampleShading { get; set; }
		public UInt32 pSampleMask { get; set; }
		public VkBool32 alphaToCoverageEnable { get; set; }
		public VkBool32 alphaToOneEnable { get; set; }
	}
}
