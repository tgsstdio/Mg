using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkGetPhysicalDevicePropertiesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceProperties(IntPtr physicalDevice, ref VkPhysicalDeviceProperties pProperties);

        public static void GetPhysicalDeviceProperties(VkPhysicalDeviceInfo info, out MgPhysicalDeviceProperties pProperties)
        {
            var pCreateInfo = default(VkPhysicalDeviceProperties);
            vkGetPhysicalDeviceProperties(info.Handle, ref pCreateInfo);

            pProperties = TranslateDeviceProperties(ref pCreateInfo);
        }

        private static MgPhysicalDeviceProperties TranslateDeviceProperties(ref VkPhysicalDeviceProperties src)
        {
            return new MgPhysicalDeviceProperties
            {
                ApiVersion = src.apiVersion,
                DriverVersion = src.driverVersion,
                VendorID = src.vendorID,
                DeviceID = src.deviceID,
                DeviceType = (MgPhysicalDeviceType)src.deviceType,
                DeviceName = VkInteropsUtility.ByteArrayToTrimmedString(src.deviceName),
                PipelineCacheUUID = new Guid(src.pipelineCacheUUID),
                Limits = new MgPhysicalDeviceLimits
                {
                    MaxImageDimension1D = src.limits.maxImageDimension1D,
                    MaxImageDimension2D = src.limits.maxImageDimension2D,
                    MaxImageDimension3D = src.limits.maxImageDimension3D,
                    MaxImageDimensionCube = src.limits.maxImageDimensionCube,
                    MaxImageArrayLayers = src.limits.maxImageArrayLayers,
                    MaxTexelBufferElements = src.limits.maxTexelBufferElements,
                    MaxUniformBufferRange = src.limits.maxUniformBufferRange,
                    MaxStorageBufferRange = src.limits.maxStorageBufferRange,
                    MaxPushConstantsSize = src.limits.maxPushConstantsSize,
                    MaxMemoryAllocationCount = src.limits.maxMemoryAllocationCount,
                    MaxSamplerAllocationCount = src.limits.maxSamplerAllocationCount,
                    BufferImageGranularity = src.limits.bufferImageGranularity,
                    SparseAddressSpaceSize = src.limits.sparseAddressSpaceSize,
                    MaxBoundDescriptorSets = src.limits.maxBoundDescriptorSets,
                    MaxPerStageDescriptorSamplers = src.limits.maxPerStageDescriptorSamplers,
                    MaxPerStageDescriptorUniformBuffers = src.limits.maxPerStageDescriptorUniformBuffers,
                    MaxPerStageDescriptorStorageBuffers = src.limits.maxPerStageDescriptorStorageBuffers,
                    MaxPerStageDescriptorSampledImages = src.limits.maxPerStageDescriptorSampledImages,
                    MaxPerStageDescriptorStorageImages = src.limits.maxPerStageDescriptorStorageImages,
                    MaxPerStageDescriptorInputAttachments = src.limits.maxPerStageDescriptorInputAttachments,
                    MaxPerStageResources = src.limits.maxPerStageResources,
                    MaxDescriptorSetSamplers = src.limits.maxDescriptorSetSamplers,
                    MaxDescriptorSetUniformBuffers = src.limits.maxDescriptorSetUniformBuffers,
                    MaxDescriptorSetUniformBuffersDynamic = src.limits.maxDescriptorSetUniformBuffersDynamic,
                    MaxDescriptorSetStorageBuffers = src.limits.maxDescriptorSetStorageBuffers,
                    MaxDescriptorSetStorageBuffersDynamic = src.limits.maxDescriptorSetStorageBuffersDynamic,
                    MaxDescriptorSetSampledImages = src.limits.maxDescriptorSetSampledImages,
                    MaxDescriptorSetStorageImages = src.limits.maxDescriptorSetStorageImages,
                    MaxDescriptorSetInputAttachments = src.limits.maxDescriptorSetInputAttachments,
                    MaxVertexInputAttributes = src.limits.maxVertexInputAttributes,
                    MaxVertexInputBindings = src.limits.maxVertexInputBindings,
                    MaxVertexInputAttributeOffset = src.limits.maxVertexInputAttributeOffset,
                    MaxVertexInputBindingStride = src.limits.maxVertexInputBindingStride,
                    MaxVertexOutputComponents = src.limits.maxVertexOutputComponents,
                    MaxTessellationGenerationLevel = src.limits.maxTessellationGenerationLevel,
                    MaxTessellationPatchSize = src.limits.maxTessellationPatchSize,
                    MaxTessellationControlPerVertexInputComponents = src.limits.maxTessellationControlPerVertexInputComponents,
                    MaxTessellationControlPerVertexOutputComponents = src.limits.maxTessellationControlPerVertexOutputComponents,
                    MaxTessellationControlPerPatchOutputComponents = src.limits.maxTessellationControlPerPatchOutputComponents,
                    MaxTessellationControlTotalOutputComponents = src.limits.maxTessellationControlTotalOutputComponents,
                    MaxTessellationEvaluationInputComponents = src.limits.maxTessellationEvaluationInputComponents,
                    MaxTessellationEvaluationOutputComponents = src.limits.maxTessellationEvaluationOutputComponents,
                    MaxGeometryShaderInvocations = src.limits.maxGeometryShaderInvocations,
                    MaxGeometryInputComponents = src.limits.maxGeometryInputComponents,
                    MaxGeometryOutputComponents = src.limits.maxGeometryOutputComponents,
                    MaxGeometryOutputVertices = src.limits.maxGeometryOutputVertices,
                    MaxGeometryTotalOutputComponents = src.limits.maxGeometryTotalOutputComponents,
                    MaxFragmentInputComponents = src.limits.maxFragmentInputComponents,
                    MaxFragmentOutputAttachments = src.limits.maxFragmentOutputAttachments,
                    MaxFragmentDualSrcAttachments = src.limits.maxFragmentDualSrcAttachments,
                    MaxFragmentCombinedOutputResources = src.limits.maxFragmentCombinedOutputResources,
                    MaxComputeSharedMemorySize = src.limits.maxComputeSharedMemorySize,
                    MaxComputeWorkGroupCount = src.limits.maxComputeWorkGroupCount,
                    MaxComputeWorkGroupInvocations = src.limits.maxComputeWorkGroupInvocations,
                    MaxComputeWorkGroupSize = src.limits.maxComputeWorkGroupSize,
                    SubPixelPrecisionBits = src.limits.subPixelPrecisionBits,
                    SubTexelPrecisionBits = src.limits.subTexelPrecisionBits,
                    MipmapPrecisionBits = src.limits.mipmapPrecisionBits,
                    MaxDrawIndexedIndexValue = src.limits.maxDrawIndexedIndexValue,
                    MaxDrawIndirectCount = src.limits.maxDrawIndirectCount,
                    MaxSamplerLodBias = src.limits.maxSamplerLodBias,
                    MaxSamplerAnisotropy = src.limits.maxSamplerAnisotropy,
                    MaxViewports = src.limits.maxViewports,
                    MaxViewportDimensions = src.limits.maxViewportDimensions,
                    ViewportBoundsRange = src.limits.viewportBoundsRange,
                    ViewportSubPixelBits = src.limits.viewportSubPixelBits,
                    MinMemoryMapAlignment = src.limits.minMemoryMapAlignment,
                    MinTexelBufferOffsetAlignment = src.limits.minTexelBufferOffsetAlignment,
                    MinUniformBufferOffsetAlignment = src.limits.minUniformBufferOffsetAlignment,
                    MinStorageBufferOffsetAlignment = src.limits.minStorageBufferOffsetAlignment,
                    MinTexelOffset = src.limits.minTexelOffset,
                    MaxTexelOffset = src.limits.maxTexelOffset,
                    MinTexelGatherOffset = src.limits.minTexelGatherOffset,
                    MaxTexelGatherOffset = src.limits.maxTexelGatherOffset,
                    MinInterpolationOffset = src.limits.minInterpolationOffset,
                    MaxInterpolationOffset = src.limits.maxInterpolationOffset,
                    SubPixelInterpolationOffsetBits = src.limits.subPixelInterpolationOffsetBits,
                    MaxFramebufferWidth = src.limits.maxFramebufferWidth,
                    MaxFramebufferHeight = src.limits.maxFramebufferHeight,
                    MaxFramebufferLayers = src.limits.maxFramebufferLayers,
                    FramebufferColorSampleCounts = (MgSampleCountFlagBits)src.limits.framebufferColorSampleCounts,
                    FramebufferDepthSampleCounts = (MgSampleCountFlagBits)src.limits.framebufferDepthSampleCounts,
                    FramebufferStencilSampleCounts = (MgSampleCountFlagBits)src.limits.framebufferStencilSampleCounts,
                    FramebufferNoAttachmentsSampleCounts = (MgSampleCountFlagBits)src.limits.framebufferNoAttachmentsSampleCounts,
                    MaxColorAttachments = src.limits.maxColorAttachments,
                    SampledImageColorSampleCounts = (MgSampleCountFlagBits)src.limits.sampledImageColorSampleCounts,
                    SampledImageIntegerSampleCounts = (MgSampleCountFlagBits)src.limits.sampledImageIntegerSampleCounts,
                    SampledImageDepthSampleCounts = (MgSampleCountFlagBits)src.limits.sampledImageDepthSampleCounts,
                    SampledImageStencilSampleCounts = (MgSampleCountFlagBits)src.limits.sampledImageStencilSampleCounts,
                    StorageImageSampleCounts = (MgSampleCountFlagBits)src.limits.storageImageSampleCounts,
                    MaxSampleMaskWords = src.limits.maxSampleMaskWords,
                    TimestampComputeAndGraphics = VkBool32.ConvertFrom(src.limits.timestampComputeAndGraphics),
                    TimestampPeriod = src.limits.timestampPeriod,
                    MaxClipDistances = src.limits.maxClipDistances,
                    MaxCullDistances = src.limits.maxCullDistances,
                    MaxCombinedClipAndCullDistances = src.limits.maxCombinedClipAndCullDistances,
                    DiscreteQueuePriorities = src.limits.discreteQueuePriorities,
                    PointSizeRange = src.limits.pointSizeRange,
                    LineWidthRange = src.limits.lineWidthRange,
                    PointSizeGranularity = src.limits.pointSizeGranularity,
                    LineWidthGranularity = src.limits.lineWidthGranularity,
                    StrictLines = VkBool32.ConvertFrom(src.limits.strictLines),
                    StandardSampleLocations = VkBool32.ConvertFrom(src.limits.standardSampleLocations),
                    OptimalBufferCopyOffsetAlignment = src.limits.optimalBufferCopyOffsetAlignment,
                    OptimalBufferCopyRowPitchAlignment = src.limits.optimalBufferCopyRowPitchAlignment,
                    NonCoherentAtomSize = src.limits.nonCoherentAtomSize,
                },
                SparseProperties = new MgPhysicalDeviceSparseProperties
                {
                    ResidencyStandard2DBlockShape = VkBool32.ConvertFrom(src.sparseProperties.residencyStandard2DBlockShape),
                    ResidencyStandard2DMultisampleBlockShape = VkBool32.ConvertFrom(src.sparseProperties.residencyStandard2DMultisampleBlockShape),
                    ResidencyStandard3DBlockShape = VkBool32.ConvertFrom(src.sparseProperties.residencyStandard3DBlockShape),
                    ResidencyAlignedMipSize = VkBool32.ConvertFrom(src.sparseProperties.residencyAlignedMipSize),
                    ResidencyNonResidentStrict = VkBool32.ConvertFrom(src.sparseProperties.residencyNonResidentStrict),
                },
            };
        }
    }
}
