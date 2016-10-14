using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtEntrypoint : IMgEntrypoint
	{
		private IAmtDeviceQuery mQuery;

		private IMTLDevice mLocalDevice;

		private IAmtMetalLibraryLoader mGenerator;

		public AmtEntrypoint(IAmtDeviceQuery query, IAmtMetalLibraryLoader generator, IMTLDevice localDevice)
		{
			mQuery = query;
			mLocalDevice = localDevice;
			mGenerator = generator;
		}

		public IMgAllocationCallbacks CreateAllocationCallbacks()
		{
			throw new NotImplementedException();
		}

		public Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
		{
			var semaphore = new AmtSemaphoreEntrypoint();
			var presentQueue = mLocalDevice.CreateCommandQueue(mQuery.NoOfCommandBufferSlots);

			var queueRenderer = new AmtQueueRenderer(presentQueue);
			var queue = new AmtQueue(queueRenderer, semaphore, presentQueue);
			var device = new AmtDevice(mLocalDevice, mQuery, mGenerator, queue);
			var physicalDevice = new AmtPhysicalDevice(device);
			instance = new AmtInstance(physicalDevice);

			return Result.SUCCESS;
		}

		public Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			pProperties = new MgExtensionProperties[] { };
			return Result.SUCCESS;
		}

		public Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties)
		{
			properties = new MgLayerProperties[] { };
			return Result.SUCCESS;
		}
	}
}
