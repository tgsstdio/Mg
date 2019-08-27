using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.PhysicalDevice
{
	public static class VkGetPhysicalDeviceFeaturesSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkGetPhysicalDeviceFeatures(IntPtr physicalDevice, ref VkPhysicalDeviceFeatures pFeatures);

        public static void GetPhysicalDeviceFeatures(VkPhysicalDeviceInfo info, out MgPhysicalDeviceFeatures pFeatures)
        {
            var features = default(VkPhysicalDeviceFeatures);

            vkGetPhysicalDeviceFeatures(info.Handle, ref features);

            pFeatures = TranslateFeatures(ref features);
        }

        internal static MgPhysicalDeviceFeatures TranslateFeatures(ref VkPhysicalDeviceFeatures features)
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
    }
}
