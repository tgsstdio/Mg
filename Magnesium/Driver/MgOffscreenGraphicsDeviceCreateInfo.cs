namespace Magnesium
{
    public class MgOffscreenGraphicsDeviceCreateInfo
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public MgOffscreenColorAttachmentInfo[] ColorAttachments { get; set; }
        public MgOffscreenDepthStencilAttachmentInfo DepthStencilAttachment { get; set; }
        public float MinDepth { get; set; }
        public float MaxDepth { get; set; }
    }
}
