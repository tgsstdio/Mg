using System;
using Magnesium.Vulkan.Functions.PhysicalDevice;

namespace Magnesium.Vulkan
{
    public class VkPhysicalDevice : IMgPhysicalDevice
	{
        internal VkPhysicalDeviceInfo Info { get; }
        internal VkPhysicalDevice(IntPtr handle)
        {
            Info = new VkPhysicalDeviceInfo(handle);
        }

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
        {
            VkGetPhysicalDevicePropertiesSection.GetPhysicalDeviceProperties(Info, out pProperties);
        }

        public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
        {
            VkGetPhysicalDeviceQueueFamilyPropertiesSection.GetPhysicalDeviceQueueFamilyProperties(Info, out pQueueFamilyProperties);
        }

        public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
        {
            VkGetPhysicalDeviceMemoryPropertiesSection.GetPhysicalDeviceMemoryProperties(Info, out pMemoryProperties);
        }

        public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
        {
            VkGetPhysicalDeviceFeaturesSection.GetPhysicalDeviceFeatures(Info, out pFeatures);
        }

        public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
        {
            VkGetPhysicalDeviceFormatPropertiesSection.GetPhysicalDeviceFormatProperties(Info, format, out pFormatProperties);
        }

        public MgResult GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
        {
            return VkGetPhysicalDeviceImageFormatPropertiesSection.GetPhysicalDeviceImageFormatProperties(Info, format, type, tiling, usage, flags, out pImageFormatProperties);
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
            return VkCreateDeviceSection.CreateDevice(Info, pCreateInfo, allocator, out pDevice);
        }

        public MgResult EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
        {
            return VkEnumerateDeviceLayerPropertiesSection.EnumerateDeviceLayerProperties(Info, out pProperties);
        }

        public MgResult EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
        {
            return VkEnumerateDeviceExtensionPropertiesSection.EnumerateDeviceExtensionProperties(Info, layerName, out pProperties);
        }

