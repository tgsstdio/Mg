using System;
using System.Collections.Generic;

namespace Magnesium.Vulkan.Functions.Device
{
    internal class VkSwapchainCreateUtility
    {
        public static VkSwapchainCreateInfoKHR GenerateSwapchainCreateInfoKHR(MgSwapchainCreateInfoKHR pCreateInfo, List<IntPtr> attachedItems)
        {
            var bSurface = (VkSurfaceKHR)pCreateInfo.Surface;
            var bSurfacePtr = bSurface != null ? bSurface.Handle : 0UL;

            var bOldSwapchain = (VkSwapchainKHR)pCreateInfo.OldSwapchain;
            var bOldSwapchainPtr = bOldSwapchain != null ? bOldSwapchain.Handle : 0UL;

            var pQueueFamilyIndices = IntPtr.Zero;
            var queueFamilyIndexCount = 0U;


            if (pCreateInfo.QueueFamilyIndices != null)
            {
                queueFamilyIndexCount = (uint)pCreateInfo.QueueFamilyIndices.Length;
                pQueueFamilyIndices = VkInteropsUtility.AllocateUInt32Array(pCreateInfo.QueueFamilyIndices);
                attachedItems.Add(pQueueFamilyIndices);
            }

            var createInfo = new VkSwapchainCreateInfoKHR
            {
                sType = VkStructureType.StructureTypeSwapchainCreateInfoKhr,
                pNext = IntPtr.Zero,
                flags = pCreateInfo.Flags,
                surface = bSurfacePtr,
                minImageCount = pCreateInfo.MinImageCount,
                imageFormat = pCreateInfo.ImageFormat,
                imageColorSpace = pCreateInfo.ImageColorSpace,
                imageExtent = pCreateInfo.ImageExtent,
                imageArrayLayers = pCreateInfo.ImageArrayLayers,
                imageUsage = pCreateInfo.ImageUsage,
                imageSharingMode = (VkSharingMode)pCreateInfo.ImageSharingMode,
                queueFamilyIndexCount = queueFamilyIndexCount,
                pQueueFamilyIndices = pQueueFamilyIndices,
                preTransform = (VkSurfaceTransformFlagsKhr)pCreateInfo.PreTransform,
                compositeAlpha = (VkCompositeAlphaFlagsKhr)pCreateInfo.CompositeAlpha,
                presentMode = (VkPresentModeKhr)pCreateInfo.PresentMode,
                clipped = VkBool32.ConvertTo(pCreateInfo.Clipped),
                oldSwapchain = bOldSwapchainPtr
            };
            return createInfo;

        }
    }
}
