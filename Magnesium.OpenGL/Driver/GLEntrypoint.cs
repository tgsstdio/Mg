using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
	public class GLEntrypoint : IMgEntrypoint
	{
		private readonly IGLQueue mQueue;
		readonly IGLDeviceEntrypoint mEntrypoint;
        readonly IGLPhysicalDeviceFormatLookupEntrypoint mFormatLookup;
        private readonly IGLDeviceMemoryTypeMap mDeviceMemoryMap;
        public GLEntrypoint(IGLQueue queue, IGLDeviceEntrypoint entrypoint, IGLPhysicalDeviceFormatLookupEntrypoint formatLookup, IGLDeviceMemoryTypeMap deviceMemoryMap)
		{
			mQueue = queue;
			mEntrypoint = entrypoint;
            mFormatLookup = formatLookup;
            mDeviceMemoryMap = deviceMemoryMap;
		}

		#region IMgEntrypoint implementation

		public Result CreateInstance (MgInstanceCreateInfo createInfo, IMgAllocationCallbacks allocator, out IMgInstance instance)
		{
			instance = new GLInstance (mQueue, mEntrypoint, mFormatLookup, mDeviceMemoryMap);
			return Result.SUCCESS;
		}

		public Result EnumerateInstanceLayerProperties (out MgLayerProperties[] properties)
		{
			throw new NotImplementedException ();
		}

		public Result EnumerateInstanceExtensionProperties (string layerName, out MgExtensionProperties[] pProperties)
		{
            pProperties = new MgExtensionProperties[] { };
            return Result.SUCCESS;
		}

		public IMgAllocationCallbacks CreateAllocationCallbacks()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

