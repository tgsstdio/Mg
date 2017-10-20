﻿using Magnesium;

namespace Magnesium.Utilities
{
    public class MgStorageBufferOffset
    {
        public uint Index { get; set; }
        public ulong Size { get; set; }
        public ulong Offset { get; set; }
        public MgBufferUsageFlagBits Usage { get; internal set; }
    }
}
