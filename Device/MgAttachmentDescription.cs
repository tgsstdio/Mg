namespace Magnesium
{
    public class MgAttachmentDescription
	{
		public MgAttachmentDescriptionFlagBits Flags { get; set; }
		public MgFormat Format { get; set; }
		public MgSampleCountFlagBits Samples { get; set; }
		public MgAttachmentLoadOp LoadOp { get; set; }
		public MgAttachmentStoreOp StoreOp { get; set; }
		public MgAttachmentLoadOp StencilLoadOp { get; set; }
		public MgAttachmentStoreOp StencilStoreOp { get; set; }
		public MgImageLayout InitialLayout { get; set; }
		public MgImageLayout FinalLayout { get; set; }
	}
}

