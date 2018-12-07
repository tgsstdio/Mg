using System;

namespace Magnesium
{
    public class MgObjectTableEntryNVX
    {
        public MgObjectEntryTypeNVX ObjectEntryType { get; set; }
        public UInt32 ObjectEntryCount { get; set; }
        public MgObjectEntryUsageFlagBitsNVX UsageFlag { get; set; }
    }
}
