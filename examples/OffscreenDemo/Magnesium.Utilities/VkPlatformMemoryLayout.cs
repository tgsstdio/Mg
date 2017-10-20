using System.Collections.Generic;
using Magnesium;

namespace Magnesium.Utilities
{
    public class VkPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private readonly Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgPlatformMemoryProperties[] mCombinations;
        public VkPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>();
            mCombinations = new[] {
                new MgPlatformMemoryProperties{ Usage = (MgBufferUsageFlagBits) 0xffff, SeparateBlockRequired = 0 }
            };
        }

        public IReadOnlyDictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> PreTransforms
        {
            get
            {
                return mPreTransforms;
            }
        }

        public MgPlatformMemoryProperties[] Combinations {
            get
            {
                return mCombinations;
            }
        }
    }
}