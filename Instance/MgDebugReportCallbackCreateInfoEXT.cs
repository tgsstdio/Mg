using System;

namespace Magnesium
{
    public class MgDebugReportCallbackCreateInfoEXT
	{
		public MgDebugReportFlagBitsEXT Flags { get; set; }
		public PFN_vkDebugReportCallbackEXT PfnCallback { get; set; }
		public IntPtr UserData { get; set; }
	}
}

