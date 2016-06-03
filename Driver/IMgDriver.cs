using System;

namespace Magnesium
{
	public interface IMgDriver : IDisposable
	{
		IMgInstance Instance { get; }
		void Initialize (MgApplicationInfo appInfo);
		void Initialize (MgApplicationInfo appInfo, string[] extensionLayers, string[] extensionNames);

		IMgLogicalDevice CreateGraphicsDevice ();
		IMgLogicalDevice CreateGraphicsDevice(IMgSurfaceKHR presentationSurface);
		IMgLogicalDevice CreateDevice(uint physicalDevice, IMgSurfaceKHR presentationSurface, MgQueueAllocation requestCount, MgQueueFlagBits requestedQueueType);
		IMgLogicalDevice CreateDevice(IMgPhysicalDevice gpu, MgDeviceQueueCreateInfo queueCreateInfo);
	}
}

