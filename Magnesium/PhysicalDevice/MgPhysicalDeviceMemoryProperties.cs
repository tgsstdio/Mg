using System;

namespace Magnesium
{
    public class MgPhysicalDeviceMemoryProperties
	{
		public MgMemoryType[] MemoryTypes { get; set; }
		public MgMemoryHeap[] MemoryHeaps { get; set; }

        public bool GetMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex)
        {
            if (MemoryTypes == null)
            {
                typeIndex = 0;
                return false;
            }

            uint requirements = (uint)memoryPropertyFlags;

            // Search memtypes to find first index with those properties
            for (UInt32 i = 0; i < MemoryTypes.Length; i++)
            {
                if ((typeBits & 1) == 1)
                {
                    // Type is available, does it match user properties?
                    if ((MemoryTypes[i].PropertyFlags & requirements) == requirements)
                    {
                        typeIndex = i;
                        return true;
                    }
                }
                typeBits >>= 1;
            }
            // No memory types matched, return failure
            typeIndex = 0;
            return false;
        }
    }
}

