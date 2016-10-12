using System;
using Magnesium;

namespace MetalSample
{
	public class MgGraphicsConfiguration : IDisposable
	{
		private Magnesium.MgDriver mDriver;
		private Magnesium.IMgPresentationSurface mPresentationSurface;
		public Magnesium.IMgDevice Device { 
			get
			{
				return LogicalDevice.Device;
			}
		}

		public Magnesium.IMgQueue Queue
		{
			get
			{
				return Partition.Queue;
			}
		}

		public Magnesium.IMgThreadPartition Partition { get; private set;}

		public IMgLogicalDevice LogicalDevice { get; private set;}

		public MgGraphicsConfiguration(Magnesium.MgDriver driver,
		                               Magnesium.IMgPresentationSurface presentationSurface)
		{
			mDriver = driver;
			var errorCode = mDriver.Initialize(
				new Magnesium.MgApplicationInfo
				{
					ApplicationName = "MetalSample",
					EngineName = "Magnesium",
					ApplicationVersion = 1,
					EngineVersion = 1,
					ApiVersion = Magnesium.MgApplicationInfo.GenerateApiVersion(1, 0, 17),
				},
				  Magnesium.MgInstanceExtensionOptions.ALL
			 );

			if (errorCode != Result.SUCCESS)
			{
				throw new InvalidOperationException("Metal is not supported on this device : " + errorCode);
			}

			// WINDOW HOOK 
			mPresentationSurface =  presentationSurface;
			presentationSurface.Initialize();
			LogicalDevice = mDriver.CreateLogicalDevice(presentationSurface.Surface,
														Magnesium.MgDeviceExtensionOptions.ALL);
			Partition = LogicalDevice.Queues[0].CreatePartition(
						MgCommandPoolCreateFlagBits.RESET_COMMAND_BUFFER_BIT,
						 new Magnesium.MgDescriptorPoolCreateInfo
						 {
							 MaxSets = 1,
							 PoolSizes = new MgDescriptorPoolSize[] {
									new MgDescriptorPoolSize
									{
										DescriptorCount = 1,
										Type =  MgDescriptorType.COMBINED_IMAGE_SAMPLER,
									},
							},
						 });

		}

		~MgGraphicsConfiguration()
		{
			Dispose(false);
		}

		private bool mIsDisposed = false;
		protected void Dispose(bool v)
		{
			if (mIsDisposed)
				return;

			if (Partition != null)
				Partition.Dispose();

			if (LogicalDevice != null)
				LogicalDevice.Dispose();

			mIsDisposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
