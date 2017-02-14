using System;
namespace Magnesium.Metal
{
	public class AmtDefaultDeviceEntrypoint : IAmtDeviceEntrypoint
	{
		public AmtDefaultDeviceEntrypoint(
			IAmtFenceEntrypoint fence
			,IAmtMetalLibraryLoader libraryLoader
			,IAmtSemaphoreEntrypoint semaphore
		)
		{
			Fence = fence;
			LibraryLoader = libraryLoader;
			Semaphore = semaphore;
		}

		public IAmtFenceEntrypoint Fence
		{
			get;
			private set;
		}

		public IAmtMetalLibraryLoader LibraryLoader
		{
			get;
			private set;
		}

		public IAmtSemaphoreEntrypoint Semaphore
		{
			get;
			private set;
		}
	}
}
