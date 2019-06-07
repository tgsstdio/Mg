using System;
namespace Magnesium.Toolkit.Validation.CommandBuffer
{
	public class CmdCopyQueryPoolResults
	{
		public static void Validate(IMgQueryPool queryPool, UInt32 firstQuery, UInt32 queryCount, IMgBuffer dstBuffer, UInt64 dstOffset, UInt64 stride, MgQueryResultFlagBits flags)
		{
			// TODO: add validation
		}
	}
}
