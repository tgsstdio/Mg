using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public class VkQueueBeginDebugUtilsLabelEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkQueueBeginDebugUtilsLabelEXT(IntPtr queue, ref VkDebugUtilsLabelEXT pLabelInfo);

        public static void QueueBeginDebugUtilsLabelEXT(VkQueueInfo info, MgDebugUtilsLabelEXT labelInfo)
        {
            var pLabelName = IntPtr.Zero;

            try
            {
                pLabelName = VkInteropsUtility.NativeUtf8FromString(labelInfo.LabelName);

                var lbl = new VkDebugUtilsLabelEXT
                {
                    sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                    pNext = IntPtr.Zero,
                    pLabelName = pLabelName,
                    color = labelInfo.Color,
                };

                vkQueueBeginDebugUtilsLabelEXT(info.Handle, ref lbl);
            }
            finally
            {
                Marshal.FreeHGlobal(pLabelName);
            }
        }
    }
}
