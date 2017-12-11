using Magnesium;

namespace OffscreenDemo
{
    public class MgCommandBuildOrder
    {
        public IMgEffectFramework Framework { get; set; }
        public int First { get; set; }
        public int Count { get; set; }
        public IMgCommandBuffer[] CommandBuffers { get; set; }
        public IMgDescriptorSet[] DescriptorSets { get; set; }
        public MgCommandBuildOrderBufferInfo[] Vertices { get; set; }
        public MgCommandBuildOrderBufferInfo Indices { get; set; }
        public uint IndexCount { get; set; }
        public uint InstanceCount { get; internal set; }
    }
}