namespace Magnesium
{
    public class MgOffscreenColorAttachmentInfo
    {
        public IMgImageView View { get; set; }
        public MgFormat Format { get; set; }
        public MgAttachmentLoadOp LoadOp { get; set; }
        public MgAttachmentStoreOp StoreOp { get; set; }
    }
}
