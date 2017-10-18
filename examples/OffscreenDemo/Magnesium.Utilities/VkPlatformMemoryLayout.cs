using System.Collections.Generic;
using Magnesium;

namespace Magnesium.Utilities
{
    public class VkPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private readonly Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgMemoryCombination[] mCombinations;
        public VkPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>();
            mCombinations = new[] {
                new MgMemoryCombination{ Usage = (MgBufferUsageFlagBits) 0xffff, SeparateMemoryRequired = 0 }
            };
        }

        public IReadOnlyDictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> PreTransforms
        {
            get
            {
                return mPreTransforms;
            }
        }

        public MgMemoryCombination[] Combinations {
            get
            {
                return mCombinations;
            }
        }
    }
}