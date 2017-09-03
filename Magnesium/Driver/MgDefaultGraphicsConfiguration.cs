using System;
using System.Diagnostics;

namespace Magnesium
{
	public class MgDefaultGraphicsConfiguration : IMgGraphicsConfiguration
    {
        private IMgLogicalDevice mLogicalDevice;
		public IMgDevice Device { 
			get
			{
                Debug.Assert(mLogicalDevice != null);
                return mLogicalDevice.Device;
			}
		}

		public IMgQueue Queue
		{
			get
			{
				return Partition.Queue;
			}
		}

        private IMgThreadPartition mPartition;
        public IMgThreadPartition Partition {
            get
            {
                Debug.Assert(mPartition != null);
                return mPartition;
            }
        }

        private MgPhysicalDeviceMemoryProperties mMemoryProperties;
        public MgPhysicalDeviceMemoryProperties MemoryProperties {
            get
            {
                return mMemoryProperties;
            }
        }

        private readonly MgDriverContext mDriverContext;
        private readonly IMgPresentationSurface mPresentationSurface;
        public MgDefaultGraphicsConfiguration(
            MgDriverContext context,
            IMgPresentationSurface presentationSurface)
		{
            // WINDOW HOOK 
            mDriverContext = context;
			mPresentationSurface =  presentationSurface;            
		}

        public void Initialize(uint width, uint height)
        {
            // FOR RESIZING ??
            ReleaseUnmanagedResources();
            // GRAPHICS DEVICE
            mPresentationSurface.Initialize(width, height);
            mLogicalDevice = mDriverContext.CreateLogicalDevice(
                mPresentationSurface.Surface,
                MgDeviceExtensionOptions.SWAPCHAIN_ONLY,
                MgQueueAllocation.One,
                MgQueueFlagBits.GRAPHICS_BIT | MgQueueFlagBits.COMPUTE_BIT);

            mPartition = mLogicalDevice.Queues[0].CreatePartition(MgCommandPoolCreateFlagBits.RESET_COMMAND_BUFFER_BIT);

            mPartition.PhysicalDevice.GetPhysicalDeviceMemoryProperties(out MgPhysicalDeviceMemoryProperties pMemoryProperties);
            mMemoryProperties = pMemoryProperties;
        }

		~MgDefaultGraphicsConfiguration()
		{
			Dispose(false);
		}

		private bool mIsDisposed = false;
        protected void Dispose(bool disposing)
        {
            if (mIsDisposed)
                return;

            ReleaseUnmanagedResources();

            mIsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            if (mPartition != null)
                mPartition.Dispose();

            if (mLogicalDevice != null)
                mLogicalDevice.Dispose();
        }

        public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
