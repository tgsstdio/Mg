using System;
namespace Magnesium.Metal
{
	public interface IAmtDeviceEntrypoint
	{
		IAmtSemaphoreEntrypoint Semaphore { get; }
		IAmtFenceEntrypoint Fence { get; }
		IAmtMetalLibraryLoader LibraryLoader { get; }

	}
}
