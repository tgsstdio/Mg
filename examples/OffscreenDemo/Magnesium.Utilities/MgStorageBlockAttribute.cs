using Magnesium;

namespace Magnesium.Utilities
{
    public class MgStorageBlockAttribute
    {
        public uint Index { get; set; }
        public MgBufferUsageFlagBits Usage { get; internal set; }
    }
}
