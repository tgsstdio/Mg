using System;
using Magnesium.Vulkan.Functions.PhysicalDevice;

namespace Magnesium.Vulkan
{
    public class VkPhysicalDevice : IMgPhysicalDevice
	{
        internal readonly VkPhysicalDeviceInfo info;
        internal VkPhysicalDevice(IntPtr handle)
        {
            info = new VkPhysicalDeviceInfo(handle);
        }

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
        {
            VkGetPhysicalDevicePropertiesSection.GetPhysicalDeviceProperties(info, out pProperties);
        }

        public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
        {
            VkGetPhysicalDeviceQueueFamilyPropertiesSection.GetPhysicalDeviceQueueFamilyProperties(info, out pQueueFamilyProperties);
        }

        public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
        {
            VkGetPhysicalDeviceMemoryPropertiesSection.GetPhysicalDeviceMemoryProperties(info, out pMemoryProperties);
        }

        public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
        {
            VkGetPhysicalDeviceFeaturesSection.GetPhysicalDeviceFeatures(info, out pFeatures);
        }

        public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
        {
            VkGetPhysicalDeviceFormatPropertiesSection.GetPhysicalDeviceFormatProperties(info, format, out pFormatProperties);
        }

        public MgResult GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
        {
            return VkGetPhysicalDeviceImageFormatPropertiesSection.GetPhysicalDeviceImageFormatProperties(info, format, type, tiling, usage, flags, out pImageFormatProperties);
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
            return VkCreateDeviceSection.CreateDevice(info, pCreateInfo, allocator, out pDevice);
        }

        public MgResult EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
        {
            return VkEnumerateDeviceLayerPropertiesSection.EnumerateDeviceLayerProperties(info, out pProperties);
        }

        public MgResult EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
        {
            return VkEnumerateDeviceExtensionPropertiesSection.EnumerateDeviceExtensionProperties(info, layerName, out pProperties);
        }

