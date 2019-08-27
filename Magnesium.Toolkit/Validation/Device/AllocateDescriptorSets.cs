using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class AllocateDescriptorSets
	{
		public static void Validate(MgDescriptorSetAllocateInfo pAllocateInfo)
		{
            if (pAllocateInfo == null)
                throw new ArgumentNullException(nameof(pAllocateInfo));

            if (pAllocateInfo.SetLayouts == null)
                throw new ArgumentNullException(nameof(pAllocateInfo.SetLayouts));

            var descriptorSetCount = pAllocateInfo.DescriptorSetCount;
            if (descriptorSetCount != pAllocateInfo.SetLayouts.Length)
                throw new ArgumentOutOfRangeException(nameof(pAllocateInfo.DescriptorSetCount) + " must equal to " + nameof(pAllocateInfo.SetLayouts.Length));

        }
    }
}
