using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkGetPipelineCacheDataSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkGetPipelineCacheData(IntPtr device, UInt64 pipelineCache, ref UIntPtr pDataSize, IntPtr pData);

        public static MgResult GetPipelineCacheData(VkDeviceInfo info, IMgPipelineCache pipelineCache, out Byte[] pData)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var bPipelineCache = (VkPipelineCache)pipelineCache;
            Debug.Assert(bPipelineCache != null);

            UIntPtr dataSize = UIntPtr.Zero;
            var first = vkGetPipelineCacheData(info.Handle, bPipelineCache.Handle, ref dataSize, IntPtr.Zero);

            if (first != MgResult.SUCCESS)
            {
                pData = null;
                return first;
            }

            pData = new byte[dataSize.ToUInt64()];
            GCHandle pinnedArray = GCHandle.Alloc(pData, GCHandleType.Pinned);
            try
            {
                var dest = pinnedArray.AddrOfPinnedObject();
                return vkGetPipelineCacheData(info.Handle, bPipelineCache.Handle, ref dataSize, dest);
            }
            finally
            {
                pinnedArray.Free();
            }
        }
    }
}
