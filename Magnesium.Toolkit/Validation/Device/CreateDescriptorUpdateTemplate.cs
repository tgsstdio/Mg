using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class CreateDescriptorUpdateTemplate
	{
		public static void Validate(MgDescriptorUpdateTemplateCreateInfo pCreateInfo, IMgAllocationCallbacks pAllocator)
		{
            if (pCreateInfo == null)
                throw new ArgumentNullException(nameof(pCreateInfo));

            if (pCreateInfo.DescriptorUpdateEntries == null)
                throw new ArgumentNullException(nameof(pCreateInfo.DescriptorUpdateEntries));
        }
	}
}
