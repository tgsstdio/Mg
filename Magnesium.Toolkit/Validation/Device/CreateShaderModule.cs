using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class CreateShaderModule
	{
		public static void Validate(MgShaderModuleCreateInfo pCreateInfo, IMgAllocationCallbacks allocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));
        }
	}
}
