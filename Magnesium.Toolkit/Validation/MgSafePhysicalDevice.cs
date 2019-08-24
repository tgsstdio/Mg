using System;
namespace Magnesium.Toolkit
{
	public class MgSafePhysicalDevice : IMgPhysicalDevice
	{
		internal IMgPhysicalDevice mImpl = null;
		internal MgSafePhysicalDevice(IMgPhysicalDevice impl)
		{
			mImpl = impl;
		}

		public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties) {
			mImpl.GetPhysicalDeviceProperties(out pProperties);
		}

		public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties) {
			mImpl.GetPhysicalDeviceQueueFamilyProperties(out pQueueFamilyProperties);
		}

		public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties) {
			mImpl.GetPhysicalDeviceMemoryProperties(out pMemoryProperties);
		}

		public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures) {
			mImpl.GetPhysicalDeviceFeatures(out pFeatures);
		}

		public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceFormatProperties.Validate(format);
			mImpl.GetPhysicalDeviceFormatProperties(format, out pFormatProperties);
		}

		public MgResult GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceImageFormatProperties.Validate(format, type, tiling, usage, flags);
			return mImpl.GetPhysicalDeviceImageFormatProperties(format, type, tiling, usage, flags, out pImageFormatProperties);
		}

		public MgResult CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice) {
			Validation.PhysicalDevice.CreateDevice.Validate(pCreateInfo, allocator);

			var result = mImpl.CreateDevice(pCreateInfo, allocator, out IMgDevice tempDevice);

            if (result != MgResult.SUCCESS)
            {
                pDevice = null;
                return result;
            }

            pDevice = new MgSafeDevice(tempDevice);
            return result;
		}

		public MgResult EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties) {
			return mImpl.EnumerateDeviceLayerProperties(out pProperties);
		}

		public MgResult EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties) {
			Validation.PhysicalDevice.EnumerateDeviceExtensionProperties.Validate(layerName);
			return mImpl.EnumerateDeviceExtensionProperties(layerName, out pProperties);
		}

		public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceSparseImageFormatProperties.Validate(format, type, samples, usage, tiling);
			mImpl.GetPhysicalDeviceSparseImageFormatProperties(format, type, samples, usage, tiling, out pProperties);
		}

		public MgResult GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties) {
			return mImpl.GetPhysicalDeviceDisplayPropertiesKHR(out pProperties);
		}

		public MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties) {
			return mImpl.GetPhysicalDeviceDisplayPlanePropertiesKHR(out pProperties);
		}

		public MgResult GetDisplayPlaneSupportedDisplaysKHR(UInt32 planeIndex, out IMgDisplayKHR[] pDisplays) {
			Validation.PhysicalDevice.GetDisplayPlaneSupportedDisplaysKHR.Validate(planeIndex);
			return mImpl.GetDisplayPlaneSupportedDisplaysKHR(planeIndex, out pDisplays);
		}

		public MgResult GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties) {
			Validation.PhysicalDevice.GetDisplayModePropertiesKHR.Validate(display);
			return mImpl.GetDisplayModePropertiesKHR(display, out pProperties);
		}

		public MgResult CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode) {
			Validation.PhysicalDevice.CreateDisplayModeKHR.Validate(display, pCreateInfo, allocator);
			return mImpl.CreateDisplayModeKHR(display, pCreateInfo, allocator, out pMode);
		}

		public MgResult GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, UInt32 planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities) {
			Validation.PhysicalDevice.GetDisplayPlaneCapabilitiesKHR.Validate(mode, planeIndex);
			return mImpl.GetDisplayPlaneCapabilitiesKHR(mode, planeIndex, out pCapabilities);
		}

		public MgResult GetPhysicalDeviceSurfaceSupportKHR(UInt32 queueFamilyIndex, IMgSurfaceKHR surface, ref Boolean pSupported) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceSupportKHR.Validate(queueFamilyIndex, surface, ref pSupported);
			return mImpl.GetPhysicalDeviceSurfaceSupportKHR(queueFamilyIndex, surface, ref pSupported);
		}

		public MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceCapabilitiesKHR.Validate(surface);
			return mImpl.GetPhysicalDeviceSurfaceCapabilitiesKHR(surface, out pSurfaceCapabilities);
		}

		public MgResult GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceFormatsKHR.Validate(surface);
			return mImpl.GetPhysicalDeviceSurfaceFormatsKHR(surface, out pSurfaceFormats);
		}

		public MgResult GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfacePresentModesKHR.Validate(surface);
			return mImpl.GetPhysicalDeviceSurfacePresentModesKHR(surface, out pPresentModes);
		}

		public Boolean GetPhysicalDeviceWin32PresentationSupportKHR(UInt32 queueFamilyIndex) {
			Validation.PhysicalDevice.GetPhysicalDeviceWin32PresentationSupportKHR.Validate(queueFamilyIndex);
			return mImpl.GetPhysicalDeviceWin32PresentationSupportKHR(queueFamilyIndex);
		}

		public MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, UInt32 externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceExternalImageFormatPropertiesNV.Validate(format, type, tiling, usage, flags, externalHandleType);
			return mImpl.GetPhysicalDeviceExternalImageFormatPropertiesNV(format, type, tiling, usage, flags, externalHandleType, out pExternalImageFormatProperties);
		}

		public MgResult GetPhysicalDevicePresentRectanglesKHR(IMgSurfaceKHR surface, MgRect2D[] pRects) {
			Validation.PhysicalDevice.GetPhysicalDevicePresentRectanglesKHR.Validate(surface, pRects);
			return mImpl.GetPhysicalDevicePresentRectanglesKHR(surface, pRects);
		}

		public MgResult GetDisplayModeProperties2KHR(IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties) {
			Validation.PhysicalDevice.GetDisplayModeProperties2KHR.Validate(display);
			return mImpl.GetDisplayModeProperties2KHR(display, out pProperties);
		}

		public MgResult GetDisplayPlaneCapabilities2KHR(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities) {
			Validation.PhysicalDevice.GetDisplayPlaneCapabilities2KHR.Validate(pDisplayPlaneInfo);
			return mImpl.GetDisplayPlaneCapabilities2KHR(pDisplayPlaneInfo, out pCapabilities);
		}

		public MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(out MgTimeDomainEXT[] pTimeDomains) {
			return mImpl.GetPhysicalDeviceCalibrateableTimeDomainsEXT(out pTimeDomains);
		}

		public MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(out MgDisplayPlaneProperties2KHR[] pProperties) {
			return mImpl.GetPhysicalDeviceDisplayPlaneProperties2KHR(out pProperties);
		}

		public MgResult GetPhysicalDeviceDisplayProperties2KHR(out MgDisplayProperties2KHR[] pProperties) {
			return mImpl.GetPhysicalDeviceDisplayProperties2KHR(out pProperties);
		}

		public MgResult GetPhysicalDeviceImageFormatProperties2(MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceImageFormatProperties2.Validate(pImageFormatInfo, pImageFormatProperties);
			return mImpl.GetPhysicalDeviceImageFormatProperties2(pImageFormatInfo, pImageFormatProperties);
		}

		public MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceCapabilities2EXT.Validate(surface);
			return mImpl.GetPhysicalDeviceSurfaceCapabilities2EXT(surface, out pSurfaceCapabilities);
		}

		public MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceCapabilities2KHR.Validate(pSurfaceInfo);
			return mImpl.GetPhysicalDeviceSurfaceCapabilities2KHR(pSurfaceInfo, out pSurfaceCapabilities);
		}

		public MgResult GetPhysicalDeviceSurfaceFormats2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats) {
			Validation.PhysicalDevice.GetPhysicalDeviceSurfaceFormats2KHR.Validate(pSurfaceInfo);
			return mImpl.GetPhysicalDeviceSurfaceFormats2KHR(pSurfaceInfo, out pSurfaceFormats);
		}

		public MgResult ReleaseDisplayEXT(IMgDisplayKHR display) {
			Validation.PhysicalDevice.ReleaseDisplayEXT.Validate(display);
			return mImpl.ReleaseDisplayEXT(display);
		}

		public void GetPhysicalDeviceExternalBufferProperties(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceExternalBufferProperties.Validate(pExternalBufferInfo);
			mImpl.GetPhysicalDeviceExternalBufferProperties(pExternalBufferInfo, out pExternalBufferProperties);
		}

		public void GetPhysicalDeviceExternalFenceProperties(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceExternalFenceProperties.Validate(pExternalFenceInfo);
			mImpl.GetPhysicalDeviceExternalFenceProperties(pExternalFenceInfo, out pExternalFenceProperties);
		}

		public void GetPhysicalDeviceExternalSemaphoreProperties(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceExternalSemaphoreProperties.Validate(pExternalSemaphoreInfo);
			mImpl.GetPhysicalDeviceExternalSemaphoreProperties(pExternalSemaphoreInfo, out pExternalSemaphoreProperties);
		}

		public void GetPhysicalDeviceFeatures2(out MgPhysicalDeviceFeatures2 pFeatures) {
			mImpl.GetPhysicalDeviceFeatures2(out pFeatures);
		}

		public void GetPhysicalDeviceFormatProperties2(MgFormat format, out MgFormatProperties2 pFormatProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceFormatProperties2.Validate(format);
			mImpl.GetPhysicalDeviceFormatProperties2(format, out pFormatProperties);
		}

		public void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits) {
			Validation.PhysicalDevice.GetPhysicalDeviceGeneratedCommandsPropertiesNVX.Validate(pFeatures);
			mImpl.GetPhysicalDeviceGeneratedCommandsPropertiesNVX(pFeatures, out pLimits);
		}

		public void GetPhysicalDeviceMemoryProperties2(out MgPhysicalDeviceMemoryProperties2 pMemoryProperties) {
			mImpl.GetPhysicalDeviceMemoryProperties2(out pMemoryProperties);
		}

		public void GetPhysicalDeviceMultisamplePropertiesEXT(MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceMultisamplePropertiesEXT.Validate(samples, pMultisampleProperties);
			mImpl.GetPhysicalDeviceMultisamplePropertiesEXT(samples, pMultisampleProperties);
		}

		public void GetPhysicalDeviceProperties2(out MgPhysicalDeviceProperties2 pProperties) {
			mImpl.GetPhysicalDeviceProperties2(out pProperties);
		}

		public void GetPhysicalDeviceQueueFamilyProperties2(out MgQueueFamilyProperties2[] pQueueFamilyProperties) {
			mImpl.GetPhysicalDeviceQueueFamilyProperties2(out pQueueFamilyProperties);
		}

		public void GetPhysicalDeviceSparseImageFormatProperties2(MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties) {
			Validation.PhysicalDevice.GetPhysicalDeviceSparseImageFormatProperties2.Validate(pFormatInfo);
			mImpl.GetPhysicalDeviceSparseImageFormatProperties2(pFormatInfo, out pProperties);
		}

	}
}
