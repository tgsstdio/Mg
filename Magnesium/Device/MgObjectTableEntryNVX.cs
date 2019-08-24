using System;
using System.Runtime.InteropServices;

namespace Magnesium
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MgObjectTableEntryNVX
    {
        public MgObjectEntryTypeNVX Type { get; set; }
        public MgObjectEntryUsageFlagBitsNVX Flags { get; set; }
    }
}
