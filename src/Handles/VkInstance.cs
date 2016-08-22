using Magnesium;
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

		private bool mIsDisposed = false;
		public void DestroyInstance(IMgAllocationCallbacks allocator)
		{
			if (!mIsDisposed)
				return;

			var bAllocator = allocator as MgVkAllocationCallbacks;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			Interops.vkDestroyInstance(this.Handle, allocatorPtr);

			Handle = IntPtr.Zero;
			mIsDisposed = true;
		}

		public Result EnumeratePhysicalDevices(out IMgPhysicalDevice[] physicalDevices)
		{
			Debug.Assert(!mIsDisposed);

			UInt32 pPropertyCount = 0;

			var first = Interops.vkEnumeratePhysicalDevices(this.Handle, ref pPropertyCount, null);

			if (first != Result.SUCCESS)
			{
				physicalDevices = null;
				return first;
			}

			var devices = new IntPtr[pPropertyCount];
			var last = Interops.vkEnumeratePhysicalDevices(this.Handle, ref pPropertyCount, devices);

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

			return Interops.vkGetInstanceProcAddr(this.Handle, pName);
		}

		public Result CreateDisplayPlaneSurfaceKHR(MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
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

			var bAllocator = allocator as MgVkAllocationCallbacks;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			UInt64 handle = 0UL;
			var result = Interops.vkCreateDisplayPlaneSurfaceKHR(this.Handle, pCreateInfo, allocatorPtr, ref handle);
			pSurface = new VkSurfaceKHR(handle);

			return result;
		}

		public Result CreateAndroidSurfaceKHR(MgAndroidSurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD
			var createInfo = new VkAndroidSurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeAndroidSurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				window = pCreateInfo.Window,
			};

			ulong surfaceHandle = 0;
			var result = Interops.vkCreateAndroidSurfaceKHR(this.Handle, createInfo, allocatorPtr, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public Result CreateWin32SurfaceKHR(MgWin32SurfaceCreateInfoKHR pCreateInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
		{
			if (pCreateInfo == null)
				throw new ArgumentNullException(nameof(pCreateInfo));

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks) allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkWin32SurfaceCreateInfoKHR
			{
				sType = VkStructureType.StructureTypeWin32SurfaceCreateInfoKhr,
				pNext = IntPtr.Zero,
				flags = pCreateInfo.Flags,
				hinstance = pCreateInfo.Hinstance,
				hwnd = pCreateInfo.Hwnd,
			};

			// TODO : MIGHT NEED GetInstanceProcAddr INSTEAD

			ulong surfaceHandle = 0;
			var result = Interops.vkCreateWin32SurfaceKHR(this.Handle, createInfo, allocatorPtr, ref surfaceHandle);
			pSurface = new VkSurfaceKHR(surfaceHandle);

			return result;
		}

		public Result CreateDebugReportCallbackEXT(MgDebugReportCallbackCreateInfoEXT pCreateInfo, IMgAllocationCallbacks allocator, out IMgDebugReportCallbackEXT pCallback)
		{
			throw new NotImplementedException();

			Debug.Assert(!mIsDisposed);

			var bAllocator = (MgVkAllocationCallbacks)allocator;
			IntPtr allocatorPtr = bAllocator != null ? bAllocator.Handle : IntPtr.Zero;

			var createInfo = new VkDebugReportCallbackCreateInfoEXT
			{
				sType = VkStructureType.StructureTypeDebugReportCallbackCreateInfoExt,
				pNext = IntPtr.Zero,
				flags = (Magnesium.Vulkan.VkDebugReportFlagsExt)pCreateInfo.Flags,
				// TODO : figure out translation
				pfnCallback = null,
				pUserData = pCreateInfo.UserData,
			};

			UInt64 callback = 0;
			var result = Interops.vkCreateDebugReportCallbackEXT(this.Handle, createInfo, allocatorPtr, ref callback);
			// TODO : figure out translation
			pCallback = new VkDebugReportCallbackEXT(callback);

			return result;
		}

		public void DebugReportMessageEXT(MgDebugReportFlagBitsEXT flags, MgDebugReportObjectTypeEXT objectType, UInt64 @object, IntPtr location, Int32 messageCode, string pLayerPrefix, string pMessage)
		{
			Debug.Assert(!mIsDisposed);

			throw new NotImplementedException();
		}

	}
}
