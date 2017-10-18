using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium;

namespace Magnesium.Utilities
{
    public class WGLPlatformMemoryLayout : IMgPlatformMemoryLayout
    {
        private Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits> mPreTransforms;
        private MgMemoryCombination[] mCombinations;

        public WGLPlatformMemoryLayout()
        {
            mPreTransforms = new Dictionary<MgBufferUsageFlagBits, MgBufferUsageFlagBits>
            {
                {
                    MgBufferUsageFlagBits.STORAGE_BUFFER_BIT,
                    MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                },
                {
                    MgBufferUsageFlagBits.STORAGE_BUFFER_BIT | MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
                    MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                }
            };

            mCombinations = new []
            {
                new MgMemoryCombination
                {
                    Usage = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                    SeparateMemoryRequired = MgBufferUsageFlagBits.INDEX_BUFFER_BIT,
                },
                new MgMemoryCombination
                {
                    Usage = MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT,
                    SeparateMemoryRequired = (MgBufferUsageFlagBits) 0,
                },
                new MgMemoryCombination
                {
                    Usage = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                    | MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                    | MgBufferUsageFlagBits.STORAGE_TEXEL_BUFFER_BIT
                    | MgBufferUsageFlagBits.TRANSFER_DST_BIT
                    | MgBufferUsageFlagBits.TRANSFER_SRC_BIT 
                    | MgBufferUsageFlagBits.UNIFORM_TEXEL_BUFFER_BIT
                    , SeparateMemoryRequired = MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT,
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

        public MgMemoryCombination[] Combinations
        {
            get
            {
                return mCombinations;
            }
        }
    }
}
