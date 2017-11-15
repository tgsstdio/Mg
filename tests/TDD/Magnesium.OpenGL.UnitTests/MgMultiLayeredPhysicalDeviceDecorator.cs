namespace Magnesium.OpenGL.UnitTests
{
    class MgMultiLayeredPhysicalDeviceDecorator : IMgPhysicalDevice
    {
        private IMgPhysicalDevice mElement;
        private IMgPhysicalDeviceValidationLayer[] mLayers;
        private IMgDecoratorFactory mFactory;

        public MgMultiLayeredPhysicalDeviceDecorator(
            IMgPhysicalDevice element,
            IMgPhysicalDeviceValidationLayer[] layers,
            IMgDecoratorFactory factory
            )
        {
            mElement = element;
            mLayers = layers;
            mFactory = factory;
        }

        public Result CreateDevice(MgDeviceCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgDevice pDevice)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateDevice(mElement, pCreateInfo, allocator);
                }
            }            

            var result = mElement.CreateDevice(pCreateInfo, allocator, out IMgDevice rawElement);
            if (result == Result.SUCCESS)
            {
                pDevice = mFactory.WrapDevice(rawElement);
                return result;
            }
            else
            {
                pDevice = rawElement;
                return result;
            }           
        }

        public Result CreateDisplayModeKHR(IMgDisplayKHR display, MgDisplayModeCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgDisplayModeKHR pMode)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.CreateDisplayModeKHR(mElement, display, pCreateInfo, allocator);
                }
            }
            return mElement.CreateDisplayModeKHR(display, pCreateInfo, allocator, out pMode);
        }

        public Result EnumerateDeviceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.EnumerateDeviceExtensionProperties(mElement, layerName);
                }
            }
            return mElement.EnumerateDeviceExtensionProperties(layerName, out pProperties);
        }

        public Result EnumerateDeviceLayerProperties(out MgLayerProperties[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.EnumerateDeviceLayerProperties(mElement);
                }
            }
            return mElement.EnumerateDeviceLayerProperties(out pProperties);
        }

        public Result GetDisplayModePropertiesKHR(IMgDisplayKHR display, out MgDisplayModePropertiesKHR[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDisplayModePropertiesKHR(mElement, display);
                }
            }
            return mElement.GetDisplayModePropertiesKHR(display, out pProperties);
        }

        public Result GetDisplayPlaneCapabilitiesKHR(IMgDisplayModeKHR mode, uint planeIndex, out MgDisplayPlaneCapabilitiesKHR pCapabilities)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDisplayPlaneCapabilitiesKHR(mElement, mode, planeIndex);
                }
            }
            return mElement.GetDisplayPlaneCapabilitiesKHR(mode, planeIndex, out pCapabilities);
        }

        public Result GetDisplayPlaneSupportedDisplaysKHR(uint planeIndex, out IMgDisplayKHR[] pDisplays)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetDisplayPlaneSupportedDisplaysKHR(mElement, planeIndex);
                }
            }
            return mElement.GetDisplayPlaneSupportedDisplaysKHR(planeIndex, out pDisplays);
        }

        public Result GetPhysicalDeviceDisplayPlanePropertiesKHR(out MgDisplayPlanePropertiesKHR[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceDisplayPlanePropertiesKHR(mElement);
                }
            }
            return mElement.GetPhysicalDeviceDisplayPlanePropertiesKHR(out pProperties);
        }

        public Result GetPhysicalDeviceDisplayPropertiesKHR(out MgDisplayPropertiesKHR[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceDisplayPropertiesKHR(mElement);
                }
            }
            return mElement.GetPhysicalDeviceDisplayPropertiesKHR(out pProperties);
        }

        public void GetPhysicalDeviceFeatures(out MgPhysicalDeviceFeatures pFeatures)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceFeatures(mElement);
                }
            }
            mElement.GetPhysicalDeviceFeatures(out pFeatures);
        }

        public void GetPhysicalDeviceFormatProperties(MgFormat format, out MgFormatProperties pFormatProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceFormatProperties(mElement, format);
                }
            }
            mElement.GetPhysicalDeviceFormatProperties(format, out pFormatProperties);
        }

        public Result GetPhysicalDeviceImageFormatProperties(MgFormat format, MgImageType type, MgImageTiling tiling, MgImageUsageFlagBits usage, MgImageCreateFlagBits flags, out MgImageFormatProperties pImageFormatProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceImageFormatProperties(mElement, format, type, tiling, usage, flags);
                }
            }
            return mElement.GetPhysicalDeviceImageFormatProperties(format, type, tiling, usage, flags, out pImageFormatProperties);
        }

        public void GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceMemoryProperties(mElement);
                }
            }
            mElement.GetPhysicalDeviceMemoryProperties(out pMemoryProperties);
        }

        public void GetPhysicalDeviceProperties(out MgPhysicalDeviceProperties pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceProperties(mElement);
                }
            }
            mElement.GetPhysicalDeviceProperties(out pProperties);
        }

        public void GetPhysicalDeviceQueueFamilyProperties(out MgQueueFamilyProperties[] pQueueFamilyProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceQueueFamilyProperties(mElement);
                }
            }
            mElement.GetPhysicalDeviceQueueFamilyProperties(out pQueueFamilyProperties);
        }

        public void GetPhysicalDeviceSparseImageFormatProperties(MgFormat format, MgImageType type, MgSampleCountFlagBits samples, MgImageUsageFlagBits usage, MgImageTiling tiling, out MgSparseImageFormatProperties[] pProperties)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceSparseImageFormatProperties(mElement, format, type, samples, usage, tiling);
                }
            }
            mElement.GetPhysicalDeviceSparseImageFormatProperties(format, type, samples, usage, tiling, out pProperties);
        }

        public Result GetPhysicalDeviceSurfaceCapabilitiesKHR(IMgSurfaceKHR surface, out MgSurfaceCapabilitiesKHR pSurfaceCapabilities)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceSurfaceCapabilitiesKHR(mElement, surface);
                }
            }
            return mElement.GetPhysicalDeviceSurfaceCapabilitiesKHR(surface, out pSurfaceCapabilities);
        }

        public Result GetPhysicalDeviceSurfaceFormatsKHR(IMgSurfaceKHR surface, out MgSurfaceFormatKHR[] pSurfaceFormats)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceSurfaceFormatsKHR(mElement, surface);
                }
            }
            return mElement.GetPhysicalDeviceSurfaceFormatsKHR(surface, out pSurfaceFormats);
        }

        public Result GetPhysicalDeviceSurfacePresentModesKHR(IMgSurfaceKHR surface, out MgPresentModeKHR[] pPresentModes)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceSurfacePresentModesKHR(mElement, surface);
                }
            }
            return mElement.GetPhysicalDeviceSurfacePresentModesKHR(surface, out pPresentModes);
        }

        public Result GetPhysicalDeviceSurfaceSupportKHR(uint queueFamilyIndex, IMgSurfaceKHR surface, ref bool pSupported)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceSurfaceSupportKHR(mElement, queueFamilyIndex, surface);
                }
            }
            return mElement.GetPhysicalDeviceSurfaceSupportKHR(queueFamilyIndex, surface, ref pSupported);
        }

        public bool GetPhysicalDeviceWin32PresentationSupportKHR(uint queueFamilyIndex)
        {
            if (mLayers != null)
            {
                foreach (var layer in mLayers)
                {
                    if (layer != null)
                        layer.GetPhysicalDeviceWin32PresentationSupportKHR(mElement, queueFamilyIndex);
                }
            }
            return mElement.GetPhysicalDeviceWin32PresentationSupportKHR(queueFamilyIndex);
        }
    }
}
