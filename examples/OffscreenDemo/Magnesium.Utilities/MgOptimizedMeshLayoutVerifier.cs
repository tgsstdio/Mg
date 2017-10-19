using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magnesium.Utilities
{
    public class MgOptimizedMeshSegmentVerifier : IMgOptimizedMeshSegmentVerifier
    {
        private MgBufferUsageFlagBits[] mPackingOrder;
        private IMgGraphicsConfiguration mConfiguration;
        public MgOptimizedMeshSegmentVerifier(IMgGraphicsConfiguration configuration)
        {
            mConfiguration = configuration;
            mPackingOrder = new MgBufferUsageFlagBits[]
            {
                MgBufferUsageFlagBits.INDEX_BUFFER_BIT
                , MgBufferUsageFlagBits.UNIFORM_BUFFER_BIT
                , MgBufferUsageFlagBits.INDIRECT_BUFFER_BIT
                , MgBufferUsageFlagBits.STORAGE_BUFFER_BIT
                , MgBufferUsageFlagBits.STORAGE_TEXEL_BUFFER_BIT
                , MgBufferUsageFlagBits.TRANSFER_DST_BIT
                , MgBufferUsageFlagBits.TRANSFER_SRC_BIT
                , MgBufferUsageFlagBits.UNIFORM_TEXEL_BUFFER_BIT
                , MgBufferUsageFlagBits.VERTEX_BUFFER_BIT
            };
        }

        public MgBufferInstance[] Revise(MgMeshSegment[] segments, MgOptimizedMeshCreateInfo createInfo)
        {
            var output = new List<MgBufferInstance>();
            foreach (var sector in segments)
            {
                if (DoPack(sector, createInfo, out MgBufferInstance firstOutput))
                {
                    output.Add(firstOutput);
                }
                else
                {
                    OntoSecondChance(output, sector, createInfo);
                }
            }

            return output.ToArray();
        }

        private void OntoSecondChance(List<MgBufferInstance> output, MgMeshSegment sector, MgOptimizedMeshCreateInfo createInfo)
        {
            MgMeshSegment[] slices = SubdivideByMemoryPropertyFlags(sector, createInfo.Allocations);

            foreach (var second in slices)
            {
                if (DoPack(second, createInfo, out MgBufferInstance secondOutput))
                {
                    output.Add(secondOutput);
                }
                else
                {
                    OntoFinalChance(output, second, createInfo);
                }
            }
        }

        private void OntoFinalChance(List<MgBufferInstance> output, MgMeshSegment second, MgOptimizedMeshCreateInfo createInfo)
        {
            var individuals = SplitToSingleBuffers(second);

            foreach (var single in individuals)
            {
                if (DoPack(single, createInfo, out MgBufferInstance thirdOutput))
                {
                    output.Add(thirdOutput);
                }
                else
                {
                    throw new InvalidOperationException("Cannot create buffer for MgOptimizedMesh");
                }
            }
        }

        private MgMeshSegment[] SplitToSingleBuffers(MgMeshSegment second)
        {
            var slices = new List<MgMeshSegment>();
            foreach(var attr in second.Attributes)
            {
                var slice = new MgMeshSegment
                {
                    Usage = attr.Usage,
                    Attributes = new List<MgMeshSegmentAttribute>(),
                };
                slice.Attributes.Add(attr);
                slices.Add(slice);
            }
            return slices.ToArray();
        }

        private MgMeshSegment[] SubdivideByMemoryPropertyFlags(MgMeshSegment sector, MgBlockAllocationInfo[] allocations)
        {
            // EXACT MATCH ONLY 
            var groups = new Dictionary<MgMemoryPropertyFlagBits, MgMeshSegment>();    
            foreach (var attr in sector.Attributes)
            {
                var allocation = allocations[attr.Index];
                MgMeshSegment found;
                if (!groups.TryGetValue(allocation.MemoryPropertyFlags, out found))
                {
                    found = new MgMeshSegment
                    {
                        Usage = attr.Usage,
                        Attributes = new List<MgMeshSegmentAttribute>(),
                    };
                    groups.Add(allocation.MemoryPropertyFlags, found);
                }
                found.Attributes.Add(attr);
            }

            var noOfGroups = groups.Keys.Count;
            var segments = new MgMeshSegment[noOfGroups];
            groups.Values.CopyTo(segments, 0);
            return segments;
        }

        private bool DoPack(MgMeshSegment sector, MgOptimizedMeshCreateInfo createInfo, out MgBufferInstance instance)
        {
            MgBufferUsageFlagBits bufferUsage = 0;
            MgMemoryPropertyFlagBits deviceMemoryProperties = 0;

            var sortedList = new SortedDictionary<SortedKey, uint>();
            foreach (var attr in sector.Attributes)
            {
                bufferUsage |= attr.Usage;
                deviceMemoryProperties |= createInfo.Allocations[attr.Index].MemoryPropertyFlags;

                var firstGroup = 0U;
                for(var i = 0U; i < mPackingOrder.Length; i += 1)
                {
                    firstGroup = i;
                    var first = (mPackingOrder[i] & createInfo.Allocations[attr.Index].Usage) > 0;
                    if (first)
                    {          
                        break;
                    }
                }

                Debug.Assert(firstGroup <= mPackingOrder.Length);

                var key = new SortedKey
                {
                    Group = firstGroup,
                    SubOrder = attr.Index,
                };
                sortedList.Add(key, attr.Index);
            }          

            ulong overallSize = 0;
            var mappings = new List<MgBufferOffsetMapping>();
            foreach(var element in sortedList.Values)
            {
                var attr = createInfo.Allocations[element];

                var mapping = new MgBufferOffsetMapping
                {
                    Index = element,
                    Offset = UpperBounded(overallSize, attr.ElementByteSize),
                    Size = attr.Size,
                    Usage = attr.Usage,
                };
                mappings.Add(mapping);

                overallSize = mapping.Offset + mapping.Size;
            }

            var pCreateInfo = new MgBufferCreateInfo
            {
                SharingMode = createInfo.SharingMode,
                QueueFamilyIndices = createInfo.QueueFamilyIndices,
                Size = overallSize,
                Usage = bufferUsage,                
            };

            var err = mConfiguration.Device.CreateBuffer(pCreateInfo, null, out IMgBuffer pBuffer);
            if (err != Result.SUCCESS)
            {
                instance = null;
                return false;
            }

            mConfiguration.Device.GetBufferMemoryRequirements(pBuffer, out MgMemoryRequirements pMemReqs);

            if (mConfiguration.MemoryProperties.GetMemoryType(pMemReqs.MemoryTypeBits, deviceMemoryProperties, out uint typeIndex))
            {
                instance = new MgBufferInstance
                {
                    Buffer = pBuffer,
                    Usage = bufferUsage,
                    MemoryPropertyFlags = deviceMemoryProperties,
                    TypeIndex = typeIndex,
                    AllocationSize = pMemReqs.Size,
                    Mappings = mappings.ToArray(),
                };
                return true;
            }
            else
            {
                // CLEAN UP
                pBuffer.DestroyBuffer(mConfiguration.Device, null);
                instance = null;
                return false;
            }
        }

        public static ulong UpperBounded(ulong value, ulong multiple)
        {
            var remainder = value % multiple;

            if (remainder == 0)
            {
                return value;
            }
            else
            {
                return value + (multiple - remainder);
            }
        }

        private class SortedKey : IComparable<SortedKey>
        {
            public uint Group { get; set; }
            public uint SubOrder { get; set; }

            public int CompareTo(SortedKey other)
            {
                if (Group < other.Group)
                {
                    return -1;
                }
                else if (Group > other.Group)
                {
                    return 1;
                }
                else if (SubOrder < other.SubOrder)
                {
                    return -1;
                }
                else if (SubOrder > other.SubOrder)
                {
                    return 1;
                }
                else
                {
                    return 0;
                } 

            }
        }
    }
}
