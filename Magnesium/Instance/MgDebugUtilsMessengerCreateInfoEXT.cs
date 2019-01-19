using System;

namespace Magnesium
{
    public delegate bool MgDebugUtilsMessengerCallbackEXT(
        MgDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
        MgDebugUtilsMessageTypeFlagBitsEXT messageType,
        MgDebugUtilsMessengerCallbackDataEXT callbackData,
        IntPtr userData
        );

    public class MgDebugUtilsMessengerCreateInfoEXT
	{
		public UInt32 Flags { get; set; }
		public MgDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity { get; set; }
		public MgDebugUtilsMessageTypeFlagBitsEXT MessageType { get; set; }
		public MgDebugUtilsMessengerCallbackEXT PfnUserCallback { get; set; } // PFN_vkDebugUtilsMessengerCallbackEXT
        public IntPtr PUserData { get; set; }
	}
}
