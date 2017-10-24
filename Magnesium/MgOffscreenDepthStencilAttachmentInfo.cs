namespace Magnesium
{
    public class MgOffscreenDepthStencilAttachmentInfo
    {
        public IMgImageView View { get; set; }
        public MgImageLayout Layout { get; set; }
        public MgFormat Format { get; set; }
        public MgAttachmentLoadOp LoadOp { get; set; }
        public MgAttachmentStoreOp StoreOp { get; set; }
        public MgAttachmentLoadOp StencilLoadOp { get; set; }
        public MgAttachmentStoreOp StencilStoreOp { get; set; }
    }
}
