using System;
using System.Runtime.InteropServices;

namespace Magnesium.Vulkan
{
    public class VkDebugUtilsMessengerData
    {
        delegate VkBool32 PFN_vkDebugUtilsMessengerCallbackEXT(
            MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
            MgDebugUtilsMessageTypeFlagBitsEXT messageType,
            IntPtr pCallbackData, // const VkDebugUtilsMessengerCallbackDataEXT*
            IntPtr pUserData);

        public VkDebugUtilsMessengerData(
            MgDebugUtilsMessengerCallbackEXT callback,
            IntPtr userData
        )
        {
            Callback = callback;
            UserData = userData;

            PFN_vkDebugUtilsMessengerCallbackEXT del = UnwrapCallback;
            UnwrapFn = Marshal.GetFunctionPointerForDelegate(del);

            var callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);

            Capsule = AllocateMemory(this.UserData, callbackPtr);
        }

        public void FreeMemory()
        {
            Callback = null;
            UserData = IntPtr.Zero;
            UnwrapFn = IntPtr.Zero;

            Marshal.FreeHGlobal(Capsule);
        }

        private static IntPtr AllocateMemory(IntPtr userData, IntPtr bActual)
        {
            var stride = Marshal.SizeOf(typeof(VkUnwrapMessengerCallbackCapsule));
            var capsule = Marshal.AllocHGlobal(stride);

            var capsuleData = new VkUnwrapMessengerCallbackCapsule
            {
                PfnCallback = bActual,
                UserData = userData,
            };

            Marshal.StructureToPtr(capsuleData, capsule, false);
            return capsule;
        }

        public IntPtr Capsule { get; private set; }
        public IntPtr UserData { get; private set; }
        public MgDebugUtilsMessengerCallbackEXT Callback { get; private set; }

        public IntPtr UnwrapFn { get; private set; }

        [StructLayout(LayoutKind.Sequential)]
        struct VkUnwrapMessengerCallbackCapsule
        {
            public IntPtr PfnCallback { get; set; }
            public IntPtr UserData { get; set; }
        }

        static MgDebugUtilsLabelEXT TransformIntoLabelInfo(ref VkDebugUtilsLabelEXT src)
        {
            return new MgDebugUtilsLabelEXT
            {
                LabelName = VkInteropsUtility.StringFromNativeUtf8(src.pLabelName),
                Color = src.color,
            };
        }

        static MgDebugUtilsObjectNameInfoEXT TransformIntoObjectInfo(ref VkDebugUtilsObjectNameInfoEXT src)
        {
            return new MgDebugUtilsObjectNameInfoEXT
            {
                ObjectHandle = src.objectHandle,
                ObjectType = src.objectType,
                ObjectName = VkInteropsUtility.StringFromNativeUtf8(src.pObjectName),
            };
        }

        public static VkBool32 UnwrapCallback(
            MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
            MgDebugUtilsMessageTypeFlagBitsEXT messageType,
            IntPtr pCallbackData, // const VkDebugUtilsMessengerCallbackDataEXT*
            IntPtr pUserData
        )
        {
            var callbackData =
                (VkDebugUtilsMessengerCallbackDataEXT)Marshal.PtrToStructure(
                    pCallbackData,
                    typeof(VkDebugUtilsMessengerCallbackDataEXT)
                );

            var userDataCapsule = (VkUnwrapMessengerCallbackCapsule)Marshal.PtrToStructure(
                pUserData, typeof(VkUnwrapMessengerCallbackCapsule));

            var actualCallback = (MgDebugUtilsMessengerCallbackEXT)Marshal.GetDelegateForFunctionPointer(
                userDataCapsule.PfnCallback,
                typeof(MgDebugUtilsMessengerCallbackEXT));

            var queueLabelCount = callbackData.queueLabelCount;
            var queueLabels = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsLabelEXT, MgDebugUtilsLabelEXT>(
                callbackData.pQueueLabels,
                queueLabelCount,
                TransformIntoLabelInfo);

            var cmdBufLabels = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsLabelEXT, MgDebugUtilsLabelEXT>(
                callbackData.pCmdBufLabels,
                callbackData.cmdBufLabelCount,
                TransformIntoLabelInfo);

            var objects = VkInteropsUtility.TransformIntoStructArray<VkDebugUtilsObjectNameInfoEXT, MgDebugUtilsObjectNameInfoEXT>(
                callbackData.pObjects,
                callbackData.objectCount,
                TransformIntoObjectInfo
            );

            var srcCallbackData = new MgDebugUtilsMessengerCallbackDataEXT
            {
                Flags = callbackData.flags,
                Message = VkInteropsUtility.StringFromNativeUtf8(callbackData.pMessage),
                MessageIdNumber = callbackData.messageIdNumber,
                MessageIdName = VkInteropsUtility.StringFromNativeUtf8(callbackData.pMessageIdName),
                QueueLabels = queueLabels,
                CmdBufLabels = cmdBufLabels,
                Objects = objects,
            };

            var result = actualCallback(messageSeverity, messageType, srcCallbackData, userDataCapsule.UserData);

            return VkBool32.ConvertTo(result);
        }
    }
}
