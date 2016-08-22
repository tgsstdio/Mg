using Magnesium;
using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
	internal struct VkPhysicalDeviceLimits
	{
		public UInt32 maxImageDimension1D { get; set; }
		public UInt32 maxImageDimension2D { get; set; }
		public UInt32 maxImageDimension3D { get; set; }
		public UInt32 maxImageDimensionCube { get; set; }
		public UInt32 maxImageArrayLayers { get; set; }
		public UInt32 maxTexelBufferElements { get; set; }
		public UInt32 maxUniformBufferRange { get; set; }
		public UInt32 maxStorageBufferRange { get; set; }
		public UInt32 maxPushConstantsSize { get; set; }
		public UInt32 maxMemoryAllocationCount { get; set; }
		public UInt32 maxSamplerAllocationCount { get; set; }
		public UInt64 bufferImageGranularity { get; set; }
		public UInt64 sparseAddressSpaceSize { get; set; }
		public UInt32 maxBoundDescriptorSets { get; set; }
		public UInt32 maxPerStageDescriptorSamplers { get; set; }
		public UInt32 maxPerStageDescriptorUniformBuffers { get; set; }
		public UInt32 maxPerStageDescriptorStorageBuffers { get; set; }
		public UInt32 maxPerStageDescriptorSampledImages { get; set; }
		public UInt32 maxPerStageDescriptorStorageImages { get; set; }
		public UInt32 maxPerStageDescriptorInputAttachments { get; set; }
		public UInt32 maxPerStageResources { get; set; }
		public UInt32 maxDescriptorSetSamplers { get; set; }
		public UInt32 maxDescriptorSetUniformBuffers { get; set; }
		public UInt32 maxDescriptorSetUniformBuffersDynamic { get; set; }
		public UInt32 maxDescriptorSetStorageBuffers { get; set; }
		public UInt32 maxDescriptorSetStorageBuffersDynamic { get; set; }
		public UInt32 maxDescriptorSetSampledImages { get; set; }
		public UInt32 maxDescriptorSetStorageImages { get; set; }
		public UInt32 maxDescriptorSetInputAttachments { get; set; }
		public UInt32 maxVertexInputAttributes { get; set; }
		public UInt32 maxVertexInputBindings { get; set; }
		public UInt32 maxVertexInputAttributeOffset { get; set; }
		public UInt32 maxVertexInputBindingStride { get; set; }
		public UInt32 maxVertexOutputComponents { get; set; }
		public UInt32 maxTessellationGenerationLevel { get; set; }
		public UInt32 maxTessellationPatchSize { get; set; }
		public UInt32 maxTessellationControlPerVertexInputComponents { get; set; }
		public UInt32 maxTessellationControlPerVertexOutputComponents { get; set; }
		public UInt32 maxTessellationControlPerPatchOutputComponents { get; set; }
		public UInt32 maxTessellationControlTotalOutputComponents { get; set; }
		public UInt32 maxTessellationEvaluationInputComponents { get; set; }
		public UInt32 maxTessellationEvaluationOutputComponents { get; set; }
		public UInt32 maxGeometryShaderInvocations { get; set; }
		public UInt32 maxGeometryInputComponents { get; set; }
		public UInt32 maxGeometryOutputComponents { get; set; }
		public UInt32 maxGeometryOutputVertices { get; set; }
		public UInt32 maxGeometryTotalOutputComponents { get; set; }
		public UInt32 maxFragmentInputComponents { get; set; }
		public UInt32 maxFragmentOutputAttachments { get; set; }
		public UInt32 maxFragmentDualSrcAttachments { get; set; }
		public UInt32 maxFragmentCombinedOutputResources { get; set; }
		public UInt32 maxComputeSharedMemorySize { get; set; }
		public UInt32 maxComputeWorkGroupCount { get; set; }
		public UInt32 maxComputeWorkGroupInvocations { get; set; }
		public UInt32 maxComputeWorkGroupSize { get; set; }
		public UInt32 subPixelPrecisionBits { get; set; }
		public UInt32 subTexelPrecisionBits { get; set; }
		public UInt32 mipmapPrecisionBits { get; set; }
		public UInt32 maxDrawIndexedIndexValue { get; set; }
		public UInt32 maxDrawIndirectCount { get; set; }
		public float maxSamplerLodBias { get; set; }
		public float maxSamplerAnisotropy { get; set; }
		public UInt32 maxViewports { get; set; }
		public UInt32 maxViewportDimensions { get; set; }
		public float viewportBoundsRange { get; set; }
		public UInt32 viewportSubPixelBits { get; set; }
		public UIntPtr minMemoryMapAlignment { get; set; }
		public UInt64 minTexelBufferOffsetAlignment { get; set; }
		public UInt64 minUniformBufferOffsetAlignment { get; set; }
		public UInt64 minStorageBufferOffsetAlignment { get; set; }
		public Int32 minTexelOffset { get; set; }
		public UInt32 maxTexelOffset { get; set; }
		public Int32 minTexelGatherOffset { get; set; }
		public UInt32 maxTexelGatherOffset { get; set; }
		public float minInterpolationOffset { get; set; }
		public float maxInterpolationOffset { get; set; }
		public UInt32 subPixelInterpolationOffsetBits { get; set; }
		public UInt32 maxFramebufferWidth { get; set; }
		public UInt32 maxFramebufferHeight { get; set; }
		public UInt32 maxFramebufferLayers { get; set; }
		public VkSampleCountFlags framebufferColorSampleCounts { get; set; }
		public VkSampleCountFlags framebufferDepthSampleCounts { get; set; }
		public VkSampleCountFlags framebufferStencilSampleCounts { get; set; }
		public VkSampleCountFlags framebufferNoAttachmentsSampleCounts { get; set; }
		public UInt32 maxColorAttachments { get; set; }
		public VkSampleCountFlags sampledImageColorSampleCounts { get; set; }
		public VkSampleCountFlags sampledImageIntegerSampleCounts { get; set; }
		public VkSampleCountFlags sampledImageDepthSampleCounts { get; set; }
		public VkSampleCountFlags sampledImageStencilSampleCounts { get; set; }
		public VkSampleCountFlags storageImageSampleCounts { get; set; }
		public UInt32 maxSampleMaskWords { get; set; }
		public VkBool32 timestampComputeAndGraphics { get; set; }
		public float timestampPeriod { get; set; }
		public UInt32 maxClipDistances { get; set; }
		public UInt32 maxCullDistances { get; set; }
		public UInt32 maxCombinedClipAndCullDistances { get; set; }
		public UInt32 discreteQueuePriorities { get; set; }
		public float pointSizeRange { get; set; }
		public float lineWidthRange { get; set; }
		public float pointSizeGranularity { get; set; }
		public float lineWidthGranularity { get; set; }
		public VkBool32 strictLines { get; set; }
		public VkBool32 standardSampleLocations { get; set; }
		public UInt64 optimalBufferCopyOffsetAlignment { get; set; }
		public UInt64 optimalBufferCopyRowPitchAlignment { get; set; }
		public UInt64 nonCoherentAtomSize { get; set; }
	}
}
