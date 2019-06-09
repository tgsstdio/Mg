using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkMergePipelineCachesSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkMergePipelineCaches(IntPtr device, UInt64 dstCache, UInt32 srcCacheCount, [In] UInt64[] pSrcCaches);

        public static MgResult MergePipelineCaches(VkDeviceInfo info, IMgPipelineCache dstCache, IMgPipelineCache[] pSrcCaches)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bDstCache = (VkPipelineCache)dstCache;
            Debug.Assert(bDstCache != null);

            var srcCacheCount = (UInt32)pSrcCaches.Length;

            ulong[] cacheHandles = new ulong[srcCacheCount];
            for (var i = 0; i < srcCacheCount; ++i)
            {
                var bCache = (VkPipelineCache)pSrcCaches[i];
                Debug.Assert(bCache != null);
                cacheHandles[i] = bCache.Handle;
            }

            return vkMergePipelineCaches(info.Handle, bDstCache.Handle, srcCacheCount, cacheHandles);
        }
    }
}
