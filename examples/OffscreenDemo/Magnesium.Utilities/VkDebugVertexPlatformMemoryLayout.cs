using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class VkDebugVertexPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgPlatformMemoryProperties[] mCombinations;

        public VkDebugVertexPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>();

            mCombinations = new[]
            {
                new MgPlatformMemoryProperties
                {
                    Usage = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT,
                    SeparateBlockRequired = MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT, 
                },
                new MgPlatformMemoryProperties
                {
                    Usage = 
                      MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_TEXEL_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT
                    | MgBufferUsageFlagBits.UNIFORM_TEXEL_BUFFER_BIT,
                    SeparateBlockRequired = MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                },
                new MgPlatformMemoryProperties
                {
                    Usage =
                    MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                    | MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_TEXEL_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT
                    | MgBufferUsageFlagBits.UNIFORM_TEXEL_BUFFER_BIT,
                    SeparateBlockRequired = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
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
