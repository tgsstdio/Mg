using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateSamplerSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateSampler(IntPtr device, ref VkSamplerCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pSampler);

        public static MgResult CreateSampler(VkDeviceInfo info, MgSamplerCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgSampler pSampler)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var internalHandle = 0UL;
            var createInfo = new VkSamplerCreateInfo
            {
                sType = VkStructureType.StructureTypeSamplerCreateInfo,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                magFilter = (VkFilter)pCreateInfo.MagFilter,
                minFilter = (VkFilter)pCreateInfo.MinFilter,
                mipmapMode = (VkSamplerMipmapMode)pCreateInfo.MipmapMode,
                addressModeU = (VkSamplerAddressMode)pCreateInfo.AddressModeU,
                addressModeV = (VkSamplerAddressMode)pCreateInfo.AddressModeV,
                addressModeW = (VkSamplerAddressMode)pCreateInfo.AddressModeW,
                mipLodBias = pCreateInfo.MipLodBias,
                anisotropyEnable = VkBool32.ConvertTo(pCreateInfo.AnisotropyEnable),
                maxAnisotropy = pCreateInfo.MaxAnisotropy,
                compareEnable = VkBool32.ConvertTo(pCreateInfo.CompareEnable),
                compareOp = (VkCompareOp)pCreateInfo.CompareOp,
                minLod = pCreateInfo.MinLod,
                maxLod = pCreateInfo.MaxLod,
                borderColor = (VkBorderColor)pCreateInfo.BorderColor,
                unnormalizedCoordinates = VkBool32.ConvertTo(pCreateInfo.UnnormalizedCoordinates),
            };

            var result = vkCreateSampler(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
            pSampler = new VkSampler(internalHandle);
            return result;
        }
    }
}
