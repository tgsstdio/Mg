namespace Magnesium.OpenGL
{
    public class GLDeviceMemoryTypeMap : IGLDeviceMemoryTypeMap
    {
        const uint STREAM_READ = 0x88E1U;
        const uint STREAM_COPY = 0x88E2U;
        const uint STATIC_READ = 0x88E5U;
        const uint STATIC_COPY = 0x88E6U;
        const uint DYNAMIC_READ = 0x88E9U;
        const uint DYNAMIC_COPY = 0x88EAU;
        const uint STREAM_DRAW = 0x88E0U;
        const uint STATIC_DRAW = 0x88E4U;
        const uint DYNAMIC_DRAW = 0x88E8U;

        private GLDeviceMemoryTypeInfo[] mMemoryTypes;
        public GLDeviceMemoryTypeInfo[] MemoryTypes
        {
            get
            {
                return mMemoryTypes;
            }
        }

        public GLDeviceMemoryTypeMap()
        {
            Initialize();
        }

        private void Initialize()
        {
            const uint MAX_NO_OF_ENTRIES = 11;
            mMemoryTypes = new GLDeviceMemoryTypeInfo[MAX_NO_OF_ENTRIES];

            var index = PushIndirectEntries(0U);
            index = PushImageEntries(index);
            index = PushIndexEntries(index);
            index = PushDrawEntries(index);
            index = PushReadEntries(index);
        }

        private uint PushIndirectEntries(uint offset)
        {
            const MgMemoryPropertyFlagBits ALL_ON =
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
                | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
                | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var info = new GLDeviceMemoryTypeInfo
            {
                Index = offset,
                IsHosted = true,
                MemoryTypeIndex = (uint) GLDeviceMemoryTypeFlagBits.INDIRECT,
                MemoryPropertyFlags = ALL_ON,
                Hint = 0,
            };

            mMemoryTypes[info.Index] = info;
            return offset + 1;
        }

        private uint PushImageEntries(uint offset)
        {
            const MgMemoryPropertyFlagBits ALL_ON = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
              | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
              | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
              | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
              | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var info = new GLDeviceMemoryTypeInfo()
            {
                Index = offset,
                IsHosted = true,
                MemoryTypeIndex = (uint) GLDeviceMemoryTypeFlagBits.IMAGE,
                MemoryPropertyFlags = ALL_ON,
                Hint = 0,
            };

            this.mMemoryTypes[info.Index] = info;

            return offset + 1;
        }

        private uint PushIndexEntries(uint offset)
        {
            const GLDeviceMemoryTypeFlagBits SELECTED_TYPE_INDEX
                = GLDeviceMemoryTypeFlagBits.INDEX
                | GLDeviceMemoryTypeFlagBits.TRANSFER_DST;

            const MgMemoryPropertyFlagBits ALL_ON =
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
              | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
              | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
              | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
              | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var info_stream = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                  = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STREAM_DRAW,
            };
            mMemoryTypes[info_stream.Index] = info_stream;

            offset += 1;

            var info_static = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                = MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STATIC_DRAW,
            };
            mMemoryTypes[info_static.Index] = info_static;

            offset += 1;

            var info_dynamic = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,
                MemoryPropertyFlags = ALL_ON,
                Hint = DYNAMIC_DRAW,
            };
            this.mMemoryTypes[info_dynamic.Index] = info_dynamic;

            return offset + 1;      
        }


        private uint PushDrawEntries(uint offset)
        {
            const GLDeviceMemoryTypeFlagBits SELECTED_TYPE_INDEX
                = GLDeviceMemoryTypeFlagBits.VERTEX
                | GLDeviceMemoryTypeFlagBits.UNIFORM
                | GLDeviceMemoryTypeFlagBits.TRANSFER_SRC;

            const MgMemoryPropertyFlagBits ALL_ON =
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
              | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
              | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
              | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
              | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var info_stream = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                  = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STREAM_DRAW,
            };
            mMemoryTypes[info_stream.Index] = info_stream;

            offset += 1;

            var info_static = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                = MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STATIC_DRAW,
            };
            mMemoryTypes[info_static.Index] = info_static;

            offset += 1;

            var info_dynamic = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,
                MemoryPropertyFlags = ALL_ON,
                Hint = DYNAMIC_DRAW,
            };
            this.mMemoryTypes[info_dynamic.Index] = info_dynamic;

            return offset + 1;
        }

        private uint PushReadEntries(uint offset)
        {
            const GLDeviceMemoryTypeFlagBits SELECTED_TYPE_INDEX
                = GLDeviceMemoryTypeFlagBits.VERTEX
                | GLDeviceMemoryTypeFlagBits.UNIFORM
                | GLDeviceMemoryTypeFlagBits.TRANSFER_SRC
                | GLDeviceMemoryTypeFlagBits.TRANSFER_DST;

            const MgMemoryPropertyFlagBits ALL_ON =
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
              | MgMemoryPropertyFlagBits.HOST_VISIBLE_BIT
              | MgMemoryPropertyFlagBits.HOST_COHERENT_BIT
              | MgMemoryPropertyFlagBits.HOST_CACHED_BIT
              | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT;

            var info_stream = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                  = MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT
                  | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STREAM_DRAW,
            };
            mMemoryTypes[info_stream.Index] = info_stream;

            offset += 1;

            var info_static = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,

                MemoryPropertyFlags
                = MgMemoryPropertyFlagBits.HOST_CACHED_BIT
                | MgMemoryPropertyFlagBits.LAZILY_ALLOCATED_BIT,

                Hint = STATIC_DRAW,
            };
            mMemoryTypes[info_static.Index] = info_static;

            offset += 1;

            var info_dynamic = new GLDeviceMemoryTypeInfo
            {
                MemoryTypeIndex = (uint)SELECTED_TYPE_INDEX,
                Index = offset,
                IsHosted = false,
                MemoryPropertyFlags = ALL_ON,
                Hint = DYNAMIC_DRAW,
            };
            this.mMemoryTypes[info_dynamic.Index] = info_dynamic;

            return offset + 1;
        }

        public uint DetermineTypeIndex(GLDeviceMemoryTypeFlagBits category)
        {
            var mask = 0U;
            for (var i = 0; i < mMemoryTypes.Length; i += 1)
            {
                var entry = mMemoryTypes[i];
                if (
                    (
                        ((GLDeviceMemoryTypeFlagBits) entry.MemoryTypeIndex) & category
                    )
                    == category
                )
                {
                    mask |= (uint) (1 << i);
                }
            }
            return mask;
        }
    }
}
