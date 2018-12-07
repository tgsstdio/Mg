using Magnesium;
using System;
namespace Magnesium.Vulkan
{
	public class VkObjectTableNVX : IMgObjectTableNVX
	{
		internal UInt64 Handle = 0L;
		internal VkObjectTableNVX(UInt64 handle)
		{
			Handle = handle;
		}

		public void DestroyObjectTableNVX(IMgDevice device, IMgAllocationCallbacks pAllocator)
		{
		}

		public MgResult RegisterObjectsNVX(IMgDevice device, MgObjectRegistrationNVX[] registrationObjects)
		{
		}

		public MgResult UnregisterObjectsNVX(IMgDevice device, MgObjectRegistrationNVX[] registrationObjects)
		{
		}

	}
}
