using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public static class BindAccelerationStructureMemoryNV
	{
		public static void Validate(MgBindAccelerationStructureMemoryInfoNV[] pBindInfos)
		{
            if (pBindInfos == null)
                throw new ArgumentNullException(nameof(pBindInfos));
        }
	}
}
