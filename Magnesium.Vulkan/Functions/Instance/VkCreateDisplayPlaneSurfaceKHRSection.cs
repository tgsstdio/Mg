using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public class VkCreateDisplayPlaneSurfaceKHRSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static MgResult vkCreateDisplayPlaneSurfaceKHR(IntPtr instance, ref VkDisplaySurfaceCreateInfoKHR pCreateInfo, IntPtr pAllocator, ref UInt64 pSurface);

        public static MgResult CreateDisplayPlaneSurfaceKHR(VkInstanceInfo info, MgDisplaySurfaceCreateInfoKHR createInfo, IMgAllocationCallbacks allocator, out IMgSurfaceKHR pSurface)
        {
            Debug.Assert(!info.IsDisposed);

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

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            UInt64 handle = 0UL;
            var result = vkCreateDisplayPlaneSurfaceKHR(info.Handle, ref pCreateInfo, allocatorHandle, ref handle);
            pSurface = new VkSurfaceKHR(handle);

            return result;
        }
    }
}
