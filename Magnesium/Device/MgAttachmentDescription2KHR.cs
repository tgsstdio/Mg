namespace Magnesium
{
    public class MgAttachmentDescription2KHR
	{
		public MgAttachmentDescriptionFlagBits Flags { get; set; }
		public MgFormat Format { get; set; }
		public MgSampleCountFlagBits Samples { get; set; }
		///
		/// Load operation for color or depth data
		///
		public MgAttachmentLoadOp LoadOp { get; set; }
		///
		/// Store operation for color or depth data
		///
		public MgAttachmentStoreOp StoreOp { get; set; }
		///
		/// Load operation for stencil data
		///
		public MgAttachmentLoadOp StencilLoadOp { get; set; }
		///
		/// Store operation for stencil data
		///
		public MgAttachmentStoreOp StencilStoreOp { get; set; }
		public MgImageLayout InitialLayout { get; set; }
		public MgImageLayout FinalLayout { get; set; }
	}
}
