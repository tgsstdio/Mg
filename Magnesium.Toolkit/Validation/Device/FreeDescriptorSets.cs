using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class FreeDescriptorSets
	{
		public static void Validate(IMgDescriptorPool descriptorPool, IMgDescriptorSet[] pDescriptorSets)
		{
            if (descriptorPool == null)
                throw new ArgumentNullException(nameof(descriptorPool));

            if (pDescriptorSets == null)
                throw new ArgumentNullException(nameof(pDescriptorSets));

            var descriptorSetCount = (uint)pDescriptorSets.Length;
            if (descriptorSetCount == 0)
                throw new ArgumentOutOfRangeException(nameof(pDescriptorSets.Length) + " == 0");
        }
	}
}
