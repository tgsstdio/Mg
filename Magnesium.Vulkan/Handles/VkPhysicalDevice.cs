using Magnesium;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
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