        public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
        {
            VkGetPhysicalDeviceSparseImageFormatPropertiesSection.GetPhysicalDeviceSparseImageFormatProperties(Info, format, type, samples, usage, tiling, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPropertiesKHRSection.GetPhysicalDeviceDisplayPropertiesKHR(Info, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPlanePropertiesKHRSection.GetPhysicalDeviceDisplayPlanePropertiesKHR(Info, out pProperties);
        }

        public MgResult GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays)
        {
            return VkGetDisplayPlaneSupportedDisplaysKHRSection.GetDisplayPlaneSupportedDisplaysKHR(Info, planeIndex, out pDisplays);
        }

        public MgResult GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
        {
            return VkGetDisplayModePropertiesKHRSection.GetDisplayModePropertiesKHR(Info, display, out pProperties);
        }

        public MgResult GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
        {
            return VkGetDisplayPlaneCapabilitiesKHRSection.GetDisplayPlaneCapabilitiesKHR(Info, mode, planeIndex, out pCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
        {
            return VkGetPhysicalDeviceSurfaceSupportKHRSection.GetPhysicalDeviceSurfaceSupportKHR(Info, queueFamilyIndex, surface, ref pSupported);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilitiesKHRSection.GetPhysicalDeviceSurfaceCapabilitiesKHR(Info, surface, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
        {
            return VkGetPhysicalDeviceSurfaceFormatsKHRSection.GetPhysicalDeviceSurfaceFormatsKHR(Info, surface, out pSurfaceFormats);
        }

        public MgResult GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
        {
            return VkGetPhysicalDeviceSurfacePresentModesKHRSection.GetPhysicalDeviceSurfacePresentModesKHR(Info, surface, out pPresentModes);
        }

        public bool GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex)
        {
            // TODO: platform specific - maybe remove
            return VkGetPhysicalDeviceWin32PresentationSupportKHRSection.GetPhysicalDeviceWin32PresentationSupportKHR(Info, queueFamilyIndex);
        }

        public MgResult CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
        {
            return VkCreateDisplayModeKHRSection.CreateDisplayModeKHR(Info, display, pCreateInfo, allocator, out pMode);
        }

        public MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            return VkGetPhysicalDeviceExternalImageFormatPropertiesNVSection.GetPhysicalDeviceExternalImageFormatPropertiesNV(Info, format, type, tiling, usage, flags, externalHandleType, out pExternalImageFormatProperties);
        }

        public MgResult GetPhysicalDevicePresentRectanglesKHR(IMgSurfaceKHR surface, MgRect2D[] pRects)
        {
            return VkGetPhysicalDevicePresentRectanglesKHRSection.GetPhysicalDevicePresentRectanglesKHR(Info, surface, pRects);
        }

        // TODO: BELOW EXTENSION methods via pNext linked

        public MgResult GetDisplayModeProperties2KHR(IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
        {
            return VkGetDisplayModeProperties2KHRSection.GetDisplayModeProperties2KHR(Info, display, out pProperties);
        }

        public MgResult GetDisplayPlaneCapabilities2KHR(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
        {
            return VkGetDisplayPlaneCapabilities2KHRSection.GetDisplayPlaneCapabilities2KHR(Info, pDisplayPlaneInfo, out pCapabilities);
        }

        public MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(out MgTimeDomainEXT[] pTimeDomains)
        {
            return VkGetPhysicalDeviceCalibrateableTimeDomainsEXTSection.GetPhysicalDeviceCalibrateableTimeDomainsEXT(Info, out pTimeDomains);
        }

        public MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(out MgDisplayPlaneProperties2KHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayPlaneProperties2KHRSection.GetPhysicalDeviceDisplayPlaneProperties2KHR(Info, out pProperties);
        }

        public MgResult GetPhysicalDeviceDisplayProperties2KHR(out MgDisplayProperties2KHR[] pProperties)
        {
            return VkGetPhysicalDeviceDisplayProperties2KHRSection.GetPhysicalDeviceDisplayProperties2KHR(Info, out pProperties);
        }

        public MgResult GetPhysicalDeviceImageFormatProperties2(MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
        {
            return VkGetPhysicalDeviceImageFormatProperties2Section.GetPhysicalDeviceImageFormatProperties2(Info, pImageFormatInfo, pImageFormatProperties);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilities2EXTSection.GetPhysicalDeviceSurfaceCapabilities2EXT(Info, surface, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            return VkGetPhysicalDeviceSurfaceCapabilities2KHRSection.GetPhysicalDeviceSurfaceCapabilities2KHR(Info, pSurfaceInfo, out pSurfaceCapabilities);
        }

        public MgResult GetPhysicalDeviceSurfaceFormats2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
        {
            return VkGetPhysicalDeviceSurfaceFormats2KHRSection.GetPhysicalDeviceSurfaceFormats2KHR(Info, pSurfaceInfo, out pSurfaceFormats);
        }

        public MgResult ReleaseDisplayEXT(IMgDisplayKHR display)
        {
            return VkReleaseDisplayEXTSection.ReleaseDisplayEXT(Info, display);
        }

        public void GetPhysicalDeviceExternalBufferProperties(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
        {
            VkGetPhysicalDeviceExternalBufferPropertiesSection.GetPhysicalDeviceExternalBufferProperties(Info, pExternalBufferInfo, out pExternalBufferProperties);
        }

        public void GetPhysicalDeviceExternalFenceProperties(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
        {
            VkGetPhysicalDeviceExternalFencePropertiesSection.GetPhysicalDeviceExternalFenceProperties(Info, pExternalFenceInfo, out pExternalFenceProperties);
        }

        public void GetPhysicalDeviceExternalSemaphoreProperties(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
        {
            VkGetPhysicalDeviceExternalSemaphorePropertiesSection.GetPhysicalDeviceExternalSemaphoreProperties(Info, pExternalSemaphoreInfo, out pExternalSemaphoreProperties);
        }

        public void GetPhysicalDeviceFeatures2(out MgPhysicalDeviceFeatures2 pFeatures)
        {
            VkGetPhysicalDeviceFeatures2Section.GetPhysicalDeviceFeatures2(Info, out pFeatures);
        }

        public void GetPhysicalDeviceFormatProperties2(MgFormat format, out MgFormatProperties2 pFormatProperties)
        {
            VkGetPhysicalDeviceFormatProperties2Section.GetPhysicalDeviceFormatProperties2(Info, format, out pFormatProperties);
        }

        public void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            VkGetPhysicalDeviceGeneratedCommandsPropertiesNVXSection.GetPhysicalDeviceGeneratedCommandsPropertiesNVX(Info, pFeatures, out pLimits);
        }

        public void GetPhysicalDeviceMemoryProperties2(out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
        {
            VkGetPhysicalDeviceMemoryProperties2Section.GetPhysicalDeviceMemoryProperties2(Info, out pMemoryProperties);
        }

        public void GetPhysicalDeviceMultisamplePropertiesEXT(MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
        {
            VkGetPhysicalDeviceMultisamplePropertiesEXTSection.GetPhysicalDeviceMultisamplePropertiesEXT(Info, samples, pMultisampleProperties);
        }

        public void GetPhysicalDeviceProperties2(out MgPhysicalDeviceProperties2 pProperties)
        {
            VkGetPhysicalDeviceProperties2Section.GetPhysicalDeviceProperties2(Info, out pProperties);
        }

        public void GetPhysicalDeviceQueueFamilyProperties2(out MgQueueFamilyProperties2[] pQueueFamilyProperties)
        {
            VkGetPhysicalDeviceQueueFamilyProperties2Section.GetPhysicalDeviceQueueFamilyProperties2(Info, out pQueueFamilyProperties);
        }

        public void GetPhysicalDeviceSparseImageFormatProperties2(MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
        {
            VkGetPhysicalDeviceSparseImageFormatProperties2Section.GetPhysicalDeviceSparseImageFormatProperties2(Info, pFormatInfo, out pProperties);
        }
    }
}
