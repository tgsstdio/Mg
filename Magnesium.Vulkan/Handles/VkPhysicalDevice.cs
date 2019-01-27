using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Magnesium.Vulkan
{
	public class VkPhysicalDevice : IMgPhysicalDevice
	{
		internal IntPtr Handle { get; private set;}
		internal VkPhysicalDevice(IntPtr handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Allocator is optional
		/// </summary>
		/// <param name="allocator"></param>
		/// <returns></returns>
		static IntPtr GetAllocatorHandle(IMgAllocationCallbacks allocator)
		{
			var bAllocator = (MgVkAllocationCallbacks)allocator;
			return bAllocator != null ? bAllocator.Handle : IntPtr.Zero;
		}

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
        {
            var pCreateInfo = default(VkPhysicalDeviceProperties);
            Interops.vkGetPhysicalDeviceProperties(Handle, ref pCreateInfo);

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

        public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			unsafe
			{
				var count = stackalloc uint[1];
				count[0] = 0;
				Interops.vkGetPhysicalDeviceQueueFamilyProperties(Handle, count, null);

				var queueFamilyCount = (int) count[0];
				var familyProperties = stackalloc VkQueueFamilyProperties[queueFamilyCount];

				Interops.vkGetPhysicalDeviceQueueFamilyProperties(Handle, count, familyProperties);

				pQueueFamilyProperties = new MgQueueFamilyProperties[queueFamilyCount];
				for (var i = 0; i < queueFamilyCount; ++i)
                {
                    pQueueFamilyProperties[i] = TranslateQueueFamilyProperties(ref familyProperties[i]);
                }
            }
		}

        private static unsafe MgQueueFamilyProperties TranslateQueueFamilyProperties(
            ref VkQueueFamilyProperties src)
        {
            return new MgQueueFamilyProperties
            {
                QueueFlags = src.queueFlags,
                QueueCount = src.queueCount,
                TimestampValidBits = src.timestampValidBits,
                MinImageTransferGranularity = src.minImageTransferGranularity,
            };
        }

        public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
        {
            var memoryProperties = default(VkPhysicalDeviceMemoryProperties);
            Interops.vkGetPhysicalDeviceMemoryProperties(Handle, ref memoryProperties);

            pMemoryProperties = TranslateMemoryProperties(ref memoryProperties);
        }

        private static MgPhysicalDeviceMemoryProperties TranslateMemoryProperties(ref VkPhysicalDeviceMemoryProperties memoryProperties)
        {
            MgPhysicalDeviceMemoryProperties pMemoryProperties;
            var memoryHeaps = new MgMemoryHeap[memoryProperties.memoryHeapCount];
            for (var i = 0; i < memoryProperties.memoryHeapCount; ++i)
            {
                memoryHeaps[i] = new MgMemoryHeap
                {
                    Size = memoryProperties.memoryHeaps[i].size,
                    Flags = (MgMemoryHeapFlagBits)memoryProperties.memoryHeaps[i].flags,
                };
            }

            var memoryTypes = new MgMemoryType[memoryProperties.memoryTypeCount];
            for (var i = 0; i < memoryProperties.memoryTypeCount; ++i)
            {
                memoryTypes[i] = new MgMemoryType
                {
                    PropertyFlags = (uint)memoryProperties.memoryTypes[i].propertyFlags,
                    HeapIndex = memoryProperties.memoryTypes[i].heapIndex,
                };
            }

            pMemoryProperties = new MgPhysicalDeviceMemoryProperties
            {
                MemoryHeaps = memoryHeaps,
                MemoryTypes = memoryTypes,
            };
            return pMemoryProperties;
        }

        public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
        {
            var features = default(VkPhysicalDeviceFeatures);

            Interops.vkGetPhysicalDeviceFeatures(Handle, ref features);

            pFeatures = TranslateFeatures(ref features);
        }

        private static MgPhysicalDeviceFeatures TranslateFeatures(ref VkPhysicalDeviceFeatures features)
        {
            return new MgPhysicalDeviceFeatures
            {
                RobustBufferAccess = VkBool32.ConvertFrom(features.robustBufferAccess),
                FullDrawIndexUint32 = VkBool32.ConvertFrom(features.fullDrawIndexUint32),
                ImageCubeArray = VkBool32.ConvertFrom(features.imageCubeArray),
                IndependentBlend = VkBool32.ConvertFrom(features.independentBlend),
                GeometryShader = VkBool32.ConvertFrom(features.geometryShader),
                TessellationShader = VkBool32.ConvertFrom(features.tessellationShader),
                SampleRateShading = VkBool32.ConvertFrom(features.sampleRateShading),
                DualSrcBlend = VkBool32.ConvertFrom(features.dualSrcBlend),
                LogicOp = VkBool32.ConvertFrom(features.logicOp),
                MultiDrawIndirect = VkBool32.ConvertFrom(features.multiDrawIndirect),
                DrawIndirectFirstInstance = VkBool32.ConvertFrom(features.drawIndirectFirstInstance),
                DepthClamp = VkBool32.ConvertFrom(features.depthClamp),
                DepthBiasClamp = VkBool32.ConvertFrom(features.depthBiasClamp),
                FillModeNonSolid = VkBool32.ConvertFrom(features.fillModeNonSolid),
                DepthBounds = VkBool32.ConvertFrom(features.depthBounds),
                WideLines = VkBool32.ConvertFrom(features.wideLines),
                LargePoints = VkBool32.ConvertFrom(features.largePoints),
                AlphaToOne = VkBool32.ConvertFrom(features.alphaToOne),
                MultiViewport = VkBool32.ConvertFrom(features.multiViewport),
                SamplerAnisotropy = VkBool32.ConvertFrom(features.samplerAnisotropy),
                TextureCompressionETC2 = VkBool32.ConvertFrom(features.textureCompressionETC2),
                TextureCompressionASTC_LDR = VkBool32.ConvertFrom(features.textureCompressionASTC_LDR),
                TextureCompressionBC = VkBool32.ConvertFrom(features.textureCompressionBC),
                OcclusionQueryPrecise = VkBool32.ConvertFrom(features.occlusionQueryPrecise),
                PipelineStatisticsQuery = VkBool32.ConvertFrom(features.pipelineStatisticsQuery),
                VertexPipelineStoresAndAtomics = VkBool32.ConvertFrom(features.vertexPipelineStoresAndAtomics),
                FragmentStoresAndAtomics = VkBool32.ConvertFrom(features.fragmentStoresAndAtomics),
                ShaderTessellationAndGeometryPointSize = VkBool32.ConvertFrom(features.shaderTessellationAndGeometryPointSize),
                ShaderImageGatherExtended = VkBool32.ConvertFrom(features.shaderImageGatherExtended),
                ShaderStorageImageExtendedFormats = VkBool32.ConvertFrom(features.shaderStorageImageExtendedFormats),
                ShaderStorageImageMultisample = VkBool32.ConvertFrom(features.shaderStorageImageMultisample),
                ShaderStorageImageReadWithoutFormat = VkBool32.ConvertFrom(features.shaderStorageImageReadWithoutFormat),
                ShaderStorageImageWriteWithoutFormat = VkBool32.ConvertFrom(features.shaderStorageImageWriteWithoutFormat),
                ShaderUniformBufferArrayDynamicIndexing = VkBool32.ConvertFrom(features.shaderUniformBufferArrayDynamicIndexing),
                ShaderSampledImageArrayDynamicIndexing = VkBool32.ConvertFrom(features.shaderSampledImageArrayDynamicIndexing),
                ShaderStorageBufferArrayDynamicIndexing = VkBool32.ConvertFrom(features.shaderStorageBufferArrayDynamicIndexing),
                ShaderStorageImageArrayDynamicIndexing = VkBool32.ConvertFrom(features.shaderStorageImageArrayDynamicIndexing),
                ShaderClipDistance = VkBool32.ConvertFrom(features.shaderClipDistance),
                ShaderCullDistance = VkBool32.ConvertFrom(features.shaderCullDistance),
                ShaderFloat64 = VkBool32.ConvertFrom(features.shaderFloat64),
                ShaderInt64 = VkBool32.ConvertFrom(features.shaderInt64),
                ShaderInt16 = VkBool32.ConvertFrom(features.shaderInt16),
                ShaderResourceResidency = VkBool32.ConvertFrom(features.shaderResourceResidency),
                ShaderResourceMinLod = VkBool32.ConvertFrom(features.shaderResourceMinLod),
                SparseBinding = VkBool32.ConvertFrom(features.sparseBinding),
                SparseResidencyBuffer = VkBool32.ConvertFrom(features.sparseResidencyBuffer),
                SparseResidencyImage2D = VkBool32.ConvertFrom(features.sparseResidencyImage2D),
                SparseResidencyImage3D = VkBool32.ConvertFrom(features.sparseResidencyImage3D),
                SparseResidency2Samples = VkBool32.ConvertFrom(features.sparseResidency2Samples),
                SparseResidency4Samples = VkBool32.ConvertFrom(features.sparseResidency4Samples),
                SparseResidency8Samples = VkBool32.ConvertFrom(features.sparseResidency8Samples),
                SparseResidency16Samples = VkBool32.ConvertFrom(features.sparseResidency16Samples),
                SparseResidencyAliased = VkBool32.ConvertFrom(features.sparseResidencyAliased),
                VariableMultisampleRate = VkBool32.ConvertFrom(features.variableMultisampleRate),
                InheritedQueries = VkBool32.ConvertFrom(features.inheritedQueries),
            };
        }

        public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
        {
            var formatProperties = default(VkFormatProperties);
            Interops.vkGetPhysicalDeviceFormatProperties(Handle, format, ref formatProperties);

            pFormatProperties = TranslateFormatProperties(format, ref formatProperties);
        }

        private static MgFormatProperties TranslateFormatProperties(MgFormat format, ref VkFormatProperties formatProperties)
        {
            return new MgFormatProperties
            {
                Format = format,
                LinearTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.linearTilingFeatures,
                OptimalTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.optimalTilingFeatures,
                BufferFeatures = (MgFormatFeatureFlagBits)formatProperties.bufferFeatures,
            };
        }

        public MgResult GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			var bType = (VkImageType)type;

			var properties = default(VkImageFormatProperties);
			var result = Interops.vkGetPhysicalDeviceImageFormatProperties
			(
				Handle,
				format,
				bType,
				tiling,
				usage,
				flags,
				ref properties
		   );

			pImageFormatProperties = new MgImageFormatProperties
			{
				MaxExtent = properties.maxExtent,
				MaxMipLevels = properties.maxMipLevels,
				MaxArrayLayers = properties.maxArrayLayers,
				SampleCounts = (MgSampleCountFlagBits)properties.sampleCounts,
				MaxResourceSize = properties.maxResourceSize,
			};
			return result;
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="pCreateInfo"></param>
        /// <param name="allocator"></param>
        /// <param name="pDevice"></param>
        /// <returns></returns>
        public MgResult CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			var allocatorPtr = GetAllocatorHandle(allocator);

			var attachedItems = new List<IntPtr>();

			try
			{
				var queueCreateInfoCount = 0U;
				var pQueueCreateInfos = IntPtr.Zero;
                if (pCreateInfo.QueueCreateInfos != null)
                {
                    queueCreateInfoCount = (uint)pCreateInfo.QueueCreateInfos.Length;
                    if (queueCreateInfoCount > 0)
                    {
                        pQueueCreateInfos = GenerateQueueCreateInfos(attachedItems, pCreateInfo.QueueCreateInfos);
                    }
                }

				var enabledExtensionCount = 0U;
                var ppEnabledExtensionNames = VkInteropsUtility.CopyStringArrays(attachedItems, pCreateInfo.EnabledExtensionNames, out enabledExtensionCount);

                var pEnabledFeatures = GenerateEnabledFeatures(attachedItems, pCreateInfo.EnabledFeatures);

				var internalHandle = IntPtr.Zero;
				var createInfo = new VkDeviceCreateInfo
				{
					sType = VkStructureType.StructureTypeDeviceCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					queueCreateInfoCount = queueCreateInfoCount,
					pQueueCreateInfos = pQueueCreateInfos,

                    // https://www.khronos.org/registry/vulkan/specs/1.0-wsi_extensions/xhtml/vkspec.html#extended-functionality-device-layer-deprecation
                    ppEnabledLayerNames = IntPtr.Zero,
                    enabledLayerCount = 0U,

                    enabledExtensionCount = enabledExtensionCount,
					ppEnabledExtensionNames = ppEnabledExtensionNames,
					pEnabledFeatures = pEnabledFeatures,
				};
				var result = Interops.vkCreateDevice(Handle, ref createInfo, allocatorPtr, ref internalHandle);
				pDevice = new VkDevice(internalHandle);
				return result;
			}
			finally
			{
				foreach (var item in attachedItems)
				{
					Marshal.FreeHGlobal(item);
				}
			}
		}


		IntPtr GenerateEnabledFeatures(List<IntPtr> attachedItems, MgPhysicalDeviceFeatures enabledFeatures)
		{
			if (enabledFeatures == null)
				return IntPtr.Zero;

			var dataItem = new VkPhysicalDeviceFeatures
			{
				robustBufferAccess = VkBool32.ConvertTo(enabledFeatures.RobustBufferAccess),
				fullDrawIndexUint32 = VkBool32.ConvertTo(enabledFeatures.FullDrawIndexUint32),
				imageCubeArray = VkBool32.ConvertTo(enabledFeatures.ImageCubeArray),
				independentBlend = VkBool32.ConvertTo(enabledFeatures.IndependentBlend),
				geometryShader = VkBool32.ConvertTo(enabledFeatures.GeometryShader),
				tessellationShader = VkBool32.ConvertTo(enabledFeatures.TessellationShader),
				sampleRateShading = VkBool32.ConvertTo(enabledFeatures.SampleRateShading),
				dualSrcBlend = VkBool32.ConvertTo(enabledFeatures.DualSrcBlend),
				logicOp = VkBool32.ConvertTo(enabledFeatures.LogicOp),
				multiDrawIndirect = VkBool32.ConvertTo(enabledFeatures.MultiDrawIndirect),
				drawIndirectFirstInstance = VkBool32.ConvertTo(enabledFeatures.DrawIndirectFirstInstance),
				depthClamp = VkBool32.ConvertTo(enabledFeatures.DepthClamp),
				depthBiasClamp = VkBool32.ConvertTo(enabledFeatures.DepthBiasClamp),
				fillModeNonSolid = VkBool32.ConvertTo(enabledFeatures.FillModeNonSolid),
				depthBounds = VkBool32.ConvertTo(enabledFeatures.DepthBounds),
				wideLines = VkBool32.ConvertTo(enabledFeatures.WideLines),
				largePoints = VkBool32.ConvertTo(enabledFeatures.LargePoints),
				alphaToOne = VkBool32.ConvertTo(enabledFeatures.AlphaToOne),
				multiViewport = VkBool32.ConvertTo(enabledFeatures.MultiViewport),
				samplerAnisotropy = VkBool32.ConvertTo(enabledFeatures.SamplerAnisotropy),
				textureCompressionETC2 = VkBool32.ConvertTo(enabledFeatures.TextureCompressionETC2),
				textureCompressionASTC_LDR = VkBool32.ConvertTo(enabledFeatures.TextureCompressionASTC_LDR),
				textureCompressionBC = VkBool32.ConvertTo(enabledFeatures.TextureCompressionBC),
				occlusionQueryPrecise = VkBool32.ConvertTo(enabledFeatures.OcclusionQueryPrecise),
				pipelineStatisticsQuery = VkBool32.ConvertTo(enabledFeatures.PipelineStatisticsQuery),
				vertexPipelineStoresAndAtomics = VkBool32.ConvertTo(enabledFeatures.VertexPipelineStoresAndAtomics),
				fragmentStoresAndAtomics = VkBool32.ConvertTo(enabledFeatures.FragmentStoresAndAtomics),
				shaderTessellationAndGeometryPointSize = VkBool32.ConvertTo(enabledFeatures.ShaderTessellationAndGeometryPointSize),
				shaderImageGatherExtended = VkBool32.ConvertTo(enabledFeatures.ShaderImageGatherExtended),
				shaderStorageImageExtendedFormats = VkBool32.ConvertTo(enabledFeatures.ShaderStorageImageExtendedFormats),
				shaderStorageImageMultisample = VkBool32.ConvertTo(enabledFeatures.ShaderStorageImageMultisample),
				shaderStorageImageReadWithoutFormat = VkBool32.ConvertTo(enabledFeatures.ShaderStorageImageReadWithoutFormat),
				shaderStorageImageWriteWithoutFormat = VkBool32.ConvertTo(enabledFeatures.ShaderStorageImageWriteWithoutFormat),
				shaderUniformBufferArrayDynamicIndexing = VkBool32.ConvertTo(enabledFeatures.ShaderUniformBufferArrayDynamicIndexing),
				shaderSampledImageArrayDynamicIndexing = VkBool32.ConvertTo(enabledFeatures.ShaderSampledImageArrayDynamicIndexing),
				shaderStorageBufferArrayDynamicIndexing = VkBool32.ConvertTo(enabledFeatures.ShaderStorageBufferArrayDynamicIndexing),
				shaderStorageImageArrayDynamicIndexing = VkBool32.ConvertTo(enabledFeatures.ShaderStorageImageArrayDynamicIndexing),
				shaderClipDistance = VkBool32.ConvertTo(enabledFeatures.ShaderClipDistance),
				shaderCullDistance = VkBool32.ConvertTo(enabledFeatures.ShaderCullDistance),
				shaderFloat64 = VkBool32.ConvertTo(enabledFeatures.ShaderFloat64),
				shaderInt64 = VkBool32.ConvertTo(enabledFeatures.ShaderInt64),
				shaderInt16 = VkBool32.ConvertTo(enabledFeatures.ShaderInt16),
				shaderResourceResidency = VkBool32.ConvertTo(enabledFeatures.ShaderResourceResidency),
				shaderResourceMinLod = VkBool32.ConvertTo(enabledFeatures.ShaderResourceMinLod),
				sparseBinding = VkBool32.ConvertTo(enabledFeatures.SparseBinding),
				sparseResidencyBuffer = VkBool32.ConvertTo(enabledFeatures.SparseResidencyBuffer),
				sparseResidencyImage2D = VkBool32.ConvertTo(enabledFeatures.SparseResidencyImage2D),
				sparseResidencyImage3D = VkBool32.ConvertTo(enabledFeatures.SparseResidencyImage3D),
				sparseResidency2Samples = VkBool32.ConvertTo(enabledFeatures.SparseResidency2Samples),
				sparseResidency4Samples = VkBool32.ConvertTo(enabledFeatures.SparseResidency4Samples),
				sparseResidency8Samples = VkBool32.ConvertTo(enabledFeatures.SparseResidency8Samples),
				sparseResidency16Samples = VkBool32.ConvertTo(enabledFeatures.SparseResidency16Samples),
				sparseResidencyAliased = VkBool32.ConvertTo(enabledFeatures.SparseResidencyAliased),
				variableMultisampleRate = VkBool32.ConvertTo(enabledFeatures.VariableMultisampleRate),
				inheritedQueries = VkBool32.ConvertTo(enabledFeatures.InheritedQueries),
			};

			{
				var structSize = Marshal.SizeOf(dataItem);
				var dest = Marshal.AllocHGlobal(structSize);
				attachedItems.Add(dest);
				Marshal.StructureToPtr(dataItem, dest, false);
				return dest;
			}
		}  

		IntPtr GenerateQueueCreateInfos(List<IntPtr> attachedItems, MgDeviceQueueCreateInfo[] queueCreateInfos)
		{
			var pQueueCreateInfos = VkInteropsUtility.AllocateNestedHGlobalArray(
                attachedItems,
                queueCreateInfos,
				(items, qcr) =>
				{
					var queueCount = qcr.QueueCount;

					Debug.Assert(qcr.QueuePriorities != null);
					int arrayLength = qcr.QueuePriorities.Length;
					Debug.Assert(qcr.QueueCount == arrayLength);

					var pQueuePriorities = Marshal.AllocHGlobal(sizeof(float) * arrayLength);
                    items.Add(pQueuePriorities);
					Marshal.Copy(qcr.QueuePriorities, 0, pQueuePriorities, arrayLength);

					return new VkDeviceQueueCreateInfo
					{
						sType = VkStructureType.StructureTypeDeviceQueueCreateInfo,
						pNext = IntPtr.Zero,
						flags = qcr.Flags,
						queueFamilyIndex = qcr.QueueFamilyIndex,
						queueCount = qcr.QueueCount,
						pQueuePriorities = pQueuePriorities,
					};
				});
			attachedItems.Add(pQueueCreateInfos);

			return pQueueCreateInfos;
		}

		public MgResult EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
		{
			uint count = 0U;
			var first = Interops.vkEnumerateDeviceLayerProperties(Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pProperties = null;
				return first;
			}

			var layers = new VkLayerProperties[count];
			var final = Interops.vkEnumerateDeviceLayerProperties(Handle, ref count, layers);

			pProperties = new MgLayerProperties[count];
			for (var i = 0; i < count; ++i)
			{
				pProperties[i] = new MgLayerProperties
				{
					LayerName = VkInteropsUtility.ByteArrayToTrimmedString(layers[i].layerName),
					SpecVersion = layers[i].specVersion,
					ImplementationVersion = layers[i].implementationVersion,
					Description = VkInteropsUtility.ByteArrayToTrimmedString(layers[i].description),
				};
			}

			return final;
		}

		public MgResult EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			var bLayerName = IntPtr.Zero;

			try
			{
				if (layerName != null)
				{
					bLayerName = VkInteropsUtility.NativeUtf8FromString(layerName);
				}
				uint count = 0;
				var first = Interops.vkEnumerateDeviceExtensionProperties(Handle, bLayerName, ref count, null);

				if (first != MgResult.SUCCESS)
				{
					pProperties = null;
					return first;
				}

				var extensions = new VkExtensionProperties[count];
				var final = Interops.vkEnumerateDeviceExtensionProperties(Handle, bLayerName, ref count, extensions);

				pProperties = new MgExtensionProperties[count];
				for (var i = 0; i < count; ++i)
				{
					pProperties[i] = new MgExtensionProperties
					{
						ExtensionName =
                            VkInteropsUtility.ByteArrayToTrimmedString(extensions[i].extensionName),
						SpecVersion = extensions[i].specVersion,
					};
				}

				return final;

			}
			finally
			{
				if (bLayerName != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(bLayerName);
				}
			}
		}

		public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
			uint count = 0;

			var bType = (VkImageType) type;
			var bSamples = samples;

			Interops.vkGetPhysicalDeviceSparseImageFormatProperties
			(
				Handle,
				format,
				bType,
				bSamples,
				usage,
				tiling,
				ref count,
				null
	   	    );

			if (count == 0)
			{
				pProperties = new MgSparseImageFormatProperties[0];
				return;
			}

			var formatProperties = new VkSparseImageFormatProperties[count];
			Interops.vkGetPhysicalDeviceSparseImageFormatProperties
			(
				Handle,
				format,
				bType,
				bSamples,
				usage,
				tiling,
				ref count,
				formatProperties
		   	);

			pProperties = new MgSparseImageFormatProperties[count];
			for (var i = 0; i < count; ++i)
            {
                pProperties[i] = TranslateSparseImageFormatProperties(ref formatProperties[i]);
            }
        }

        private static MgSparseImageFormatProperties TranslateSparseImageFormatProperties(ref VkSparseImageFormatProperties src)
        {
            return new MgSparseImageFormatProperties
            {
                AspectMask = src.aspectMask,
                ImageGranularity = src.imageGranularity,
                Flags = (MgSparseImageFormatFlagBits) src.flags,
            };
        }

        public MgResult GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
		{
			uint count = 0;
			var first = Interops.vkGetPhysicalDeviceDisplayPropertiesKHR(Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pProperties = null;
				return first;
			}

			var displayProperties = new VkDisplayPropertiesKHR[count];
			var final = Interops.vkGetPhysicalDeviceDisplayPropertiesKHR(Handle, ref count, displayProperties);

			pProperties = new MgDisplayPropertiesKHR[count];
			for (var i = 0; i < count; ++i)
			{
				var internalDisplay = new VkDisplayKHR(displayProperties[i].display);

				pProperties[i] = new MgDisplayPropertiesKHR
				{
					Display = internalDisplay,
					DisplayName = displayProperties[i].displayName,
					PhysicalDimensions = displayProperties[i].physicalDimensions,
					PhysicalResolution = displayProperties[i].physicalResolution,
					SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)displayProperties[i].supportedTransforms,
					PlaneReorderPossible = VkBool32.ConvertFrom(displayProperties[i].planeReorderPossible),
					PersistentContent = VkBool32.ConvertFrom(displayProperties[i].persistentContent),
				};
			}
			return final;
		}

		public MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			uint count = 0;
			var first = Interops.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pProperties = null;
				return first;
			}

			var planeProperties = new VkDisplayPlanePropertiesKHR[count];
			var final = Interops.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, ref count, planeProperties);

			pProperties = new MgDisplayPlanePropertiesKHR[count];
			for (var i = 0; i < count; ++i)
			{
				pProperties[i] = new MgDisplayPlanePropertiesKHR
				{
					CurrentDisplay = new VkDisplayKHR(planeProperties[i].currentDisplay),
					CurrentStackIndex = planeProperties[i].currentStackIndex,
				};
			}

			return final;
		}

		public MgResult GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			uint count = 0;
			var first = Interops.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pDisplays = null;
				return first;
			}

			var supportedDisplays = new ulong[count];
			var final = Interops.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, ref count, supportedDisplays);

			pDisplays = new VkDisplayKHR[count];
			for (var i = 0; i < count; ++i)
			{
				pDisplays[i] = new VkDisplayKHR(supportedDisplays[i]);
			}

			return final;
		}

		public MgResult GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			if (display == null)
				throw new ArgumentNullException(nameof(display));

			var bDisplay = (VkDisplayKHR)display;
			Debug.Assert(bDisplay != null); // MAYBE DUPLICATE CHECK
			uint count = 0;
			var first = Interops.vkGetDisplayModePropertiesKHR(Handle, bDisplay.Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pProperties = null;
				return first;
			}

			var modeProperties = new VkDisplayModePropertiesKHR[count];
			var final = Interops.vkGetDisplayModePropertiesKHR(Handle, bDisplay.Handle, ref count, modeProperties);

			pProperties = new MgDisplayModePropertiesKHR[count];
			for (var i = 0; i < count; ++i)
			{
				pProperties[i] = new MgDisplayModePropertiesKHR
				{
					DisplayMode = new VkDisplayModeKHR(modeProperties[i].displayMode),
					Parameters = modeProperties[i].parameters,
				};
			}

			return final;
		}

		public MgResult GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			if (mode == null)
				throw new ArgumentNullException(nameof(mode));

			var bMode = (VkDisplayModeKHR)mode;
			Debug.Assert(bMode != null);

			var capabilities = default(VkDisplayPlaneCapabilitiesKHR);
			var result = Interops.vkGetDisplayPlaneCapabilitiesKHR(Handle, bMode.Handle, planeIndex, ref capabilities);

			pCapabilities = new MgDisplayPlaneCapabilitiesKHR
			{
				SupportedAlpha = (MgDisplayPlaneAlphaFlagBitsKHR) capabilities.supportedAlpha,
				MinSrcPosition = capabilities.minSrcPosition,
				MaxSrcPosition = capabilities.maxSrcPosition,
				MinSrcExtent = capabilities.minSrcExtent,
				MaxSrcExtent = capabilities.maxSrcExtent,
				MinDstPosition = capabilities.minDstPosition,
				MaxDstPosition = capabilities.maxDstPosition,
				MinDstExtent = capabilities.minDstExtent,
				MaxDstExtent = capabilities.maxDstExtent,
			};
			return result;
		}

		public MgResult GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			VkBool32 isSupported = default(VkBool32);
			var result = Interops.vkGetPhysicalDeviceSurfaceSupportKHR(Handle, queueFamilyIndex, bSurface.Handle, ref isSupported);
			pSupported = VkBool32.ConvertFrom(isSupported);
			return result;
		}

		public MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));

            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var pCreateInfo = default(VkSurfaceCapabilitiesKHR);
            var result = Interops.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(Handle, bSurface.Handle, ref pCreateInfo);
            pSurfaceCapabilities = TranslateSurfaceCapabilities(ref pCreateInfo);

            return result;
        }

        private static MgSurfaceCapabilitiesKHR TranslateSurfaceCapabilities(ref VkSurfaceCapabilitiesKHR src)
        {
            return new MgSurfaceCapabilitiesKHR
            {
                MinImageCount = src.minImageCount,
                MaxImageCount = src.maxImageCount,
                CurrentExtent = src.currentExtent,
                MinImageExtent = src.minImageExtent,
                MaxImageExtent = src.maxImageExtent,
                MaxImageArrayLayers = src.maxImageArrayLayers,
                SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)src.supportedTransforms,
                CurrentTransform = (MgSurfaceTransformFlagBitsKHR)src.currentTransform,
                SupportedCompositeAlpha = (MgCompositeAlphaFlagBitsKHR)src.supportedCompositeAlpha,
                SupportedUsageFlags = (MgImageUsageFlagBits)src.supportedUsageFlags,
            };
        }

        public MgResult GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			var count = 0U;
			var first = Interops.vkGetPhysicalDeviceSurfaceFormatsKHR(Handle, bSurface.Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pSurfaceFormats = null;
				return first;
			}

			var surfaceFormats = new VkSurfaceFormatKHR[count];
			var final = Interops.vkGetPhysicalDeviceSurfaceFormatsKHR(Handle, bSurface.Handle, ref count, surfaceFormats);

			pSurfaceFormats = new MgSurfaceFormatKHR[count];
			for (var i = 0; i < count; ++i)
			{
				pSurfaceFormats[i] = new MgSurfaceFormatKHR
				{
					Format = (MgFormat)surfaceFormats[i].format,
					ColorSpace = (MgColorSpaceKHR)surfaceFormats[i].colorSpace,
				};
			}

			return final;
		}

		public MgResult GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			var count = 0U;
			var first = Interops.vkGetPhysicalDeviceSurfacePresentModesKHR(Handle, bSurface.Handle, ref count, null);

			if (first != MgResult.SUCCESS)
			{
				pPresentModes = null;
				return first;
			}

			var modes = new VkPresentModeKhr[count];
			var final = Interops.vkGetPhysicalDeviceSurfacePresentModesKHR(Handle, bSurface.Handle, ref count, modes);

			pPresentModes = new MgPresentModeKHR[count];
			for (var i = 0; i < count; ++i)
			{
				pPresentModes[i] = (MgPresentModeKHR)modes[i];
			}

			return final;
		}

		public bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex)
		{
			var final = Interops.vkGetPhysicalDeviceWin32PresentationSupportKHR(Handle, queueFamilyIndex);
			return VkBool32.ConvertFrom(final);
		}

		public MgResult CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
		{
			if (display == null)
				throw new ArgumentNullException(nameof(display));

			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));
			

			var bDisplay = (VkDisplayModeKHR)display;
			Debug.Assert(bDisplay != null);

			var allocatorPtr = GetAllocatorHandle(allocator);

			var createInfo = new VkDisplayModeCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeDisplayModeCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.flags,
				parameters = pCreateInfo.parameters,
			};

			var modeHandle = 0UL;
			var result = Interops.vkCreateDisplayModeKHR(this.Handle, bDisplay.Handle, ref createInfo, allocatorPtr, ref modeHandle);
			pMode = new VkDisplayModeKHR(modeHandle);

			return result;
		}

        public MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            var bType = (VkImageType)type;      

            var properties = default(VkExternalImageFormatPropertiesNV);

            var result = Interops.vkGetPhysicalDeviceExternalImageFormatPropertiesNV(
                this.Handle,
                format,
                bType,
                tiling,
                usage,               
                flags,
                externalHandleType,
                ref properties);

            pExternalImageFormatProperties = new MgExternalImageFormatPropertiesNV
            {
                ImageFormatProperties = new MgImageFormatProperties
                {
                    MaxExtent = properties.imageFormatProperties.maxExtent,
                    MaxArrayLayers = properties.imageFormatProperties.maxArrayLayers,
                    MaxMipLevels = properties.imageFormatProperties.maxMipLevels,
                    MaxResourceSize = properties.imageFormatProperties.maxResourceSize,
                    SampleCounts = (MgSampleCountFlagBits) properties.imageFormatProperties.sampleCounts,
                },
                CompatibleHandleTypes = properties.compatibleHandleTypes,
                ExportFromImportedHandleTypes = properties.exportFromImportedHandleTypes,
                ExternalMemoryFeatures = properties.externalMemoryFeatures,
            };

            return result;
        }

        public MgResult GetPhysicalDevicePresentRectanglesKHR(IMgSurfaceKHR surface, MgRect2D[] pRects)
        {
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));

            var hRects = GCHandle.Alloc(pRects, GCHandleType.Pinned);

            try
            {
                var bSurface = (VkSurfaceKHR)surface;

                unsafe
                {
                    var count = (UInt32)pRects.Length;
                    var pinnedObject = hRects.AddrOfPinnedObject();

                    var bRects = (MgRect2D*)pinnedObject.ToPointer();

                    return Interops.vkGetPhysicalDevicePresentRectanglesKHR(this.Handle, bSurface.Handle, count, bRects);
                }
            }
            finally
            {
                hRects.Free();
            }
        }

        // TODO: BELOW EXTENSION methods via pNext linked

        public MgResult GetDisplayModeProperties2KHR(IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
        {
            if (display == null)
                throw new ArgumentNullException(nameof(display));

            var bDisplay = (VkDisplayKHR)display;
            Debug.Assert(bDisplay != null); // MAYBE DUPLICATE CHECK
            uint count = 0;
            var first = Interops.vkGetDisplayModeProperties2KHR(Handle, bDisplay.Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var modeProperties = new VkDisplayModeProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                modeProperties[i] = new VkDisplayModeProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayModeProperties2Khr,
                    // TODO: extension modeProperties[i].pNext ???
                    pNext = IntPtr.Zero,
                };
            }

            var final = Interops.vkGetDisplayModeProperties2KHR(
                Handle, bDisplay.Handle, ref count, modeProperties);

            pProperties = new MgDisplayModeProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var current = modeProperties[i].displayModeProperties;
                pProperties[i] = new MgDisplayModeProperties2KHR
                {
                    DisplayModeProperties = new MgDisplayModePropertiesKHR
                    {
                        DisplayMode = new VkDisplayModeKHR(current.displayMode),
                        Parameters = current.parameters,
                    },
                };
            }

            return final;
        }

        public MgResult GetDisplayPlaneCapabilities2KHR(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
        {
            if (pDisplayPlaneInfo == null)
                throw new ArgumentNullException(nameof(pDisplayPlaneInfo));

            if (pDisplayPlaneInfo.Mode == null)
                throw new ArgumentNullException(nameof(pDisplayPlaneInfo.Mode));

            var bMode = (VkDisplayModeKHR)pDisplayPlaneInfo.Mode;
            Debug.Assert(bMode != null);

            var bDisplayPlaneInfo = new VkDisplayPlaneInfo2KHR
            {
                sType = VkStructureType.StructureTypeDisplayPlaneInfo2Khr,
                pNext = IntPtr.Zero, // TODO: extension
                mode = bMode.Handle,
                planeIndex = pDisplayPlaneInfo.PlaneIndex,
            };

            var output = new VkDisplayPlaneCapabilities2KHR
            {
                sType = VkStructureType.StructureTypeDisplayPlaneCapabilities2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = Interops.vkGetDisplayPlaneCapabilities2KHR(Handle, ref bDisplayPlaneInfo, ref output);

            var caps = output.capabilities;
            pCapabilities = new MgDisplayPlaneCapabilities2KHR
            {
                Capabilities = new MgDisplayPlaneCapabilitiesKHR
                {
                    SupportedAlpha = (MgDisplayPlaneAlphaFlagBitsKHR)caps.supportedAlpha,
                    MinSrcPosition = caps.minSrcPosition,
                    MaxSrcPosition = caps.maxSrcPosition,
                    MinSrcExtent = caps.minSrcExtent,
                    MaxSrcExtent = caps.maxSrcExtent,
                    MinDstPosition = caps.minDstPosition,
                    MaxDstPosition = caps.maxDstPosition,
                    MinDstExtent = caps.minDstExtent,
                    MaxDstExtent = caps.maxDstExtent,
                }
            };
            return result;
        }

        public MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(out MgTimeDomainEXT[] pTimeDomains)
        {
            var pTimeDomainCount = 0U;
            var result = Interops.vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(this.Handle, ref pTimeDomainCount, null);

            if (result != MgResult.SUCCESS)
            {
                pTimeDomains = new MgTimeDomainEXT[0];
                return result;
            }

            pTimeDomains = new MgTimeDomainEXT[pTimeDomainCount];
            return Interops.vkGetPhysicalDeviceCalibrateableTimeDomainsEXT(this.Handle, ref pTimeDomainCount, pTimeDomains);
        }

        public MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(out MgDisplayPlaneProperties2KHR[] pProperties)
        {
            uint count = 0;
            var first = Interops.vkGetPhysicalDeviceDisplayPlaneProperties2KHR(Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var planeProperties = new VkDisplayPlaneProperties2KHR[count];

            for (var i = 0; i < count; i += 1)
            {
                planeProperties[i] = new VkDisplayPlaneProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayPlaneProperties2Khr,
                    // TODO: extension
                    pNext = IntPtr.Zero,
                };
            }

            var final = Interops.vkGetPhysicalDeviceDisplayPlaneProperties2KHR(Handle, ref count, planeProperties);

            pProperties = new MgDisplayPlaneProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var currentProperty = planeProperties[i].displayPlaneProperties;
                pProperties[i] = new MgDisplayPlaneProperties2KHR
                {
                    DisplayPlaneProperties = new MgDisplayPlanePropertiesKHR
                    {
                        CurrentDisplay = new VkDisplayKHR(currentProperty.currentDisplay),
                        CurrentStackIndex = currentProperty.currentStackIndex,
                    }
                };
            }

            return final;
        }

        public MgResult GetPhysicalDeviceDisplayProperties2KHR(out MgDisplayProperties2KHR[] pProperties)
        {
            uint count = 0;
            var first = Interops.vkGetPhysicalDeviceDisplayProperties2KHR(Handle, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pProperties = null;
                return first;
            }

            var displayProperties = new VkDisplayProperties2KHR[count];

            for (var i = 0; i < count; i += 1)
            {
                displayProperties[i] = new VkDisplayProperties2KHR
                {
                    sType = VkStructureType.StructureTypeDisplayProperties2Khr,
                    // TODO: extension
                    pNext = IntPtr.Zero,
                };
            }

            var final = Interops.vkGetPhysicalDeviceDisplayProperties2KHR(Handle, ref count, displayProperties);

            pProperties = new MgDisplayProperties2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                var current = displayProperties[i].displayProperties;
                var internalDisplay = new VkDisplayKHR(current.display);

                pProperties[i] = new MgDisplayProperties2KHR
                {
                    DisplayProperties = new MgDisplayPropertiesKHR
                    {
                        Display = internalDisplay,
                        DisplayName = current.displayName,
                        PhysicalDimensions = current.physicalDimensions,
                        PhysicalResolution = current.physicalResolution,
                        SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)current.supportedTransforms,
                        PlaneReorderPossible = VkBool32.ConvertFrom(current.planeReorderPossible),
                        PersistentContent = VkBool32.ConvertFrom(current.persistentContent),
                    }
                };
            }
            return final;
        }

        public MgResult GetPhysicalDeviceImageFormatProperties2(MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
        {
            var bImageFormatInfo = new VkPhysicalDeviceImageFormatInfo2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceImageFormatInfo2,
                pNext = IntPtr.Zero, // TODO: extension
                type = (VkImageType) pImageFormatInfo.Type,
                flags = pImageFormatInfo.Flags,
                format = pImageFormatInfo.Format,
                tiling = pImageFormatInfo.Tiling,
                usage = pImageFormatInfo.Usage,
            };

            var output = new VkImageFormatProperties2
            {
                sType = VkStructureType.StructureTypeImageFormatProperties2,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = Interops.vkGetPhysicalDeviceImageFormatProperties2(this.Handle, ref bImageFormatInfo, ref output);

            pImageFormatProperties = new MgImageFormatProperties2
            {
                ImageFormatProperties = new MgImageFormatProperties
                {
                    MaxExtent = output.imageFormatProperties.maxExtent,
                    MaxMipLevels = output.imageFormatProperties.maxMipLevels,
                    MaxArrayLayers = output.imageFormatProperties.maxArrayLayers,
                    SampleCounts = (MgSampleCountFlagBits)output.imageFormatProperties.sampleCounts,
                    MaxResourceSize = output.imageFormatProperties.maxResourceSize,
                }
            };
            return result;
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
        {
            if (surface == null)
                throw new ArgumentNullException(nameof(surface));

            var bSurface = (VkSurfaceKHR)surface;
            Debug.Assert(bSurface != null);

            var pCreateInfo = new VkSurfaceCapabilities2EXT
            {
                sType = VkStructureType.StructureTypeSurfaceCapabilities2Ext,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = Interops.vkGetPhysicalDeviceSurfaceCapabilities2EXT(Handle, bSurface.Handle, ref pCreateInfo);

            pSurfaceCapabilities = new MgSurfaceCapabilities2EXT
            {
                MinImageCount = pCreateInfo.minImageCount,
                MaxImageCount = pCreateInfo.maxImageCount,
                CurrentExtent = pCreateInfo.currentExtent,
                MinImageExtent = pCreateInfo.minImageExtent,
                MaxImageExtent = pCreateInfo.maxImageExtent,
                MaxImageArrayLayers = pCreateInfo.maxImageArrayLayers,
                SupportedTransforms = (MgSurfaceTransformFlagBitsKHR)pCreateInfo.supportedTransforms,
                CurrentTransform = (MgSurfaceTransformFlagBitsKHR)pCreateInfo.currentTransform,
                SupportedCompositeAlpha = (MgCompositeAlphaFlagBitsKHR)pCreateInfo.supportedCompositeAlpha,
                SupportedUsageFlags = (MgImageUsageFlagBits)pCreateInfo.supportedUsageFlags,
                SupportedSurfaceCounters = pCreateInfo.supportedSurfaceCounters,                
            };

            return result;
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            if (pSurfaceInfo == null)
                throw new ArgumentNullException(nameof(pSurfaceInfo));

            var bSurface = (VkSurfaceKHR)pSurfaceInfo.Surface;
            Debug.Assert(bSurface != null);

            var bSurfaceInfo = new VkPhysicalDeviceSurfaceInfo2KHR
            {
                sType = VkStructureType.StructureTypePhysicalDeviceSurfaceInfo2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
                surface = bSurface.Handle,
            };

            var output = new VkSurfaceCapabilities2KHR
            {
                sType = VkStructureType.StructureTypeSurfaceCapabilities2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
            };

            var result = Interops.vkGetPhysicalDeviceSurfaceCapabilities2KHR(Handle, ref bSurfaceInfo, ref output);

            pSurfaceCapabilities = new MgSurfaceCapabilities2KHR
            {
                SurfaceCapabilities = TranslateSurfaceCapabilities(ref output.surfaceCapabilities),
            };

            return result;
        }

        public MgResult GetPhysicalDeviceSurfaceFormats2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
        {
            if (pSurfaceInfo == null)
                throw new ArgumentNullException(nameof(pSurfaceInfo));

            var bSurface = (VkSurfaceKHR) pSurfaceInfo.Surface;
            Debug.Assert(bSurface != null);

            var bSurfaceInfo = new VkPhysicalDeviceSurfaceInfo2KHR
            {
                sType = VkStructureType.StructureTypePhysicalDeviceSurfaceInfo2Khr,
                // TODO: extension
                pNext = IntPtr.Zero,
                surface = bSurface.Handle,
            };

            var count = 0U;
            var first = Interops.vkGetPhysicalDeviceSurfaceFormats2KHR(Handle, ref bSurfaceInfo, ref count, null);

            if (first != MgResult.SUCCESS)
            {
                pSurfaceFormats = null;
                return first;
            }

            var surfaceFormats = new VkSurfaceFormat2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                surfaceFormats[i] = new VkSurfaceFormat2KHR
                {
                    sType = VkStructureType.StructureTypeSurfaceFormat2Khr,
                    pNext = IntPtr.Zero,
                };
            }

            var final = Interops.vkGetPhysicalDeviceSurfaceFormats2KHR(Handle, ref bSurfaceInfo, ref count, surfaceFormats);

            pSurfaceFormats = new MgSurfaceFormat2KHR[count];
            for (var i = 0; i < count; ++i)
            {
                pSurfaceFormats[i] = new MgSurfaceFormat2KHR
                {
                    SurfaceFormat = TranslateSurfaceFormatKHR(ref surfaceFormats[i].surfaceFormat),
                };
            }

            return final;
        }

        private static MgSurfaceFormatKHR TranslateSurfaceFormatKHR(ref VkSurfaceFormatKHR src)
        {
            return new MgSurfaceFormatKHR
            {
                Format = (MgFormat) src.format,
                ColorSpace = (MgColorSpaceKHR) src.colorSpace,
            };
        }

        public MgResult ReleaseDisplayEXT(IMgDisplayKHR display)
        {
            if (display == null)
                throw new ArgumentNullException(nameof(display));

            var bDisplay = (VkDisplayKHR)display;
            Debug.Assert(bDisplay != null);

            return Interops.vkReleaseDisplayEXT(this.Handle, bDisplay.Handle);
        }

        public void GetPhysicalDeviceExternalBufferProperties(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
        {
            if (pExternalBufferInfo == null)
                throw new ArgumentNullException(nameof(pExternalBufferInfo));

            var bExternalBufferInfo = new VkPhysicalDeviceExternalBufferInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalBufferInfo,
                // TODO: extension
                pNext = IntPtr.Zero,
                flags = (VkBufferCreateFlags) pExternalBufferInfo.Flags,
                handleType = pExternalBufferInfo.HandleType,
                usage = pExternalBufferInfo.Usage,
            };

            var output = new VkExternalBufferProperties
            {
                sType = VkStructureType.StructureTypeExternalBufferProperties,
                pNext = IntPtr.Zero,
            };

            Interops.vkGetPhysicalDeviceExternalBufferProperties(this.Handle, ref bExternalBufferInfo, ref output);

            pExternalBufferProperties = new MgExternalBufferProperties
            {
                ExternalMemoryProperties = new MgExternalMemoryProperties
                {
                    CompatibleHandleTypes = output.externalMemoryProperties.compatibleHandleTypes,
                    ExportFromImportedHandleTypes = output.externalMemoryProperties.exportFromImportedHandleTypes,
                    ExternalMemoryFeatures = output.externalMemoryProperties.externalMemoryFeatures,                    
                },
            };

        }

        public void GetPhysicalDeviceExternalFenceProperties(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
        {
            if (pExternalFenceInfo == null)
                throw new ArgumentNullException(nameof(pExternalFenceInfo));

            var bExternalFenceInfo = new VkPhysicalDeviceExternalFenceInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalFenceInfo,
                // TODO: extension
                pNext = IntPtr.Zero,
                handleType = pExternalFenceInfo.HandleType,
            };

            var output = new VkExternalFenceProperties
            {
                sType = VkStructureType.StructureTypeExternalFenceProperties,
                pNext = IntPtr.Zero,
            };

            Interops.vkGetPhysicalDeviceExternalFenceProperties(this.Handle, ref bExternalFenceInfo, ref output);

            pExternalFenceProperties = new MgExternalFenceProperties
            {
                CompatibleHandleTypes = output.compatibleHandleTypes,
                ExternalFenceFeatures = output.externalFenceFeatures,
                ExportFromImportedHandleTypes = output.exportFromImportedHandleTypes,
            };
        }

        public void GetPhysicalDeviceExternalSemaphoreProperties(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
        {
            if (pExternalSemaphoreInfo == null)
                throw new ArgumentNullException(nameof(pExternalSemaphoreInfo));

            var bExternalSemaphoreInfo = new VkPhysicalDeviceExternalSemaphoreInfo
            {
                sType = VkStructureType.StructureTypePhysicalDeviceExternalSemaphoreInfo,
                pNext = IntPtr.Zero, // TODO: extension
                handleType = pExternalSemaphoreInfo.HandleType,
            };

            var output = new VkExternalSemaphoreProperties
            {
                sType = VkStructureType.StructureTypeExternalSemaphoreProperties,
                pNext = IntPtr.Zero,
            };

            Interops.vkGetPhysicalDeviceExternalSemaphoreProperties(this.Handle, 
                ref bExternalSemaphoreInfo,
                ref output);

            pExternalSemaphoreProperties = new MgExternalSemaphoreProperties
            {
                CompatibleHandleTypes = output.compatibleHandleTypes,
                ExportFromImportedHandleTypes = output.exportFromImportedHandleTypes,
                ExternalSemaphoreFeatures = output.externalSemaphoreFeatures,
            };
        }

        public void GetPhysicalDeviceFeatures2(out MgPhysicalDeviceFeatures2 pFeatures)
        {
            var bFeatures = new VkPhysicalDeviceFeatures2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceFeatures2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            Interops.vkGetPhysicalDeviceFeatures2(this.Handle, ref bFeatures);

            pFeatures = new MgPhysicalDeviceFeatures2
            {
                Features = TranslateFeatures(ref bFeatures.features),
            };
        }

        public void GetPhysicalDeviceFormatProperties2(MgFormat format, out MgFormatProperties2 pFormatProperties)
        {
            var output = new VkFormatProperties2
            {
                sType = VkStructureType.StructureTypeFormatProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };
            Interops.vkGetPhysicalDeviceFormatProperties2(Handle, format, ref output);

            pFormatProperties = new MgFormatProperties2
            {
                FormatProperties = TranslateFormatProperties(format, ref output.formatProperties),
            };
        }

        public void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            if (pFeatures == null)
                throw new ArgumentNullException(nameof(pFeatures));

            var bFeatures = new VkDeviceGeneratedCommandsFeaturesNVX
            {
                sType = VkStructureType.StructureTypeDeviceGeneratedCommandsFeaturesNvx,
                pNext = IntPtr.Zero, // TODO: extension
                computeBindingPointSupport = VkBool32.ConvertTo(pFeatures.ComputeBindingPointSupport),
            };

            var output = new VkDeviceGeneratedCommandsLimitsNVX
            {
                sType = VkStructureType.StructureTypeDeviceGeneratedCommandsLimitsNvx,
                pNext = IntPtr.Zero, // TODO : extension
            };

            Interops.vkGetPhysicalDeviceGeneratedCommandsPropertiesNVX(
                this.Handle,
                ref bFeatures,
                ref output);

            pLimits = new MgDeviceGeneratedCommandsLimitsNVX
            {
                MaxIndirectCommandsLayoutTokenCount = output.maxIndirectCommandsLayoutTokenCount,
                MaxObjectEntryCounts = output.maxObjectEntryCounts,
                MinCommandsTokenBufferOffsetAlignment = output.minCommandsTokenBufferOffsetAlignment,
                MinSequenceCountBufferOffsetAlignment = output.minSequenceCountBufferOffsetAlignment,
                MinSequenceIndexBufferOffsetAlignment = output.minSequenceIndexBufferOffsetAlignment,                
            };
        }

        public void GetPhysicalDeviceMemoryProperties2(out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
        {
            var bMemoryProperties = new VkPhysicalDeviceMemoryProperties2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceMemoryProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            Interops.vkGetPhysicalDeviceMemoryProperties2(this.Handle, ref bMemoryProperties);

            pMemoryProperties = new MgPhysicalDeviceMemoryProperties2
            {
                MemoryProperties = TranslateMemoryProperties(ref bMemoryProperties.memoryProperties),
            };
        }

        public void GetPhysicalDeviceMultisamplePropertiesEXT(MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
        {
            var output = new VkMultisamplePropertiesEXT
            {
                sType = VkStructureType.StructureTypeMultisamplePropertiesExt,
                pNext = IntPtr.Zero, // TODO: extension
            };

            Interops.vkGetPhysicalDeviceMultisamplePropertiesEXT(this.Handle, samples, ref output);

            pMultisampleProperties = new MgMultisamplePropertiesEXT
            {
                MaxSampleLocationGridSize = output.maxSampleLocationGridSize,
            };
        }

        public void GetPhysicalDeviceProperties2(out MgPhysicalDeviceProperties2 pProperties)
        {
            var bProperties = new VkPhysicalDeviceProperties2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceProperties2,
                pNext = IntPtr.Zero, // TODO: extension
            };

            Interops.vkGetPhysicalDeviceProperties2(this.Handle, ref bProperties);

            pProperties = new MgPhysicalDeviceProperties2
            {
                Properties = TranslateDeviceProperties(ref bProperties.properties),
            };
        }

        public void GetPhysicalDeviceQueueFamilyProperties2(out MgQueueFamilyProperties2[] pQueueFamilyProperties)
        {
            var count = 0U;
            Interops.vkGetPhysicalDeviceQueueFamilyProperties2(this.Handle, ref count, null);

            var bProperties = new VkQueueFamilyProperties2[count];

            for (var i = 0; i < count; i += 1)
            {
                bProperties[i] = new VkQueueFamilyProperties2
                {
                    sType = VkStructureType.StructureTypeQueueFamilyProperties2,
                    pNext = IntPtr.Zero, // TODO : extension
                };
            }

            if (count > 0)
            {
                Interops.vkGetPhysicalDeviceQueueFamilyProperties2(this.Handle, ref count, bProperties);
            }

            pQueueFamilyProperties = new MgQueueFamilyProperties2[count];

            for (var i = 0; i < count; i += 1)
            {
                pQueueFamilyProperties[i] = new MgQueueFamilyProperties2
                {
                    QueueFamilyProperties = TranslateQueueFamilyProperties(ref bProperties[i].queueFamilyProperties),
                };
            }
        }

        public void GetPhysicalDeviceSparseImageFormatProperties2(MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
        {
            if (pFormatInfo == null)
                throw new ArgumentNullException(nameof(pFormatInfo));

            uint count = 0;

            var bFormatInfo = new VkPhysicalDeviceSparseImageFormatInfo2
            {
                sType = VkStructureType.StructureTypePhysicalDeviceSparseImageFormatInfo2,
                pNext = IntPtr.Zero, // TODO: extension
                format = pFormatInfo.Format,
                samples = pFormatInfo.Samples,
                tiling = pFormatInfo.Tiling,
                type = (VkImageType) pFormatInfo.Type,
                usage = pFormatInfo.Usage,
            };

            Interops.vkGetPhysicalDeviceSparseImageFormatProperties2
            (
                Handle,
                ref bFormatInfo,
                ref count,
                null
            );

            
            pProperties = new MgSparseImageFormatProperties2[count];

            if (count > 0)
            {

                var bFormatProperties = new VkSparseImageFormatProperties2[count];
                for (var i = 0; i < count; i += 1)
                {
                    bFormatProperties[i] = new VkSparseImageFormatProperties2
                    {
                        sType = VkStructureType.StructureTypeSparseImageFormatProperties2,
                        pNext = IntPtr.Zero, // TODO: extension
                    };
                }

                Interops.vkGetPhysicalDeviceSparseImageFormatProperties2
                (
                    Handle,
                    ref bFormatInfo,
                    ref count,
                    bFormatProperties
                );

                for (var i = 0; i < count; i += 1)
                {
                    pProperties[i] = new MgSparseImageFormatProperties2
                    {
                        Properties = TranslateSparseImageFormatProperties(ref bFormatProperties[i].properties),
                    };
                }
            }

        }
    }
}
