using System;
namespace Magnesium.Toolkit.Validation.Device
{
	public class GetQueryPoolResults
	{
		public static void Validate(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IntPtr dataSize, IntPtr pData, UInt64 stride, MgQueryResultFlagBits flags)
		{
            if (queryPool == null)
                throw new ArgumentNullException(nameof(queryPool));
        }
	}
}
