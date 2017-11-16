namespace Magnesium.OpenGL
{
    public class GLLunarImageDescriptor
    {
        readonly IGLLunarImageDescriptorEntrypoint mImgDescriptor;
        public GLLunarImageDescriptor(IGLLunarImageDescriptorEntrypoint imgDescriptor)
        {
            mImgDescriptor = imgDescriptor;
        }

        public long? SamplerHandle { get; set; }

        public void Replace(long handle)
        {
            Destroy();
            SamplerHandle = handle;
        }

        public void Destroy()
        {
            if (SamplerHandle.HasValue)
            {
                mImgDescriptor.ReleaseHandle(SamplerHandle.Value);
                SamplerHandle = null;
            }
        }
    }
}
