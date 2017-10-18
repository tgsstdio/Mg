using Magnesium;

namespace Magnesium.Utilities
{
    public class MgMeshSegmentAttribute
    {
        public uint Index { get; set; }
        public MgBufferUsageFlagBits Usage { get; internal set; }
    }
}
