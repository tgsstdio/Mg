using System;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    public interface IAmtDescriptorSetBindingCacheEntrypoint
    {

    }

    public class AmtDescriptorSetBindingCache
    {
        public int LastDescriptorSetKey { get; set; }

        // for binding new descriptor sets to cache
        public AmtDescriptorSetBindingCache(MgBufferUsageFlagBits target, uint noOfBindings)
        {
            mBindingIndices = new AmtBindingIndex[noOfBindings];
        }

        private readonly AmtBindingIndex[] mBindingIndices;
        public void Bind(GLDescriptorSet dSet)
        {
            if (LastDescriptorSetKey != dSet.Key)
            {
                foreach (var bindingIndex in mBindingIndices)
                {

                }

                LastDescriptorSetKey = dSet.Key;
            }
        }
    }

    public class AmtBindingIndex
    {
        public int GroupIndex { get; set; }
        public int ArrayIndex { get; set; }
        public int BindingIndex { get; set; }
        public int Buffer { get; set; }
        public int Offset { get; set; }
        public int Range { get; set; }
        public int Stride { get; set; }
    }

    public class AmtInternalPipelineLayout
    {
        public class AmtUniformBlockLayoutGroup
        {
            public string UniformPrefix { get; set; }
            public int ZeroBlockLocation { get; set; }
            public int Stride { get; set; }
            public uint DescriptorCount { get; set; }
        }

        public class AmtStorageBufferBindingIndex
        {
            public int Location { get; set; }
            public int BindingIndex { get; set; }
        }

        public AmtUniformBlockLayoutGroup[] Groups { get; private set; }
        public AmtInternalPipelineLayout(int programID, GLPipelineLayout layout, IGLPipelineCacheLayoutEntrypoint entrypoint)
        {
            Extract(programID, layout, entrypoint);
        }

        public void Extract(int programID, GLPipelineLayout layout, IGLPipelineCacheLayoutEntrypoint entrypoint)
        {
            // SCAN THE PROGRAM
                // FOR UNIFORM BLOCKS

            // FOR PIPELINE LAYOUT
            // EXTRACT BINDING OF SSBOs
            var prefixes = new Dictionary<string, AmtUniformBlockLayoutGroup>();
            foreach (var binding in layout.Bindings)
            {
                switch(binding.DescriptorType)
                {
                    case MgDescriptorType.UNIFORM_BUFFER:                        


                        break;
                    case MgDescriptorType.STORAGE_BUFFER:

                        break;
                }
            }

            // FOR EVERY UNIFORM BLOCK 
                // EXTRACT BINDING OF FIRST INDEX UNIFORM + DESCRIPTOR TYPE
        }
    }
}
