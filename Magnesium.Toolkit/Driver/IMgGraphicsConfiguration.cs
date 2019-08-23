using System;

namespace Magnesium.Toolkit
{
    public interface IMgGraphicsConfiguration : IDisposable
    {
        IMgDevice Device { get; }
        IMgThreadPartition Partition { get; }
        IMgQueue Queue { get; }

        void Initialize(uint width, uint height);
    }
}