using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VkPhysicalDeviceFeatures
	{
		public VkBool32 robustBufferAccess { get; set; }
		public VkBool32 fullDrawIndexUint32 { get; set; }
		public VkBool32 imageCubeArray { get; set; }
		public VkBool32 independentBlend { get; set; }
		public VkBool32 geometryShader { get; set; }
		public VkBool32 tessellationShader { get; set; }
		public VkBool32 sampleRateShading { get; set; }
		public VkBool32 dualSrcBlend { get; set; }
		public VkBool32 logicOp { get; set; }
		public VkBool32 multiDrawIndirect { get; set; }
		public VkBool32 drawIndirectFirstInstance { get; set; }
		public VkBool32 depthClamp { get; set; }
		public VkBool32 depthBiasClamp { get; set; }
		public VkBool32 fillModeNonSolid { get; set; }
		public VkBool32 depthBounds { get; set; }
		public VkBool32 wideLines { get; set; }
		public VkBool32 largePoints { get; set; }
		public VkBool32 alphaToOne { get; set; }
		public VkBool32 multiViewport { get; set; }
		public VkBool32 samplerAnisotropy { get; set; }
		public VkBool32 textureCompressionETC2 { get; set; }
		public VkBool32 textureCompressionASTC_LDR { get; set; }
		public VkBool32 textureCompressionBC { get; set; }
		public VkBool32 occlusionQueryPrecise { get; set; }
		public VkBool32 pipelineStatisticsQuery { get; set; }
		public VkBool32 vertexPipelineStoresAndAtomics { get; set; }
		public VkBool32 fragmentStoresAndAtomics { get; set; }
		public VkBool32 shaderTessellationAndGeometryPointSize { get; set; }
		public VkBool32 shaderImageGatherExtended { get; set; }
		public VkBool32 shaderStorageImageExtendedFormats { get; set; }
		public VkBool32 shaderStorageImageMultisample { get; set; }
		public VkBool32 shaderStorageImageReadWithoutFormat { get; set; }
		public VkBool32 shaderStorageImageWriteWithoutFormat { get; set; }
		public VkBool32 shaderUniformBufferArrayDynamicIndexing { get; set; }
		public VkBool32 shaderSampledImageArrayDynamicIndexing { get; set; }
		public VkBool32 shaderStorageBufferArrayDynamicIndexing { get; set; }
		public VkBool32 shaderStorageImageArrayDynamicIndexing { get; set; }
		public VkBool32 shaderClipDistance { get; set; }
		public VkBool32 shaderCullDistance { get; set; }
		public VkBool32 shaderFloat64 { get; set; }
		public VkBool32 shaderInt64 { get; set; }
		public VkBool32 shaderInt16 { get; set; }
		public VkBool32 shaderResourceResidency { get; set; }
		public VkBool32 shaderResourceMinLod { get; set; }
		public VkBool32 sparseBinding { get; set; }
		public VkBool32 sparseResidencyBuffer { get; set; }
		public VkBool32 sparseResidencyImage2D { get; set; }
		public VkBool32 sparseResidencyImage3D { get; set; }
		public VkBool32 sparseResidency2Samples { get; set; }
		public VkBool32 sparseResidency4Samples { get; set; }
		public VkBool32 sparseResidency8Samples { get; set; }
		public VkBool32 sparseResidency16Samples { get; set; }
		public VkBool32 sparseResidencyAliased { get; set; }
		public VkBool32 variableMultisampleRate { get; set; }
		public VkBool32 inheritedQueries { get; set; }
	}
}
