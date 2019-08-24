using System;

namespace Magnesium
{
    public class MgObjectTableCreateInfoEntryNVX
    {
        public UInt32 ObjectEntryCount { get; set; }
        public MgObjectEntryTypeNVX ObjectEntryType { get; set; }
        public MgObjectEntryUsageFlagBitsNVX UsageFlag { get; set; }
    }
}