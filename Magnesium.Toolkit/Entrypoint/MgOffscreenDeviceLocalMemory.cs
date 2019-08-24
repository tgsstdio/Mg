using System.Diagnostics;

namespace Magnesium.Toolkit
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

            MgResult err = mConfiguration.Device.AllocateMemory(memAlloc, null, out IMgDeviceMemory memory);
            Debug.Assert(err == MgResult.SUCCESS);
            mOffscreenMemory = memory;
            err = offscreenImage.BindImageMemory(mConfiguration.Device, mOffscreenMemory, 0);
            Debug.Assert(err == MgResult.SUCCESS);
        }
    }
}
