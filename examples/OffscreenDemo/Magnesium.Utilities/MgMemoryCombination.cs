using Magnesium;

namespace Magnesium.Utilities
{
    public struct MgMemoryCombination
    {
        public MgBufferUsageFlagBits Usage { get; set; }
        public MgBufferUsageFlagBits SeparateMemoryRequired { get; set; }
    }
}
