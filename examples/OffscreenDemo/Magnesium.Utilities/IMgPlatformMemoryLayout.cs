using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public interface IMgPlatformMemoryLayout
    {
        IReadOnlyDictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> PreTransforms { get; }
        MgPlatformMemoryProperties[] Combinations { get; }
    }
}