using System;

namespace Magnesium
{
	public class MgDebugUtilsMessengerCreateInfoEXT
	{
		public UInt32 Flags { get; set; }
		public MgDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity { get; set; }
		public MgDebugUtilsMessageTypeFlagBitsEXT MessageType { get; set; }
		public IntPtr PfnUserCallback { get; set; } // PFN_vkDebugUtilsMessengerCallbackEXT
        public IntPtr PUserData { get; set; }
	}
}
