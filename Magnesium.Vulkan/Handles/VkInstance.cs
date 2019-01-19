using System;
using System.Diagnostics;

namespace Magnesium.Vulkan
{
	public class VkInstance : IMgInstance
	{
		internal IntPtr Handle { get; private set; }
		internal VkInstance(IntPtr handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Allocator is optional
		/// </summary>
		/// <param name="allocator"></param>
		/// <returns></returns>
		static IntPtr GetAllocatorHandle(IMgAllocationCallbacks allocator)
		{
			var bAllocator = (MgVkAllocationCallbacks)allocator;
			return bAllocator != null ? bAllocator.Handle : IntPtr.Zero;
		}

		private bool mIsDisposed = false;
		public void DestroyInstance(IMgAllocationCallbacks allocator)
		{
			if (!mIsDisposed)
				return;

			var allocatorHandle = GetAllocatorHandle(allocator);	

			Interops.vkDestroyInstance(Handle, allocatorHandle);

			Handle = IntPtr.Zero;
			mIsDisposed = true;
		}

		public MgResult EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
		{
			Debug.Assert(!mIsDisposed);

			var pPropertyCount = 0U;

			var first = Interops.vkEnumeratePhysicalDevices(Handle, ref pPropertyCount, null);

			if (first != MgResult.SUCCESS)
			{
				physicalDevices = null;
				return first;
			}

			var devices = new IntPtr[pPropertyCount];
			var last = Interops.vkEnumeratePhysicalDevices(Handle, ref pPropertyCount, devices);

			physicalDevices = new VkPhysicalDevice[pPropertyCount];
			for (uint i = 0; i < pPropertyCount; ++i)
			{
				physicalDevices[i] = new VkPhysicalDevice(devices[i]);
			}
			return last;
		}

		public PFN_vkVoidFunction GetInstanceProcAddr(string pName)
		{
			Debug.Assert(!mIsDisposed);

			return Interops.vkGetInstanceProcAddr(Handle, pName);
		}

		public MgResult CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (createInfo == null)
				throw new ArgumentNullException(nameof(createInfo));

			Debug.Assert(!mIsDisposed);

			var bDisplayMode = (VkDisplayModeKHR)createInfo.DisplayMode;
			Debug.Assert(bDisplayMode != null);

			var pCreateInfo = new VkDisplaySurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeDisplaySurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = createInfo.Flags,
				displayMode = bDisplayMode.Handle,
				planeIndex = createInfo.PlaneIndex,
				planeStackIndex = createInfo.PlaneStackIndex,
				transform = (VkSurfaceTransformFlagsKhr)createInfo.Transform,
				globalAlpha = createInfo.GlobalAlpha,
				alphaMode = (VkDisplayPlaneAlphaFlagsKhr)createInfo.AlphaMode,
				imageExtent = createInfo.ImageExtent,
			};

			// MIGHT NEED GetInstanceProcAddr INSTEAD

			var allocatorHandle = GetAllocatorHandle(allocator);

			UInt64 handle = 0UL;
			var result = Interops.vkCreateDisplayPlaneSurfaceKHR(Handle, ref pCreateInfo, allocatorHandle, ref handle);
			pSurface = new VkSurfaceKHR(handle);

			return result;
		}

		public MgResult CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD
			var createInfo = new VkAndroidSurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeAndroidSurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				window = pCreateInfo.Window,
			};

			var surfaceHandle = 0UL;
			var result = Interops.vkCreateAndroidSurfaceKHR(Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public MgResult CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			var createInfo = new VkWin32SurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeWin32SurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				hinstance = pCreateInfo.Hinstance,
				hwnd = pCreateInfo.Hwnd,
			};

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD

			var surfaceHandle = 0UL;
			var result = Interops.vkCreateWin32SurfaceKHR(Handle, ref createInfo, allocatorHandle, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public MgResult CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException();

			Debug.Assert(!mIsDisposed);

			var allocatorHandle = GetAllocatorHandle(allocator);

			var createInfo = new VkDebugReportCallbackCreateInfoEXT
			{
				sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
				pNext = IntPtr.Zero,
				flags = (VkDebugReportFlagsExt)pCreateInfo.Flags,
				// TODO : figure out translation
				pfnCallback = null,
				pUserData = pCreateInfo.UserData,
			};

			var callback = 0UL;
			var result = Interops.vkCreateDebugReportCallbackEXT(Handle, ref createInfo, allocatorHandle, ref callback);
			// TODO : figure out translation
			pCallback = new VkDebugReportCallbackEXT(callback);

			return result;
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
		{
			Debug.Assert(!mIsDisposed);

			throw new NotImplementedException();
		}

        public MgResult CreateDebugUtilsMessengerEXT(MgDebugUtilsMessengerCreateInfoEXT createInfo, IMgAllocationCallbacks allocator, out IMgDebugUtilsMessengerEXT pSurface)
        {
            if (createInfo == null)
                throw new ArgumentNullException(nameof(createInfo));

            Debug.Assert(!mIsDisposed);

            var allocatorHandle = GetAllocatorHandle(allocator);

            var bCreateInfo = new VkDebugUtilsMessengerCreateInfoEXT
            {
                 
            };

          //  Interops.vkCreateDebugUtilsMessengerEXT(bCreateInfo,
        }

        public MgResult EnumeratePhysicalDeviceGroups(out MgPhysicalDeviceGroupProperties[] pPhysicalDeviceGroupProperties)
        {
            Debug.Assert(!mIsDisposed);

            var count = 0U;

            var first = Interops.vkEnumeratePhysicalDeviceGroups(Handle, ref count, null);

            var srcGroups = new VkPhysicalDeviceGroupProperties[count];
            var final = Interops.vkEnumeratePhysicalDeviceGroups(Handle, ref count, srcGroups);

            var dstGroups = new MgPhysicalDeviceGroupProperties[count];
            for (var i = 0; i < count; ++i)
            {
                var deviceCount = srcGroups[i].physicalDeviceCount;

                var devices = new IMgPhysicalDevice[deviceCount];

                for(var j = 0; j < deviceCount; j += 1)
                {
                    devices[j] = new VkPhysicalDevice(srcGroups[i].physicalDevices[j]);
                }

                dstGroups[i] = new MgPhysicalDeviceGroupProperties
                {
                    PhysicalDevices = devices,
                    SubsetAllocation = VkBool32.ConvertFrom(srcGroups[i].subsetAllocation),
                };
            }

            pPhysicalDeviceGroupProperties = dstGroups;

            return final;
        }

        public void SubmitDebugUtilsMessageEXT(MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData)
        {
            throw new NotImplementedException();
        }
    }
}
