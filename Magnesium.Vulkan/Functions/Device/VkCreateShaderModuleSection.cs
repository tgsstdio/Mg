using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Device
{
	public class VkCreateShaderModuleSection
	{
		[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]
        internal extern static MgResult vkCreateShaderModule(IntPtr device, ref VkShaderModuleCreateInfo pCreateInfo, IntPtr pAllocator, ref UInt64 pShaderModule);

        public static MgResult CreateShaderModule(VkDeviceInfo info, MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator, out IMgShaderModule pShaderModule)
        {
            Debug.Assert(!info.IsDisposed, "VkDevice has been disposed");

            var allocatorPtr = VkDeviceInfo.GetAllocatorHandle(allocator);

            var bufferSize = (int)pCreateInfo.CodeSize;
            var dest = Marshal.AllocHGlobal(bufferSize);

            try
            {

                Debug.Assert(pCreateInfo.Code != null);

                using (var ms = new MemoryStream())
                {
                    pCreateInfo.Code.CopyTo(ms, bufferSize);
                    Marshal.Copy(ms.ToArray(), 0, dest, bufferSize);
                }

                var createInfo = new VkShaderModuleCreateInfo
                {
                    sType = VkStructureType.StructureTypeShaderModuleCreateInfo,
                    pNext = IntPtr.Zero,
                    flags = pCreateInfo.Flags,
                    codeSize = pCreateInfo.CodeSize,
                    pCode = dest
                };
                ulong internalHandle = 0;
                var result = vkCreateShaderModule(info.Handle, ref createInfo, allocatorPtr, ref internalHandle);
                pShaderModule = new VkShaderModule(internalHandle);
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(dest);
            }
        }
    }
}
