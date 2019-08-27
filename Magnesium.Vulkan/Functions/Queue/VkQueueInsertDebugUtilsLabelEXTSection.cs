using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Queue
{
	public static class VkQueueInsertDebugUtilsLabelEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention=CallingConvention.Winapi)]
        internal extern static void vkQueueInsertDebugUtilsLabelEXT(IntPtr queue, ref VkDebugUtilsLabelEXT pLabelInfo);

        public static void QueueInsertDebugUtilsLabelEXT(VkQueueInfo info, MgDebugUtilsLabelEXT labelInfo)
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

                vkQueueInsertDebugUtilsLabelEXT(info.Handle, ref lbl);
            }
            finally
            {
                Marshal.FreeHGlobal(pLabelName);
            }
        }
    }
}
