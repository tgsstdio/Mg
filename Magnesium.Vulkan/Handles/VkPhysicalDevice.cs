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
			Interops.vkGetPhysicalDeviceProperties(Handle, pCreateInfo);

			pProperties = new MgPhysicalDeviceProperties
			{
				ApiVersion = pCreateInfo.apiVersion,
				DriverVersion = pCreateInfo.driverVersion,
				VendorID = pCreateInfo.vendorID,
				DeviceID = pCreateInfo.deviceID,
				DeviceType =  (MgPhysicalDeviceType) pCreateInfo.deviceType,
				DeviceName = Encoding.UTF8.GetString(pCreateInfo.deviceName, 0, pCreateInfo.deviceName.Length),
				PipelineCacheUUID = new Guid(pCreateInfo.pipelineCacheUUID),
				Limits = new MgPhysicalDeviceLimits
				{
					MaxImageDimension1D = pCreateInfo.limits.maxImageDimension1D,
					MaxImageDimension2D = pCreateInfo.limits.maxImageDimension2D,
					MaxImageDimension3D = pCreateInfo.limits.maxImageDimension3D,
					MaxImageDimensionCube = pCreateInfo.limits.maxImageDimensionCube,
					MaxImageArrayLayers = pCreateInfo.limits.maxImageArrayLayers,
					MaxTexelBufferElements = pCreateInfo.limits.maxTexelBufferElements,
					MaxUniformBufferRange = pCreateInfo.limits.maxUniformBufferRange,
					MaxStorageBufferRange = pCreateInfo.limits.maxStorageBufferRange,
					MaxPushConstantsSize = pCreateInfo.limits.maxPushConstantsSize,
					MaxMemoryAllocationCount = pCreateInfo.limits.maxMemoryAllocationCount,
					MaxSamplerAllocationCount = pCreateInfo.limits.maxSamplerAllocationCount,
					BufferImageGranularity = pCreateInfo.limits.bufferImageGranularity,
					SparseAddressSpaceSize = pCreateInfo.limits.sparseAddressSpaceSize,
					MaxBoundDescriptorSets = pCreateInfo.limits.maxBoundDescriptorSets,
					MaxPerStageDescriptorSamplers = pCreateInfo.limits.maxPerStageDescriptorSamplers,
					MaxPerStageDescriptorUniformBuffers = pCreateInfo.limits.maxPerStageDescriptorUniformBuffers,
					MaxPerStageDescriptorStorageBuffers = pCreateInfo.limits.maxPerStageDescriptorStorageBuffers,
					MaxPerStageDescriptorSampledImages = pCreateInfo.limits.maxPerStageDescriptorSampledImages,
					MaxPerStageDescriptorStorageImages = pCreateInfo.limits.maxPerStageDescriptorStorageImages,
					MaxPerStageDescriptorInputAttachments = pCreateInfo.limits.maxPerStageDescriptorInputAttachments,
					MaxPerStageResources = pCreateInfo.limits.maxPerStageResources,
					MaxDescriptorSetSamplers = pCreateInfo.limits.maxDescriptorSetSamplers,
					MaxDescriptorSetUniformBuffers = pCreateInfo.limits.maxDescriptorSetUniformBuffers,
					MaxDescriptorSetUniformBuffersDynamic = pCreateInfo.limits.maxDescriptorSetUniformBuffersDynamic,
					MaxDescriptorSetStorageBuffers = pCreateInfo.limits.maxDescriptorSetStorageBuffers,
					MaxDescriptorSetStorageBuffersDynamic = pCreateInfo.limits.maxDescriptorSetStorageBuffersDynamic,
					MaxDescriptorSetSampledImages = pCreateInfo.limits.maxDescriptorSetSampledImages,
					MaxDescriptorSetStorageImages = pCreateInfo.limits.maxDescriptorSetStorageImages,
					MaxDescriptorSetInputAttachments = pCreateInfo.limits.maxDescriptorSetInputAttachments,
					MaxVertexInputAttributes = pCreateInfo.limits.maxVertexInputAttributes,
					MaxVertexInputBindings = pCreateInfo.limits.maxVertexInputBindings,
					MaxVertexInputAttributeOffset = pCreateInfo.limits.maxVertexInputAttributeOffset,
					MaxVertexInputBindingStride = pCreateInfo.limits.maxVertexInputBindingStride,
					MaxVertexOutputComponents = pCreateInfo.limits.maxVertexOutputComponents,
					MaxTessellationGenerationLevel = pCreateInfo.limits.maxTessellationGenerationLevel,
					MaxTessellationPatchSize = pCreateInfo.limits.maxTessellationPatchSize,
					MaxTessellationControlPerVertexInputComponents = pCreateInfo.limits.maxTessellationControlPerVertexInputComponents,
					MaxTessellationControlPerVertexOutputComponents = pCreateInfo.limits.maxTessellationControlPerVertexOutputComponents,
					MaxTessellationControlPerPatchOutputComponents = pCreateInfo.limits.maxTessellationControlPerPatchOutputComponents,
					MaxTessellationControlTotalOutputComponents = pCreateInfo.limits.maxTessellationControlTotalOutputComponents,
					MaxTessellationEvaluationInputComponents = pCreateInfo.limits.maxTessellationEvaluationInputComponents,
					MaxTessellationEvaluationOutputComponents = pCreateInfo.limits.maxTessellationEvaluationOutputComponents,
					MaxGeometryShaderInvocations = pCreateInfo.limits.maxGeometryShaderInvocations,
					MaxGeometryInputComponents = pCreateInfo.limits.maxGeometryInputComponents,
					MaxGeometryOutputComponents = pCreateInfo.limits.maxGeometryOutputComponents,
					MaxGeometryOutputVertices = pCreateInfo.limits.maxGeometryOutputVertices,
					MaxGeometryTotalOutputComponents = pCreateInfo.limits.maxGeometryTotalOutputComponents,
					MaxFragmentInputComponents = pCreateInfo.limits.maxFragmentInputComponents,
					MaxFragmentOutputAttachments = pCreateInfo.limits.maxFragmentOutputAttachments,
					MaxFragmentDualSrcAttachments = pCreateInfo.limits.maxFragmentDualSrcAttachments,
					MaxFragmentCombinedOutputResources = pCreateInfo.limits.maxFragmentCombinedOutputResources,
					MaxComputeSharedMemorySize = pCreateInfo.limits.maxComputeSharedMemorySize,
					MaxComputeWorkGroupCount = pCreateInfo.limits.maxComputeWorkGroupCount,
					MaxComputeWorkGroupInvocations = pCreateInfo.limits.maxComputeWorkGroupInvocations,
					MaxComputeWorkGroupSize = pCreateInfo.limits.maxComputeWorkGroupSize,
					SubPixelPrecisionBits = pCreateInfo.limits.subPixelPrecisionBits,
					SubTexelPrecisionBits = pCreateInfo.limits.subTexelPrecisionBits,
					MipmapPrecisionBits = pCreateInfo.limits.mipmapPrecisionBits,
					MaxDrawIndexedIndexValue = pCreateInfo.limits.maxDrawIndexedIndexValue,
					MaxDrawIndirectCount = pCreateInfo.limits.maxDrawIndirectCount,
					MaxSamplerLodBias = pCreateInfo.limits.maxSamplerLodBias,
					MaxSamplerAnisotropy = pCreateInfo.limits.maxSamplerAnisotropy,
					MaxViewports = pCreateInfo.limits.maxViewports,
					MaxViewportDimensions = pCreateInfo.limits.maxViewportDimensions,
					ViewportBoundsRange = pCreateInfo.limits.viewportBoundsRange,
					ViewportSubPixelBits = pCreateInfo.limits.viewportSubPixelBits,
					MinMemoryMapAlignment = pCreateInfo.limits.minMemoryMapAlignment,
					MinTexelBufferOffsetAlignment = pCreateInfo.limits.minTexelBufferOffsetAlignment,
					MinUniformBufferOffsetAlignment = pCreateInfo.limits.minUniformBufferOffsetAlignment,
					MinStorageBufferOffsetAlignment = pCreateInfo.limits.minStorageBufferOffsetAlignment,
					MinTexelOffset = pCreateInfo.limits.minTexelOffset,
					MaxTexelOffset = pCreateInfo.limits.maxTexelOffset,
					MinTexelGatherOffset = pCreateInfo.limits.minTexelGatherOffset,
					MaxTexelGatherOffset = pCreateInfo.limits.maxTexelGatherOffset,
					MinInterpolationOffset = pCreateInfo.limits.minInterpolationOffset,
					MaxInterpolationOffset = pCreateInfo.limits.maxInterpolationOffset,
					SubPixelInterpolationOffsetBits = pCreateInfo.limits.subPixelInterpolationOffsetBits,
					MaxFramebufferWidth = pCreateInfo.limits.maxFramebufferWidth,
					MaxFramebufferHeight = pCreateInfo.limits.maxFramebufferHeight,
					MaxFramebufferLayers = pCreateInfo.limits.maxFramebufferLayers,
					FramebufferColorSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.framebufferColorSampleCounts,
					FramebufferDepthSampleCounts = (MgSampleCountFlagBits)pCreateInfo.limits.framebufferDepthSampleCounts,
					FramebufferStencilSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.framebufferStencilSampleCounts,
					FramebufferNoAttachmentsSampleCounts = (MgSampleCountFlagBits)pCreateInfo.limits.framebufferNoAttachmentsSampleCounts,
					MaxColorAttachments = pCreateInfo.limits.maxColorAttachments,
					SampledImageColorSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.sampledImageColorSampleCounts,
					SampledImageIntegerSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.sampledImageIntegerSampleCounts,
					SampledImageDepthSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.sampledImageDepthSampleCounts,
					SampledImageStencilSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.sampledImageStencilSampleCounts,
					StorageImageSampleCounts = (MgSampleCountFlagBits) pCreateInfo.limits.storageImageSampleCounts,
					MaxSampleMaskWords = pCreateInfo.limits.maxSampleMaskWords,
					TimestampComputeAndGraphics = VkBool32.ConvertFrom(pCreateInfo.limits.timestampComputeAndGraphics),
					TimestampPeriod = pCreateInfo.limits.timestampPeriod,
					MaxClipDistances = pCreateInfo.limits.maxClipDistances,
					MaxCullDistances = pCreateInfo.limits.maxCullDistances,
					MaxCombinedClipAndCullDistances = pCreateInfo.limits.maxCombinedClipAndCullDistances,
					DiscreteQueuePriorities = pCreateInfo.limits.discreteQueuePriorities,
					PointSizeRange = pCreateInfo.limits.pointSizeRange,
					LineWidthRange = pCreateInfo.limits.lineWidthRange,
					PointSizeGranularity = pCreateInfo.limits.pointSizeGranularity,
					LineWidthGranularity = pCreateInfo.limits.lineWidthGranularity,
					StrictLines = VkBool32.ConvertFrom(pCreateInfo.limits.strictLines),
					StandardSampleLocations = VkBool32.ConvertFrom(pCreateInfo.limits.standardSampleLocations),
					OptimalBufferCopyOffsetAlignment = pCreateInfo.limits.optimalBufferCopyOffsetAlignment,
					OptimalBufferCopyRowPitchAlignment = pCreateInfo.limits.optimalBufferCopyRowPitchAlignment,
					NonCoherentAtomSize = pCreateInfo.limits.nonCoherentAtomSize,
				},
				SparseProperties = new MgPhysicalDeviceSparseProperties
				{
					ResidencyStandard2DBlockShape = VkBool32.ConvertFrom(pCreateInfo.sparseProperties.residencyStandard2DBlockShape),
					ResidencyStandard2DMultisampleBlockShape = VkBool32.ConvertFrom(pCreateInfo.sparseProperties.residencyStandard2DMultisampleBlockShape),
					ResidencyStandard3DBlockShape = VkBool32.ConvertFrom(pCreateInfo.sparseProperties.residencyStandard3DBlockShape),
					ResidencyAlignedMipSize = VkBool32.ConvertFrom(pCreateInfo.sparseProperties.residencyAlignedMipSize),
					ResidencyNonResidentStrict = VkBool32.ConvertFrom(pCreateInfo.sparseProperties.residencyNonResidentStrict),
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
					pQueueFamilyProperties[i] = new MgQueueFamilyProperties
					{
						QueueFlags = (MgQueueFlagBits)familyProperties[i].queueFlags,
						QueueCount = familyProperties[i].queueCount,
						TimestampValidBits = familyProperties[i].timestampValidBits,
						MinImageTransferGranularity = familyProperties[i].minImageTransferGranularity,
					};
				}
			}
		}	

		public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			var memoryProperties = default(VkPhysicalDeviceMemoryProperties);
			Interops.vkGetPhysicalDeviceMemoryProperties(Handle, memoryProperties);

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
		}

		public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
		{
			var features = default(VkPhysicalDeviceFeatures);

			Interops.vkGetPhysicalDeviceFeatures(Handle, features);

			pFeatures = new MgPhysicalDeviceFeatures
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
			Interops.vkGetPhysicalDeviceFormatProperties(Handle, (VkFormat)format, formatProperties);

			pFormatProperties = new MgFormatProperties
			{
				Format = format,
				LinearTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.linearTilingFeatures,
				OptimalTilingFeatures = (MgFormatFeatureFlagBits)formatProperties.optimalTilingFeatures,
				BufferFeatures = (MgFormatFeatureFlagBits)formatProperties.bufferFeatures,
			};
		}

		public Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			var bFormat = (VkFormat)format;
			var bType = (VkImageType)type;
			var bUsage = (VkImageUsageFlags)usage;
			var bTiling = (VkImageTiling)tiling;
			var bFlags = (VkImageCreateFlags)flags;

			var properties = default(VkImageFormatProperties);
			var result = Interops.vkGetPhysicalDeviceImageFormatProperties
			(
				Handle,
				bFormat,
				bType,
				bTiling,
				bUsage,
				bFlags,
				properties
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

		public Result CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
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

				var enabledLayerCount = 0U;
				var ppEnabledLayerNames = GenerateEnabledLayerNames();

				var enabledExtensionCount = 0U;
				var ppEnabledExtensionNames = IntPtr.Zero;
				if (pCreateInfo.EnabledExtensionNames != null)
				{
					enabledExtensionCount = (uint)pCreateInfo.EnabledExtensionNames.Length;
					if (enabledExtensionCount > 0)
					{
						ppEnabledExtensionNames = GenerateExtensionNames(attachedItems, pCreateInfo.EnabledExtensionNames);
					}
				}
				var pEnabledFeatures = GenerateEnabledFeatures(attachedItems, pCreateInfo.EnabledFeatures);

				var internalHandle = IntPtr.Zero;
				var createInfo = new VkDeviceCreateInfo
				{
					sType = VkStructureType.StructureTypeDeviceCreateInfo,
					pNext = IntPtr.Zero,
					flags = pCreateInfo.Flags,
					queueCreateInfoCount = queueCreateInfoCount,
					pQueueCreateInfos = pQueueCreateInfos,
					enabledLayerCount = enabledLayerCount,
					ppEnabledLayerNames = ppEnabledLayerNames,
					enabledExtensionCount = enabledExtensionCount,
					ppEnabledExtensionNames = ppEnabledExtensionNames,
					pEnabledFeatures = pEnabledFeatures,
				};
				var result = Interops.vkCreateDevice(Handle, createInfo, allocatorPtr, ref internalHandle);
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

		IntPtr GenerateExtensionNames(List<IntPtr> attachedItems, string[] enabledExtensionNames)
		{
			if (enabledExtensionNames == null)
				return IntPtr.Zero;

			var stride = Marshal.SizeOf(typeof(IntPtr));
			var arrayLength = enabledExtensionNames.Length;
			var pEnabledExtensionNames = Marshal.AllocHGlobal(stride * arrayLength);
			attachedItems.Add(pEnabledExtensionNames);

			var pExtensions = new IntPtr[arrayLength];
			foreach (var name in enabledExtensionNames)
			{
				var handle = VkInteropsUtility.NativeUtf8FromString(name);
				attachedItems.Add(handle);
			}
			Marshal.Copy(pExtensions, 0, pEnabledExtensionNames, arrayLength);

			return pEnabledExtensionNames;
		}
  
		IntPtr GenerateEnabledLayerNames()
		{
			// https://www.khronos.org/registry/vulkan/specs/1.0-wsi_extensions/xhtml/vkspec.html#extended-functionality-device-layer-deprecation
			return IntPtr.Zero;
		}

		IntPtr GenerateQueueCreateInfos(List<IntPtr> attachedItems, MgDeviceQueueCreateInfo[] queueCreateInfos)
		{
			var pQueueCreateInfos = VkInteropsUtility.AllocateHGlobalArray(
				queueCreateInfos,
				(item) =>
				{
					var queueCount = item.QueueCount;

					Debug.Assert(item.QueuePriorities != null);
					int arrayLength = item.QueuePriorities.Length;
					Debug.Assert(item.QueueCount == arrayLength);

					var pQueuePriorities = Marshal.AllocHGlobal(sizeof(float) * arrayLength);
					attachedItems.Add(pQueuePriorities);
					Marshal.Copy(item.QueuePriorities, 0, pQueuePriorities, arrayLength);

					return new VkDeviceQueueCreateInfo
					{
						sType = VkStructureType.StructureTypeDeviceQueueCreateInfo,
						pNext = IntPtr.Zero,
						flags = item.Flags,
						queueFamilyIndex = item.QueueFamilyIndex,
						queueCount = item.QueueCount,
						pQueuePriorities = pQueuePriorities,
					};
				});
			attachedItems.Add(pQueueCreateInfos);

			return pQueueCreateInfos;
		}

		public Result EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
		{
			uint count = 0U;
			var first = Interops.vkEnumerateDeviceLayerProperties(Handle, ref count, null);

			if (first != Result.SUCCESS)
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
					LayerName = 
						Encoding.UTF8.GetString(
						layers[i].layerName,
						0,
						layers[i].layerName.Length),
					SpecVersion = layers[i].specVersion,
					ImplementationVersion = layers[i].implementationVersion,
					Description = 
						Encoding.UTF8.GetString(
						layers[i].description,
						0,
						layers[i].description.Length),
				};
			}

			return final;
		}

		public Result EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
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

				if (first != Result.SUCCESS)
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
							Encoding.UTF8.GetString(
							extensions[i].extensionName,
							0,
							extensions[i].extensionName.Length),
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

			var bFormat = (VkFormat) format;
			var bType = (VkImageType) type;
			var bSamples = (VkSampleCountFlags) samples;
			var bUsage = (VkImageUsageFlags) usage;
			var bTiling = (VkImageTiling) tiling;

			Interops.vkGetPhysicalDeviceSparseImageFormatProperties
			(
				Handle,
				bFormat,
				bType,
				bSamples,
				bUsage,
				bTiling,
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
				bFormat,
				bType,
				bSamples,
				bUsage,
				bTiling,
				ref count,
				formatProperties
		   	);

			pProperties = new MgSparseImageFormatProperties[count];
			for (var i = 0; i < count; ++i)
			{
				pProperties[i] = new MgSparseImageFormatProperties
				{
					AspectMask = (MgImageAspectFlagBits)formatProperties[i].aspectMask,
					ImageGranularity = formatProperties[i].imageGranularity,
					Flags = (MgSparseImageFormatFlagBits)formatProperties[i].flags,
				};
			}
		}

		public Result GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
		{
			uint count = 0;
			var first = Interops.vkGetPhysicalDeviceDisplayPropertiesKHR(Handle, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			uint count = 0;
			var first = Interops.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(Handle, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			uint count = 0;
			var first = Interops.vkGetDisplayPlaneSupportedDisplaysKHR(Handle, planeIndex, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			if (display == null)
				throw new ArgumentNullException(nameof(display));

			var bDisplay = (VkDisplayKHR)display;
			Debug.Assert(bDisplay != null); // MAYBE DUPLICATE CHECK
			uint count = 0;
			var first = Interops.vkGetDisplayModePropertiesKHR(Handle, bDisplay.Handle, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			if (mode == null)
				throw new ArgumentNullException(nameof(mode));

			var bMode = (VkDisplayModeKHR)mode;
			Debug.Assert(bMode != null);

			var capabilities = default(VkDisplayPlaneCapabilitiesKHR);
			var result = Interops.vkGetDisplayPlaneCapabilitiesKHR(Handle, bMode.Handle, planeIndex, capabilities);

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

		public Result GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
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

		public Result GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			var pCreateInfo = default(VkSurfaceCapabilitiesKHR);
			var result = Interops.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(Handle, bSurface.Handle, pCreateInfo);

			pSurfaceCapabilities = new MgSurfaceCapabilitiesKHR
			{
				MinImageCount = pCreateInfo.minImageCount,
				MaxImageCount = pCreateInfo.maxImageCount,
				CurrentExtent = pCreateInfo.currentExtent,
				MinImageExtent = pCreateInfo.minImageExtent,
				MaxImageExtent = pCreateInfo.maxImageExtent,
				MaxImageArrayLayers = pCreateInfo.maxImageArrayLayers,
				SupportedTransforms = (MgSurfaceTransformFlagBitsKHR) pCreateInfo.supportedTransforms,
				CurrentTransform = (MgSurfaceTransformFlagBitsKHR) pCreateInfo.currentTransform,
				SupportedCompositeAlpha = (MgCompositeAlphaFlagBitsKHR) pCreateInfo.supportedCompositeAlpha,
				SupportedUsageFlags = (MgImageUsageFlagBits) pCreateInfo.supportedUsageFlags,
			};

			return result;
		}

		public Result GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			var count = 0U;
			var first = Interops.vkGetPhysicalDeviceSurfaceFormatsKHR(Handle, bSurface.Handle, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			if (surface == null)
				throw new ArgumentNullException(nameof(surface));

			var bSurface = (VkSurfaceKHR)surface;
			Debug.Assert(bSurface != null);

			var count = 0U;
			var first = Interops.vkGetPhysicalDeviceSurfacePresentModesKHR(Handle, bSurface.Handle, ref count, null);

			if (first != Result.SUCCESS)
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

		public Result CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
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
			var result = Interops.vkCreateDisplayModeKHR(this.Handle, bDisplay.Handle, createInfo, allocatorPtr, ref modeHandle);
			pMode = new VkDisplayModeKHR(modeHandle);

			return result;
		}
	}
}
