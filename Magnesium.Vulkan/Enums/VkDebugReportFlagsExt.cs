using System;

namespace Magnesium.Vulkan
{
	[Flags]
	internal enum VkDebugReportFlagsExt : uint
	{
		DebugReportInformation = 0x1,
		DebugReportWarning = 0x2,
		DebugReportPerformanceWarning = 0x4,
		DebugReportError = 0x8,
		DebugReportDebug = 0x10,
	}
}
