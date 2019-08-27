using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateBufferView
	{
		public static void Validate(MgBufferViewCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
