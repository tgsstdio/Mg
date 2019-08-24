using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateBuffer
	{
		public static void Validate(MgBufferCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
