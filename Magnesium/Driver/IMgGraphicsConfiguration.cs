using System;

namespace Magnesium
{
    public interface IMgGraphicsConfiguration : IDisposable
    {
        IMgDevice Device { get; }
        IMgThreadPartition Partition { get; }
        IMgQueue Queue { get; }

        void Initialize(uint width, uint height);
    }
}