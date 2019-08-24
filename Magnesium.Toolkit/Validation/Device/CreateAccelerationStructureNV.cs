using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateAccelerationStructureNV
	{
		public static void Validate(MgAccelerationStructureCreateInfoNV pCreateInfo, IMgAllocationCallbacks pAllocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.Info == null)
                throw new ArgumentNullException(nameof(pCreateInfo.Info));
        }
	}
}
