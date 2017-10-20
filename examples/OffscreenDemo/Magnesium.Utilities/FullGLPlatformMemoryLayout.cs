using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class FullGLPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private readonly Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgPlatformMemoryProperties[] mCombinations;
        public FullGLPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>();
            mCombinations = new[] {
                new MgPlatformMemoryProperties{
                    Usage = (MgBufferUsageFlagBits) 0xffff,
                    SeparateBlockRequired = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                }
            };
        }

        public IReadOnlyDictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> PreTransforms
        {
            get
            {
                return mPreTransforms;
            }
        }

        public MgPlatformMemoryProperties[] Combinations
        {
            get
            {
                return mCombinations;
            }
        }
    }
}
