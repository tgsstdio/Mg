using System;

namespace Magnesium
{
	public class MgDebugUtilsMessengerCallbackDataEXT
	{
		public UInt32 Flags { get; set; }
		public string MessageIdName { get; set; }
		public Int32 MessageIdNumber { get; set; }
		public string Message { get; set; }
		public MgDebugUtilsLabelEXT[] QueueLabels { get; set; }
		public MgDebugUtilsLabelEXT[] CmdBufLabels { get; set; }
		public MgDebugUtilsObjectNameInfoEXT[] Objects { get; set; }
	}
}
