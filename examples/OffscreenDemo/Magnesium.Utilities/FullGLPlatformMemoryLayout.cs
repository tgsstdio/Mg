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
            mCombinations = new[]
            {
                new MgPlatformMemoryProperties
                {
                    Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                    SeparateBlockRequired = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                },
                new MgPlatformMemoryProperties
                {
                    Usage =
                    MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                    | MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_TEXEL_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT
                    | MgBufferUsageFlagBits.UNIFORM_TEXEL_BUFFER_BIT
                },
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
