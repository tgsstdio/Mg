using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtEntrypoint : IMgEntrypoint
	{
		readonly IAmtDeviceQuery mQuery;
		readonly IMTLDevice mLocalDevice;
		readonly IAmtDeviceEntrypoint mDeviceEntrypoint;
        readonly IAmtPhysicalDeviceFormatLookupEntrypoint mFormatLookup;

        public AmtEntrypoint(
			IMTLDevice localDevice,
			IAmtDeviceQuery query, 
			IAmtDeviceEntrypoint device,
            IAmtPhysicalDeviceFormatLookupEntrypoint formatLookup
        )
		{
			mLocalDevice = localDevice;
			mQuery = query;
			mDeviceEntrypoint = device;
            mFormatLookup = formatLookup;
		}

		public IMgAllocationCallbacks CreateAllocationCallbacks()
		{
			throw new NotImplementedException();
		}

		public Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
		{
			var presentQueue = mLocalDevice.CreateCommandQueue(mQuery.NoOfCommandBufferSlots);

			var queueRenderer = new AmtQueueRenderer(presentQueue);
			var queue = new AmtQueue(queueRenderer, presentQueue);
			var device = new AmtDevice(mLocalDevice, mQuery, mDeviceEntrypoint, queue);
            var physicalDevice = new AmtPhysicalDevice(device, mFormatLookup);
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
