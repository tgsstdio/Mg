using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public static class VkGetImageSparseMemoryRequirementsSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static unsafe void vkGetImageSparseMemoryRequirements(IntPtr device, UInt64 image, UInt32* pSparseMemoryRequirementCount, Magnesium.MgSparseImageMemoryRequirements* pSparseMemoryRequirements);

        public static void GetImageSparseMemoryRequirements(VkDeviceInfo info, IMgImage image, out MgSparseImageMemoryRequirements[] sparseMemoryRequirements)
		{
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bImage = (VkImage)image;
            Debug.Assert(bImage != null);

            var requirements = new uint[1];
            unsafe
            {
                fixed (uint* count = &requirements[0])
                {
                    vkGetImageSparseMemoryRequirements(info.Handle, bImage.Handle, count, null);
                }
            }

            var arrayLength = (int)requirements[0];
            sparseMemoryRequirements = new MgSparseImageMemoryRequirements[arrayLength];

            GCHandle smrHandle = GCHandle.Alloc(sparseMemoryRequirements, GCHandleType.Pinned);

            try
            {
                unsafe
                {
                    IntPtr pinnedArray = smrHandle.AddrOfPinnedObject();
                    var sparseReqs = (MgSparseImageMemoryRequirements*)pinnedArray.ToPointer();

                    fixed (uint* count = &requirements[0])
                    {
                        vkGetImageSparseMemoryRequirements(info.Handle, bImage.Handle, count, sparseReqs);
                    }
                }
            }
            finally
            {
                smrHandle.Free();
            }
        }
	}
}
