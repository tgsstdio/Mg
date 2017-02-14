using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtEntrypoint : IMgEntrypoint
	{
		private IAmtDeviceQuery mQuery;

		private IMTLDevice mLocalDevice;

		private IAmtDeviceEntrypoint mDeviceEntrypoint;
		public AmtEntrypoint(
			IMTLDevice localDevice, 
			IAmtDeviceQuery query, 
			IAmtDeviceEntrypoint device)
		{
			mQuery = query;
			mLocalDevice = localDevice;
			mDeviceEntrypoint = device;
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
			var device = new AmtDevice(mLocalDevice, mQuery, mDeviceEntrypoint, queue);
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
