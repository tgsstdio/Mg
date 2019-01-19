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

		public void DestroyObjectTableNVX(IMgDevice device, IMgAllocationCallbacks allocator)
		{
            if (device != null)
                throw new ArgumentNullException(nameof(device));

            var bDevice = (VkDevice)device;

            var allocatorHandle = VkInteropsUtility.GetAllocatorHandle(allocator);

            Interops.vkDestroyObjectTableNVX(bDevice.Handle, this.Handle, allocatorHandle);
		}

		public MgResult RegisterObjectsNVX(IMgDevice device, MgObjectTableEntryIndexNYX[] registrationObjects)
		{
            if (device != null)
                throw new ArgumentNullException(nameof(device));

            if (registrationObjects != null)
                throw new ArgumentNullException(nameof(registrationObjects));

            var bDevice = (VkDevice)device;

            var objectCount = (UInt32) registrationObjects.Length;

            var ppObjectTableEntries = new MgObjectTableEntryNVX[objectCount];
            var pObjectIndices = new UInt32[objectCount];

            for (var i = 0; i < objectCount; i += 1)
            {
                var current = registrationObjects[i];
                ppObjectTableEntries[i] = current.TableEntry;
                pObjectIndices[i] = current.Index;
            }

            return Interops.vkRegisterObjectsNVX(bDevice.Handle, this.Handle, objectCount, ref ppObjectTableEntries, pObjectIndices);
        }

		public MgResult UnregisterObjectsNVX(IMgDevice device, MgObjectTableEntryIndexNYX[] registrationObjects)
		{
            if (device != null)
                throw new ArgumentNullException(nameof(device));

            if (registrationObjects != null)
                throw new ArgumentNullException(nameof(registrationObjects));

            var bDevice = (VkDevice)device;

            var objectCount = (UInt32)registrationObjects.Length;

            var ppObjectTableEntries = new MgObjectTableEntryNVX[objectCount];
            var pObjectIndices = new UInt32[objectCount];

            for (var i = 0; i < objectCount; i += 1)
            {
                var current = registrationObjects[i];
                ppObjectTableEntries[i] = current.TableEntry;
                pObjectIndices[i] = current.Index;
            }

            return Interops.vkUnregisterObjectsNVX(bDevice.Handle, this.Handle, objectCount, ref ppObjectTableEntries, pObjectIndices);
        }

	}
}
