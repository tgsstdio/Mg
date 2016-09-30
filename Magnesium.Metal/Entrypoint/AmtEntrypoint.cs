using System;
using Metal;

namespace Magnesium.Metal
{
	public class AmtEntrypoint : IMgEntrypoint
	{
		private IAmtDeviceQuery mQuery;
		public AmtEntrypoint(IAmtDeviceQuery query)
		{
			mQuery = query;
		}

		public IMgAllocationCallbacks CreateAllocationCallbacks()
		{
			throw new NotImplementedException();
		}

		public Result CreateInstance(MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
		{
			var queue = new AmtQueue();
			var device = new AmtDevice(MTLDevice.SystemDefault, mQuery, queue);
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
