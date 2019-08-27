using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan.Functions.Instance
{
	public static class VkSubmitDebugUtilsMessageEXTSection
	{
		[DllImport(Interops.VULKAN_LIB_1, CallingConvention = CallingConvention.Winapi)]
        internal extern static void vkSubmitDebugUtilsMessageEXT(IntPtr instance, MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, [In, Out] VkDebugUtilsMessengerCallbackDataEXT pCallbackData);

        public static void SubmitDebugUtilsMessageEXT(VkInstanceInfo info, MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, MgDebugUtilsMessageTypeFlagBitsEXT messageTypes, MgDebugUtilsMessengerCallbackDataEXT pCallbackData)
        {
            Debug.Assert(!info.IsDisposed);

            var queueLabelCount =
                pCallbackData.QueueLabels != null
                ? (UInt32)pCallbackData.QueueLabels.Length
                : 0U;

            var pQueueLabels = IntPtr.Zero;

            var cmdBufLabelCount =
                pCallbackData.CmdBufLabels != null
                ? (UInt32)pCallbackData.CmdBufLabels.Length
                : 0U;

            var pCmdBufLabels = IntPtr.Zero;

            var objectCount =
                pCallbackData.Objects != null
                ? (UInt32)pCallbackData.Objects.Length
                : 0U;

            var pObjects = IntPtr.Zero;

            var pinned = new List<IntPtr>();
            try
            {
                if (queueLabelCount > 0)
                {
                    pQueueLabels = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.QueueLabels,
                        (f) =>
                        {
                            var lbl = VkInteropsUtility.NativeUtf8FromString(f.LabelName);
                            pinned.Add(lbl);

                            return new VkDebugUtilsLabelEXT
                            {
                                sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                                pNext = IntPtr.Zero,
                                pLabelName = lbl,
                                color = f.Color,
                            };
                        });
                    pinned.Add(pQueueLabels);
                }

                if (cmdBufLabelCount > 0)
                {
                    pCmdBufLabels = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.CmdBufLabels,
                        (f) =>
                        {
                            var lbl = VkInteropsUtility.NativeUtf8FromString(f.LabelName);
                            pinned.Add(lbl);

                            return new VkDebugUtilsLabelEXT
                            {
                                sType = VkStructureType.StructureTypeDebugUtilsLabelExt,
                                pNext = IntPtr.Zero,
                                pLabelName = lbl,
                                color = f.Color,
                            };
                        });
                    pinned.Add(pCmdBufLabels);
                }

                if (objectCount > 0)
                {
                    pObjects = VkInteropsUtility.AllocateHGlobalArray(pCallbackData.Objects,
                        (f) =>
                        {
                            var pObjectName = IntPtr.Zero;
                            if (!string.IsNullOrEmpty(f.ObjectName))
                            {
                                pObjectName = VkInteropsUtility.NativeUtf8FromString(f.ObjectName);
                                pinned.Add(pObjectName);
                            }

                            return new VkDebugUtilsObjectNameInfoEXT
                            {
                                sType = VkStructureType.StructureTypeDebugMarkerObjectNameInfoExt,
                                pNext = IntPtr.Zero,
                                objectHandle = f.ObjectHandle,
                                pObjectName = pObjectName,
                                objectType = f.ObjectType,
                            };
                        }
                        );

                    pinned.Add(pObjects);
                }


                var pMessageIdName = IntPtr.Zero;

                if (!string.IsNullOrEmpty(pCallbackData.MessageIdName))
                {
                    pMessageIdName = VkInteropsUtility.NativeUtf8FromString(pCallbackData.MessageIdName);
                    pinned.Add(pMessageIdName);
                }

                var bCallbackData = new VkDebugUtilsMessengerCallbackDataEXT
                {
                    sType = VkStructureType.StructureTypeDebugUtilsMessengerCallbackDataExt,
                    pNext = IntPtr.Zero,
                    flags = pCallbackData.Flags,
                    pMessage = VkInteropsUtility.NativeUtf8FromString(pCallbackData.Message),
                    messageIdNumber = pCallbackData.MessageIdNumber,
                    pMessageIdName = pMessageIdName,
                    queueLabelCount = queueLabelCount,
                    pQueueLabels = pQueueLabels,
                    cmdBufLabelCount = cmdBufLabelCount,
                    pCmdBufLabels = pCmdBufLabels,
                    objectCount = objectCount,
                    pObjects = pObjects,
                };

                vkSubmitDebugUtilsMessageEXT(info.Handle, messageSeverity, messageTypes, bCallbackData);
            }
            finally
            {
                foreach (var pin in pinned)
                {
                    Marshal.FreeHGlobal(pin);
                }

            }
        }
    }
}
