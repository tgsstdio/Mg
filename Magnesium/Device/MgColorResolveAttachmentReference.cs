using System;

namespace Magnesium
{
    public struct MgColorResolveAttachmentReference
    {
        public UInt32 Attachment { get; set; }
        public MgImageLayout Layout { get; set; }
        public MgImageAspectFlagBits AspectMask { get; set; }
    }
}