        public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
        {
            VkGetPhysicalDeviceSparseImageFormatPropertiesSection.GetPhysicalDeviceSparseImageFormatProperties(info, format, type, samples, usage, tiling, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPropertiesKHRSection.GetPhysicalDeviceDisplayPropertiesKHR(info, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPlanePropertiesKHRSection.GetPhysicalDeviceDisplayPlanePropertiesKHR(info, out pProperties);
        }

        public MgResult GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
        {
            return VkGetDisplayPlaneSupportedDisplaysKHRSection.GetDisplayPlaneSupportedDisplaysKHR(info, planeIndex, out pDisplays);
        }

        public MgResult GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
        {
            return VkGetDisplayModePropertiesKHRSection.GetDisplayModePropertiesKHR(info, display, out pProperties);
        }

        public MgResult GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
        {
            return VkGetDisplayPlaneCapabilitiesKHRSection.GetDisplayPlaneCapabilitiesKHR(info, mode, planeIndex, out pCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
        {
            return VkGetPhysicalDeviceSurfaceSupportKHRSection.GetPhysicalDeviceSurfaceSupportKHR(info, queueFamilyIndex, surface, ref pSupported);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilitiesKHRSection.GetPhysicalDeviceSurfaceCapabilitiesKHR(info, surface, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
        {
            return VkGetPhysicalDeviceSurfaceFormatsKHRSection.GetPhysicalDeviceSurfaceFormatsKHR(info, surface, out pSurfaceFormats);
        }

        public MgResult GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
        {
            return VkGetPhysicalDeviceSurfacePresentModesKHRSection.GetPhysicalDeviceSurfacePresentModesKHR(info, surface, out pPresentModes);
        }

        public bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex)
        {
            // TODO: platform specific - maybe remove
            return VkGetPhysicalDeviceWin32PresentationSupportKHRSection.GetPhysicalDeviceWin32PresentationSupportKHR(info, queueFamilyIndex);
        }

        public MgResult CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
        {
            return VkCreateDisplayModeKHRSection.CreateDisplayModeKHR(info, display, pCreateInfo, allocator, out pMode);
        }

        public MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            return VkGetPhysicalDeviceExternalImageFormatPropertiesNVSection.GetPhysicalDeviceExternalImageFormatPropertiesNV(info, format, type, tiling, usage, flags, externalHandleType, out pExternalImageFormatProperties);
        }

        public MgResult GetPhysicalDevicePresentRectanglesKHR(IMgSurfaceKHR surface, MgRect2D[] pRects)
        {
            return VkGetPhysicalDevicePresentRectanglesKHRSection.GetPhysicalDevicePresentRectanglesKHR(info, surface, pRects);
        }

        // TODO: BELOW EXTENSION methods via pNext linked

        public MgResult GetDisplayModeProperties2KHR(IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
        {
            return VkGetDisplayModeProperties2KHRSection.GetDisplayModeProperties2KHR(info, display, out pProperties);
        }

        public MgResult GetDisplayPlaneCapabilities2KHR(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
        {
            return VkGetDisplayPlaneCapabilities2KHRSection.GetDisplayPlaneCapabilities2KHR(info, pDisplayPlaneInfo, out pCapabilities);
        }

        public MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(out MgTimeDomainEXT[] pTimeDomains)
        {
            return VkGetPhysicalDeviceCalibrateableTimeDomainsEXTSection.GetPhysicalDeviceCalibrateableTimeDomainsEXT(info, out pTimeDomains);
        }

        public MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(out MgDisplayPlaneProperties2KHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPlaneProperties2KHRSection.GetPhysicalDeviceDisplayPlaneProperties2KHR(info, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayProperties2KHR(out MgDisplayProperties2KHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayProperties2KHRSection.GetPhysicalDeviceDisplayProperties2KHR(info, out pProperties);
        }

        public MgResult GetPhysicalDeviceImageFormatProperties2(MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
        {
            return VkGetPhysicalDeviceImageFormatProperties2Section.GetPhysicalDeviceImageFormatProperties2(info, pImageFormatInfo, pImageFormatProperties);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilities2EXTSection.GetPhysicalDeviceSurfaceCapabilities2EXT(info, surface, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilities2KHRSection.GetPhysicalDeviceSurfaceCapabilities2KHR(info, pSurfaceInfo, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceFormats2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
        {
            return VkGetPhysicalDeviceSurfaceFormats2KHRSection.GetPhysicalDeviceSurfaceFormats2KHR(info, pSurfaceInfo, out pSurfaceFormats);
        }

        public MgResult ReleaseDisplayEXT(IMgDisplayKHR display)
        {
            return VkReleaseDisplayEXTSection.ReleaseDisplayEXT(info, display);
        }

        public void GetPhysicalDeviceExternalBufferProperties(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
        {
            VkGetPhysicalDeviceExternalBufferPropertiesSection.GetPhysicalDeviceExternalBufferProperties(info, pExternalBufferInfo, out pExternalBufferProperties);
        }

        public void GetPhysicalDeviceExternalFenceProperties(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
        {
            VkGetPhysicalDeviceExternalFencePropertiesSection.GetPhysicalDeviceExternalFenceProperties(info, pExternalFenceInfo, out pExternalFenceProperties);
        }

        public void GetPhysicalDeviceExternalSemaphoreProperties(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
        {
            VkGetPhysicalDeviceExternalSemaphorePropertiesSection.GetPhysicalDeviceExternalSemaphoreProperties(info, pExternalSemaphoreInfo, out pExternalSemaphoreProperties);
        }

        public void GetPhysicalDeviceFeatures2(out MgPhysicalDeviceFeatures2 pFeatures)
        {
            VkGetPhysicalDeviceFeatures2Section.GetPhysicalDeviceFeatures2(info, out pFeatures);
        }

        public void GetPhysicalDeviceFormatProperties2(MgFormat format, out MgFormatProperties2 pFormatProperties)
        {
            VkGetPhysicalDeviceFormatProperties2Section.GetPhysicalDeviceFormatProperties2(info, format, out pFormatProperties);
        }

        public void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            VkGetPhysicalDeviceGeneratedCommandsPropertiesNVXSection.GetPhysicalDeviceGeneratedCommandsPropertiesNVX(info, pFeatures, out pLimits);
        }

        public void GetPhysicalDeviceMemoryProperties2(out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
        {
            VkGetPhysicalDeviceMemoryProperties2Section.GetPhysicalDeviceMemoryProperties2(info, out pMemoryProperties);
        }

        public void GetPhysicalDeviceMultisamplePropertiesEXT(MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
        {
            VkGetPhysicalDeviceMultisamplePropertiesEXTSection.GetPhysicalDeviceMultisamplePropertiesEXT(info, samples, pMultisampleProperties);
        }

        public void GetPhysicalDeviceProperties2(out MgPhysicalDeviceProperties2 pProperties)
        {
            VkGetPhysicalDeviceProperties2Section.GetPhysicalDeviceProperties2(info, out pProperties);
        }

        public void GetPhysicalDeviceQueueFamilyProperties2(out MgQueueFamilyProperties2[] pQueueFamilyProperties)
        {
            VkGetPhysicalDeviceQueueFamilyProperties2Section.GetPhysicalDeviceQueueFamilyProperties2(info, out pQueueFamilyProperties);
        }

        public void GetPhysicalDeviceSparseImageFormatProperties2(MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
        {
            VkGetPhysicalDeviceSparseImageFormatProperties2Section.GetPhysicalDeviceSparseImageFormatProperties2(info, pFormatInfo, out pProperties);
        }
    }
}
