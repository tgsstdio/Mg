using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public class VkCreateDeviceSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDevice(IntPtr physicalDevice, ref VkDeviceCreateInfo pCreateInfo, IntPtr pAllocator, ref IntPtr pDevice);

        public static MgResult CreateDevice(VkPhysicalDeviceInfo info, MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
        {
            var allocatorPtr = VkInteropsUtility.GetAllocatorHandle(allocator);

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
                var result = vkCreateDevice(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
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

        static IntPtr GenerateEnabledFeatures(List<IntPtr> attachedItems, MgPhysicalDeviceFeatures enabledFeatures)
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

        static IntPtr GenerateQueueCreateInfos(List<IntPtr> attachedItems, MgDeviceQueueCreateInfo[] queueCreateInfos)
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
    }
}
