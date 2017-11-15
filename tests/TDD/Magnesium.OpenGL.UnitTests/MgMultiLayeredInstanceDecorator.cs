using System;

namespace Magnesium.OpenGL.UnitTests
{
    class MgMultiLayeredInstanceDecorator : IMgInstance        
    {
        private IMgInstance mElement;
        private IMgInstanceValidationLayer[] mInstanceLayers;
        private IMgDecoratorFactory mFactory;
        private IMgDestroyElementLogger mDestroyChecker;

        public MgMultiLayeredInstanceDecorator(
            IMgInstance impl,
            IMgInstanceValidationLayer[] instanceLayers,
            IMgDecoratorFactory factory,
            IMgDestroyElementLogger destroyCheck
        )
        {
            this.mElement = impl;
            this.mInstanceLayers = instanceLayers;
            this.mFactory = factory;
            this.mDestroyChecker = destroyCheck;
        }

        public Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.CreateAndroidSurfaceKHR(mElement, pCreateInfo, allocator);
                }
            }

            return mElement.CreateAndroidSurfaceKHR(pCreateInfo, allocator, out pSurface);
        }

        public Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.CreateDebugReportCallbackEXT(mElement, pCreateInfo, allocator);
                }
            }
            return mElement.CreateDebugReportCallbackEXT(pCreateInfo, allocator, out pCallback);
        }

        public Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.CreateDisplayPlaneSurfaceKHR(mElement, createInfo, allocator);
                }
            }
            return mElement.CreateDisplayPlaneSurfaceKHR(createInfo, allocator, out pSurface);
        }

        public Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.DestroyInstance(mElement, allocator);
                }
            }
            return mElement.CreateWin32SurfaceKHR(pCreateInfo, allocator, out pSurface);
        }

        public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, ulong @object, IntPtr location, int messageCode, string pLayerPrefix, string pMessage)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.DebugReportMessageEXT(mElement, flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
                }
            }
            mElement.DebugReportMessageEXT(flags, objectType, @object, location, messageCode, pLayerPrefix, pMessage);
        }

        private bool mIsDestroyed = false;
        public void DestroyInstance(IMgAllocationCallbacks allocator)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.DestroyInstance(mElement, allocator);
                }
            }

            mElement.DestroyInstance(allocator);
            mIsDestroyed = true;            
        }

        public Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.EnumeratePhysicalDevices(mElement);
                }
            }
            var result = mElement.EnumeratePhysicalDevices(out IMgPhysicalDevice[] rawElements);
            if (result != Result.SUCCESS)
            {
                physicalDevices = rawElements;
                return result;
            }
            else
            {
                if (rawElements != null)
                {
                    var wrappedElements = new IMgPhysicalDevice[rawElements.Length];
                    for(var i = 0; i < rawElements.Length; i += 1)
                    {
                        wrappedElements[i] = mFactory.WrapPhysicalDevice(rawElements[i]);
                    }
                    physicalDevices = wrappedElements;
                }
                else
                {
                    physicalDevices = rawElements;
                }
                return result;
            }
        }

        public IntPtr GetInstanceProcAddr(string pName)
        {
            mDestroyChecker.Debug(mIsDestroyed);
            if (mInstanceLayers != null)
            {
                foreach (var layer in mInstanceLayers)
                {
                    if (layer != null)
                        layer.GetInstanceProcAddr(mElement, pName);
                }
            }
            return mElement.GetInstanceProcAddr(pName);
        }
    }
}
