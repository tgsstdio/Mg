using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Magnesium.OpenGL
{
    public class FullGLDeviceMemoryTypeMap : IGLDeviceMemoryTypeMap
    {
        private GLDeviceMemoryTypeInfo[] mMemoryTypes;
        public GLDeviceMemoryTypeInfo[] MemoryTypes
        {
            get
            {
                return mMemoryTypes;
            }
        }

        public FullGLDeviceMemoryTypeMap()
        {
            Initialize();
        }

        private void Initialize()
        {

            var entries = new List<GLDeviceMemoryTypeInfo>();

            PushIndirectEntries(entries);
            PushImageEntries(entries);
            PushTransferDstEntries(entries);
            PushTransferSrcEntries(entries);
            PushGeneralizedEntries(entries);

            mMemoryTypes = entries.ToArray();
        }

        private static void PushIndirectEntries(List<GLDeviceMemoryTypeInfo> entries)
        {
            const MgMemoryPropertyFlagBits ALL_ON =
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var offset = (uint) entries.Count;
            var info = new GLDeviceMemoryTypeInfo
            {
                Index = offset,
                IsHosted = true,
                MemoryTypeIndex = (uint) GLDeviceMemoryTypeFlagBits.INDIRECT,
                MemoryPropertyFlags = ALL_ON,
                Hint = 0,
            };

            entries.Add(info);
        }

        private static void PushImageEntries(List<GLDeviceMemoryTypeInfo> entries)
        {
            const MgMemoryPropertyFlagBits ALL_ON = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
              | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
              | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
              | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
              | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var offset = (uint)entries.Count;
            var info = new GLDeviceMemoryTypeInfo()
            {
                Index = offset,
                IsHosted = true,
                MemoryTypeIndex = (uint) GLDeviceMemoryTypeFlagBits.IMAGE,
                MemoryPropertyFlags = ALL_ON,
                Hint = 0,
            };
            entries.Add(info);
        }

        private static void PushTransferDstEntries(List<GLDeviceMemoryTypeInfo> entries)
        {
            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint) GLDeviceMemoryTypeFlagBits.TRANSFER_DST,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }

            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_DST,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }

            {
                const MgMemoryPropertyFlagBits ALL_ON =
                    MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                  | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                  | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_DST,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags = ALL_ON,

                    Hint = (uint)(BufferStorageFlags.MapCoherentBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }
        }

        private static void PushTransferSrcEntries(List<GLDeviceMemoryTypeInfo> entries)
        {
            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_SRC,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }

            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_SRC,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }

            {
                const MgMemoryPropertyFlagBits ALL_ON =
                    MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                  | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                  | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)GLDeviceMemoryTypeFlagBits.TRANSFER_SRC,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags = ALL_ON,

                    Hint = (uint)(BufferStorageFlags.MapCoherentBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit),
                };
                entries.Add(info);
            }
        }

        private static void PushGeneralizedEntries(List<GLDeviceMemoryTypeInfo> entries)
        {
            const GLDeviceMemoryTypeFlagBits SELECTED_TYPE_INDEX
                = GLDeviceMemoryTypeFlagBits.TRANSFER_DST
                | GLDeviceMemoryTypeFlagBits.TRANSFER_SRC
                | GLDeviceMemoryTypeFlagBits.UNIFORM
                | GLDeviceMemoryTypeFlagBits.VERTEX
                | GLDeviceMemoryTypeFlagBits.INDEX
                | GLDeviceMemoryTypeFlagBits.STORAGE;

            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapReadBit | BufferStorageFlags.MapWriteBit),
                };
                entries.Add(info);
            }

            {
                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags
                      = MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                      | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                    Hint = (uint)(BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit | BufferStorageFlags.MapWriteBit),
                };
                entries.Add(info);
            }

            {
                const MgMemoryPropertyFlagBits ALL_ON =
                    MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                  | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                  | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

                var offset = (uint)entries.Count;
                var info = new GLDeviceMemoryTypeInfo
                {
                    MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                    Index = offset,
                    IsHosted = false,

                    MemoryPropertyFlags = ALL_ON,

                    Hint = (uint)(BufferStorageFlags.MapCoherentBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapReadBit | BufferStorageFlags.MapWriteBit),
                };
                entries.Add(info);
            }     
        }

        public uint DetermineTypeIndex(GLDeviceMemoryTypeFlagBits category)
        {
            var mask = 0U;
            for (var i = 0; i < mMemoryTypes.Length; i += 1)
            {
                var entry = mMemoryTypes[i];
                if (
                    (
                        ((GLDeviceMemoryTypeFlagBits)entry.MemoryTypeIndex) & category
                    )
                    == category
                )
                {
                    mask |= (uint)(1 << i);
                }
            }
            return mask;
        }
    }
}
