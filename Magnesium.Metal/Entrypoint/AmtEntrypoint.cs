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
			var device = new AmtDevice(MTLDevice.SystemDefault, mQuery);
			var physicalDevice = new AmtPhysicalDevice(device);
			instance = new AmtInstance(physicalDevice);
			return Result.SUCCESS;
		}

		public Result EnumerateInstanceExtensionProperties(string layerName, out MgExtensionProperties[] pProperties)
		{
			throw new NotImplementedException();
		}

		public Result EnumerateInstanceLayerProperties(out MgLayerProperties[] properties)
		{
			throw new NotImplementedException();
		}
	}
}
