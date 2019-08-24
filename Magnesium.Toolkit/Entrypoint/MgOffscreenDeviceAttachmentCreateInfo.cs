namespace Magnesium
{
    public class MgOffscreenDeviceAttachmentCreateInfo
    {
        public MgFormat Format { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public MgImageUsageFlagBits Usage { get; set; }
        public MgImageLayout ImageLayout { get; set; }
        public MgImageAspectFlagBits AspectMask { get; set; }
    }
}
