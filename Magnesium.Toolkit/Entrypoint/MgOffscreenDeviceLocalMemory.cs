using System.Diagnostics;

namespace Magnesium
{
    public class MgOffscreenDeviceLocalMemory : IMgOffscreenDeviceLocalMemory
    {
        private IMgGraphicsConfiguration mConfiguration;
        private IMgDeviceMemory mOffscreenMemory;

        public MgOffscreenDeviceLocalMemory(IMgGraphicsConfiguration configuration)
        {
            mConfiguration = configuration;
        }

        public void FreeMemory()
        {
            if (mOffscreenMemory != null)
                mOffscreenMemory.FreeMemory(mConfiguration.Device, null);
        }

        public void Initialize(IMgImage offscreenImage)
        {
            Result err;
            mConfiguration.Device.GetImageMemoryRequirements(offscreenImage, out MgMemoryRequirements memReqs);

            bool isValidMemoryType = mConfiguration.MemoryProperties.GetMemoryType(
                memReqs.MemoryTypeBits,
                MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT,
                out uint typeIndex);
            Debug.Assert(isValidMemoryType);
            var memAlloc = new MgMemoryAllocateInfo
            {
                AllocationSize = memReqs.Size,
                MemoryTypeIndex = typeIndex,
            };

            err = mConfiguration.Device.AllocateMemory(memAlloc, null, out IMgDeviceMemory memory);
            Debug.Assert(err == Result.SUCCESS);
            mOffscreenMemory = memory;
            err = offscreenImage.BindImageMemory(mConfiguration.Device, mOffscreenMemory, 0);
            Debug.Assert(err == Result.SUCCESS);
        }
    }
}
