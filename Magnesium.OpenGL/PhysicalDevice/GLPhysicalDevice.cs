using System;

namespace Magnesium.OpenGL.Internals
{
	public class GLPhysicalDevice : IMgPhysicalDevice
	{
		private readonly GLDevice mDevice;
		public GLPhysicalDevice (IGLQueue queue, IGLDeviceEntrypoint entrypoint)
		{
			mDevice = new GLDevice (queue, entrypoint);
		}

		#region IMgPhysicalDevice implementation
		public void GetPhysicalDeviceProperties (out MgPhysicalDeviceProperties pProperties)
		{
			pProperties = new MgPhysicalDeviceProperties
            {
                Limits = new MgPhysicalDeviceLimits
                {
                    // OPENGL : only one descriptor sets can be bound
                    MaxBoundDescriptorSets = 1,
                },
            };
		}
		public void GetPhysicalDeviceQueueFamilyProperties (out MgQueueFamilyProperties[] pQueueFamilyProperties)
		{
			// ONE QUEUE FOR ALL
			pQueueFamilyProperties = new [] {
				new MgQueueFamilyProperties {					
					QueueFlags = MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT,
				}
			};
		}
		public void GetPhysicalDeviceMemoryProperties (out MgPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			// TODO : overwrite here to shift memory based on which type
			// 0 : buffer based 
			// 1 : host defined (for INDIRECT)
			pMemoryProperties = new MgPhysicalDeviceMemoryProperties();
			var slots = new MgMemoryType[8];

			const uint allOn = (uint)(
			                       MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT |
			                       MgMemoryPropertyFlagBits.HOST_CACHED_BIT |
			                       MgMemoryPropertyFlagBits.HOST_COHERENT_BIT |
			                       MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT |
			                       MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT);

			// THE NUMBER OF SLOTS DETERMINE THE DIFFERENT BUFFER TYPES = no of GLMemoryBufferType enums
			slots [0] = new MgMemoryType{ PropertyFlags = allOn };
			slots [1] = new MgMemoryType{ PropertyFlags = allOn };
			slots [2] = new MgMemoryType{ PropertyFlags = allOn };
			slots [3] = new MgMemoryType{ PropertyFlags = allOn };
			slots [4] = new MgMemoryType{ PropertyFlags = allOn };
            slots [5] = new MgMemoryType { PropertyFlags = allOn };
            slots [6] = new MgMemoryType { PropertyFlags = allOn };
            slots [7] = new MgMemoryType { PropertyFlags = allOn };

            pMemoryProperties.MemoryTypes = slots;
		}
		public void GetPhysicalDeviceFeatures (out MgPhysicalDeviceFeatures pFeatures)
		{
			pFeatures = new MgPhysicalDeviceFeatures ();
		}
		public void GetPhysicalDeviceFormatProperties (MgFormat format, out MgFormatProperties pFormatProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceImageFormatProperties (MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult CreateDevice (MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
		{
			// USING SINGLE DEVICE & SINGLE QUEUE 
				// SHOULD BE 
			pDevice = mDevice;
			return MgResult.SUCCESS;
		}
		public MgResult EnumerateDeviceLayerProperties (out MgLayerProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult EnumerateDeviceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
            pProperties = new MgExtensionProperties[] { };
            return MgResult.SUCCESS;
		}
		public void GetPhysicalDeviceSparseImageFormatProperties (MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceDisplayPropertiesKHR (out MgDisplayPropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceDisplayPlanePropertiesKHR (out MgDisplayPlanePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetDisplayPlaneSupportedDisplaysKHR (uint planeIndex, out IMgDisplayKHR[] pDisplays)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetDisplayModePropertiesKHR (IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetDisplayPlaneCapabilitiesKHR (IMgDisplayModeKHR mode, uint planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceSurfaceSupportKHR (uint queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceSurfaceCapabilitiesKHR (IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceSurfaceFormatsKHR (IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
		{
			throw new NotImplementedException ();
		}
		public MgResult GetPhysicalDeviceSurfacePresentModesKHR (IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
		{
			throw new NotImplementedException ();
		}
		public bool GetPhysicalDeviceWin32PresentationSupportKHR (uint queueFamilyIndex)
		{
			throw new NotImplementedException ();
		}

		public MgResult CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
		{
			throw new NotImplementedException();
		}

        public MgResult GetPhysicalDeviceExternalImageFormatPropertiesNV(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, uint externalHandleType, out MgExternalImageFormatPropertiesNV pExternalImageFormatProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDevicePresentRectanglesKHR(IMgSurfaceKHR surface, MgRect2D[] pRects)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDisplayModeProperties2KHR(IMgDisplayKHR display, out MgDisplayModeProperties2KHR[] pProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetDisplayPlaneCapabilities2KHR(MgDisplayPlaneInfo2KHR pDisplayPlaneInfo, out MgDisplayPlaneCapabilities2KHR pCapabilities)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceCalibrateableTimeDomainsEXT(out MgTimeDomainEXT[] pTimeDomains)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceDisplayPlaneProperties2KHR(out MgDisplayPlaneProperties2KHR[] pProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceDisplayProperties2KHR(out MgDisplayProperties2KHR[] pProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceImageFormatProperties2(MgPhysicalDeviceImageFormatInfo2 pImageFormatInfo, MgImageFormatProperties2 pImageFormatProperties)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2EXT(IMgSurfaceKHR surface, out MgSurfaceCapabilities2EXT pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceSurfaceCapabilities2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceCapabilities2KHR pSurfaceCapabilities)
        {
            throw new NotImplementedException();
        }

        public MgResult GetPhysicalDeviceSurfaceFormats2KHR(MgPhysicalDeviceSurfaceInfo2KHR pSurfaceInfo, out MgSurfaceFormat2KHR[] pSurfaceFormats)
        {
            throw new NotImplementedException();
        }

        public MgResult ReleaseDisplayEXT(IMgDisplayKHR display)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceExternalBufferProperties(MgPhysicalDeviceExternalBufferInfo pExternalBufferInfo, out MgExternalBufferProperties pExternalBufferProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceExternalFenceProperties(MgPhysicalDeviceExternalFenceInfo pExternalFenceInfo, out MgExternalFenceProperties pExternalFenceProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceExternalSemaphoreProperties(MgPhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out MgExternalSemaphoreProperties pExternalSemaphoreProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceFeatures2(out MgPhysicalDeviceFeatures2 pFeatures)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceFormatProperties2(MgFormat format, out MgFormatProperties2 pFormatProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceGeneratedCommandsPropertiesNVX(MgDeviceGeneratedCommandsFeaturesNVX pFeatures, out MgDeviceGeneratedCommandsLimitsNVX pLimits)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceMemoryProperties2(out MgPhysicalDeviceMemoryProperties2 pMemoryProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceMultisamplePropertiesEXT(MgSampleCountFlagBits samples, MgMultisamplePropertiesEXT pMultisampleProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceProperties2(out MgPhysicalDeviceProperties2 pProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceQueueFamilyProperties2(out MgQueueFamilyProperties2[] pQueueFamilyProperties)
        {
            throw new NotImplementedException();
        }

        public void GetPhysicalDeviceSparseImageFormatProperties2(MgPhysicalDeviceSparseImageFormatInfo2 pFormatInfo, out MgSparseImageFormatProperties2[] pProperties)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}

