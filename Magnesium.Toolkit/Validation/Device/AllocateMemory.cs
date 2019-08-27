using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class AllocateMemory
	{
		public static void Validate(MgMemoryAllocateInfo pAllocateInfo, IMgAllocationCallbacks allocator)
		{
            if (pAllocateInfo == null)
                throw new ArgumentNullException(nameof(pAllocateInfo));
        }
	}
}
