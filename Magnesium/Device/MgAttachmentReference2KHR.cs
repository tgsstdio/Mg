using System;

namespace Magnesium
{
	public class MgAttachmentReference2KHR
	{
		public UInt32 Attachment { get; set; }
		public MgImageLayout Layout { get; set; }
		public MgImageAspectFlagBits AspectMask { get; set; }
	}
}
