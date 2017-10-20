using Magnesium;

namespace Magnesium.Utilities
{
    public struct MgPlatformMemoryProperties
    {
        public MgBufferUsageFlagBits Usage { get; set; }
        public MgBufferUsageFlagBits SeparateBlockRequired { get; set; }
    }
}
