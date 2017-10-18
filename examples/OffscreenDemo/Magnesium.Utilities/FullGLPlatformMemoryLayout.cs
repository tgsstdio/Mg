using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class FullGLPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private readonly Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgMemoryCombination[] mCombinations;
        public FullGLPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>();
            mCombinations = new[] {
                new MgMemoryCombination{
                    Usage = (MgBufferUsageFlagBits) 0xffff,
                    SeparateMemoryRequired = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
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

        public MgMemoryCombination[] Combinations
        {
            get
            {
                return mCombinations;
            }
        }
    }
}
