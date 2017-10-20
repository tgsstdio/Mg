using Magnesium;
using System.Collections.Generic;

namespace Magnesium.Utilities
{
    public class MgOptimizedStoragePartitioner : IMgOptimizedStoragePartitioner
    {
        private IMgPlatformMemoryLayout mLayout;
        public MgOptimizedStoragePartitioner(IMgPlatformMemoryLayout layout)
        {
            mLayout = layout;
        }

        public MgStorageBlockInfo[] Setup(MgStorageBlockAllocationInfo[] attributes)
        {
            var layouts = new List<MgStorageBlockInfo>();

            MgBufferUsageFlagBits?[] unallocatedAttributes = TransformAttributes(attributes);

            foreach (var combination in mLayout.Combinations)
            {
                MgStorageBlockInfo currentLayout = null;
                for (var i = 0U; i < unallocatedAttributes.Length; i += 1)
                {
                    var attr = unallocatedAttributes[i];

                    if (attr.HasValue)
                    {
                        var mask = (attr.Value & combination.Usage);
                        if (mask == attr.Value)
                        {
                            MgStorageBlockInfo destination = null;

                            if (currentLayout == null)
                            {
                                currentLayout = new MgStorageBlockInfo
                                {
                                    Usage = mask,
                                    Attributes = new List<MgStorageBlockAttribute>(),
                                };
                                layouts.Add(currentLayout);
                                destination = currentLayout;
                            }
                            else
                            {
                                // CREATE ADDITIONAL BUFFER IF NEEDED
                                if ((attr.Value & combination.SeparateBlockRequired) == attr.Value
                                    // IF CURRENT LAYOUT ALREADY HAS OCCUPIED, THEN CREATE NEW SEGMENT
                                    && (currentLayout.Usage & attr.Value) == attr.Value)
                                {
                                    var temp = new MgStorageBlockInfo
                                    {
                                        Usage = mask,
                                        Attributes = new List<MgStorageBlockAttribute>(),
                                    };
                                    layouts.Add(temp);
                                    destination = temp;
                                }
                                else
                                {
                                    // APPEND ATTRIBUTE
                                    currentLayout.Usage |= attr.Value;
                                    // currentBuffer.RequestedSize += attr.Size;
                                    destination = currentLayout;
                                }
                            }

                            var location = new MgStorageBlockAttribute
                            {
                                Index = i,
                                Usage = attr.Value,
                            };
                            destination.Attributes.Add(location);

                            // REMOVE attributes
                            unallocatedAttributes[i] = null;
                        }
                    }
                }
            }
            return layouts.ToArray();
        }

        private MgBufferUsageFlagBits?[] TransformAttributes(MgStorageBlockAllocationInfo[] attributes)
        {
            var unallocatedAttributes = new MgBufferUsageFlagBits?[attributes.Length];
            for (var i = 0; i < unallocatedAttributes.Length; i += 1)
            {
                MgBufferUsageFlagBits original = attributes[i].Usage;
                bool isFound = mLayout.PreTransforms.TryGetValue(
                    original,
                    out MgBufferUsageFlagBits updated);

                unallocatedAttributes[i] = isFound ? updated : original;
            }

            return unallocatedAttributes;
        }
    }
}
