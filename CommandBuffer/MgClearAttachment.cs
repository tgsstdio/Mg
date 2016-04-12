using System;

namespace Magnesium
{
    public class MgClearAttachment
	{
		public MgImageAspectFlagBits AspectMask { get; set; }
		public UInt32 ColorAttachment { get; set; }
		public MgClearValue ClearValue { get; set; }
	}
}

