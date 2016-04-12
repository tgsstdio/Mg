using System;

namespace Magnesium
{
    public class MgPhysicalDeviceLimits
	{
		public UInt32 MaxImageDimension1D { get; set; }
		public UInt32 MaxImageDimension2D { get; set; }
		public UInt32 MaxImageDimension3D { get; set; }
		public UInt32 MaxImageDimensionCube { get; set; }
		public UInt32 MaxImageArrayLayers { get; set; }
		public UInt32 MaxTexelBufferElements { get; set; }
		public UInt32 MaxUniformBufferRange { get; set; }
		public UInt32 MaxStorageBufferRange { get; set; }
		public UInt32 MaxPushConstantsSize { get; set; }
		public UInt32 MaxMemoryAllocationCount { get; set; }
		public UInt32 MaxSamplerAllocationCount { get; set; }
		public UInt64 BufferImageGranularity { get; set; }
		public UInt64 SparseAddressSpaceSize { get; set; }
		public UInt32 MaxBoundDescriptorSets { get; set; }
		public UInt32 MaxPerStageDescriptorSamplers { get; set; }
		public UInt32 MaxPerStageDescriptorUniformBuffers { get; set; }
		public UInt32 MaxPerStageDescriptorStorageBuffers { get; set; }
		public UInt32 MaxPerStageDescriptorSampledImages { get; set; }
		public UInt32 MaxPerStageDescriptorStorageImages { get; set; }
		public UInt32 MaxPerStageDescriptorInputAttachments { get; set; }
		public UInt32 MaxPerStageResources { get; set; }
		public UInt32 MaxDescriptorSetSamplers { get; set; }
		public UInt32 MaxDescriptorSetUniformBuffers { get; set; }
		public UInt32 MaxDescriptorSetUniformBuffersDynamic { get; set; }
		public UInt32 MaxDescriptorSetStorageBuffers { get; set; }
		public UInt32 MaxDescriptorSetStorageBuffersDynamic { get; set; }
		public UInt32 MaxDescriptorSetSampledImages { get; set; }
		public UInt32 MaxDescriptorSetStorageImages { get; set; }
		public UInt32 MaxDescriptorSetInputAttachments { get; set; }
		public UInt32 MaxVertexInputAttributes { get; set; }
		public UInt32 MaxVertexInputBindings { get; set; }
		public UInt32 MaxVertexInputAttributeOffset { get; set; }
		public UInt32 MaxVertexInputBindingStride { get; set; }
		public UInt32 MaxVertexOutputComponents { get; set; }
		public UInt32 MaxTessellationGenerationLevel { get; set; }
		public UInt32 MaxTessellationPatchSize { get; set; }
		public UInt32 MaxTessellationControlPerVertexInputComponents { get; set; }
		public UInt32 MaxTessellationControlPerVertexOutputComponents { get; set; }
		public UInt32 MaxTessellationControlPerPatchOutputComponents { get; set; }
		public UInt32 MaxTessellationControlTotalOutputComponents { get; set; }
		public UInt32 MaxTessellationEvaluationInputComponents { get; set; }
		public UInt32 MaxTessellationEvaluationOutputComponents { get; set; }
		public UInt32 MaxGeometryShaderInvocations { get; set; }
		public UInt32 MaxGeometryInputComponents { get; set; }
		public UInt32 MaxGeometryOutputComponents { get; set; }
		public UInt32 MaxGeometryOutputVertices { get; set; }
		public UInt32 MaxGeometryTotalOutputComponents { get; set; }
		public UInt32 MaxFragmentInputComponents { get; set; }
		public UInt32 MaxFragmentOutputAttachments { get; set; }
		public UInt32 MaxFragmentDualSrcAttachments { get; set; }
		public UInt32 MaxFragmentCombinedOutputResources { get; set; }
		public UInt32 MaxComputeSharedMemorySize { get; set; }
		public MgVec3Ui MaxComputeWorkGroupCount { get; set; } //  3
		public UInt32 MaxComputeWorkGroupInvocations { get; set; }
		public MgVec3Ui MaxComputeWorkGroupSize  { get; set; } // 3
		public UInt32 SubPixelPrecisionBits { get; set; }
		public UInt32 SubTexelPrecisionBits { get; set; }
		public UInt32 MipmapPrecisionBits { get; set; }
		public UInt32 MaxDrawIndexedIndexValue { get; set; }
		public UInt32 MaxDrawIndirectCount { get; set; }
		public float MaxSamplerLodBias { get; set; }
		public float MaxSamplerAnisotropy { get; set; }
		public UInt32 MaxViewports { get; set; }
		public MgVec2Ui MaxViewportDimensions { get; set; } // 2
		public MgVec2Ui ViewportBoundsRange { get; set; } // 2
		public UInt32 ViewportSubPixelBits { get; set; }
		public UIntPtr MinMemoryMapAlignment { get; set; }
		public UInt64 MinTexelBufferOffsetAlignment { get; set; }
		public UInt64 MinUniformBufferOffsetAlignment { get; set; }
		public UInt64 MinStorageBufferOffsetAlignment { get; set; }
		public Int32 MinTexelOffset { get; set; }
		public UInt32 MaxTexelOffset { get; set; }
		public Int32 MinTexelGatherOffset { get; set; }
		public UInt32 MaxTexelGatherOffset { get; set; }
		public float MinInterpolationOffset { get; set; }
		public float MaxInterpolationOffset { get; set; }
		public UInt32 SubPixelInterpolationOffsetBits { get; set; }
		public UInt32 MaxFramebufferWidth { get; set; }
		public UInt32 MaxFramebufferHeight { get; set; }
		public UInt32 MaxFramebufferLayers { get; set; }
		public MgSampleCountFlagBits FramebufferColorSampleCounts { get; set; }
		public MgSampleCountFlagBits FramebufferDepthSampleCounts { get; set; }
		public MgSampleCountFlagBits FramebufferStencilSampleCounts { get; set; }
		public MgSampleCountFlagBits FramebufferNoAttachmentsSampleCounts { get; set; }
		public UInt32 MaxColorAttachments { get; set; }
		public MgSampleCountFlagBits SampledImageColorSampleCounts { get; set; }
		public MgSampleCountFlagBits SampledImageIntegerSampleCounts { get; set; }
		public MgSampleCountFlagBits SampledImageDepthSampleCounts { get; set; }
		public MgSampleCountFlagBits SampledImageStencilSampleCounts { get; set; }
		public MgSampleCountFlagBits StorageImageSampleCounts { get; set; }
		public UInt32 MaxSampleMaskWords { get; set; }
		public bool TimestampComputeAndGraphics { get; set; }
		public float TimestampPeriod { get; set; }
		public UInt32 MaxClipDistances { get; set; }
		public UInt32 MaxCullDistances { get; set; }
		public UInt32 MaxCombinedClipAndCullDistances { get; set; }
		public UInt32 DiscreteQueuePriorities { get; set; }
		public MgVec2f PointSizeRange  { get; set; } // 2
		public MgVec2f LineWidthRange  { get; set; } // 2
		public float PointSizeGranularity { get; set; }
		public float LineWidthGranularity { get; set; }
		public bool StrictLines { get; set; }
		public bool StandardSampleLocations { get; set; }
		public UInt64 OptimalBufferCopyOffsetAlignment { get; set; }
		public UInt64 OptimalBufferCopyRowPitchAlignment { get; set; }
		public UInt64 NonCoherentAtomSize { get; set; }
	}
}

