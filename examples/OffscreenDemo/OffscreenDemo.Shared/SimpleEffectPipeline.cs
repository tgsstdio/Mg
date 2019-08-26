using Magnesium;
using Magnesium.Toolkit;
using System;
using System.Diagnostics;

namespace OffscreenDemo
{
    public class SimpleEffectPipeline : IDisposable
    {
        public IMgPipelineSeed Seed { get; private set; }
        public SimpleEffectPipeline(IMgPipelineSeed seed)
        {
            Seed = seed;
        }

        public IMgDevice Device { get; private set; }
        public IMgEffectFramework Framework { get; private set; }
        public IMgDescriptorSetLayout DescriptorSetLayout { get; private set; }
        public IMgPipelineLayout PipelineLayout { get; private set; }
        public IMgPipeline Pipeline { get; private set; }

        public void Initialize(IMgGraphicsConfiguration configuration, IMgEffectFramework framework)
        {
            Device = configuration.Device;
            Debug.Assert(Device != null);
            Framework = framework;
            Debug.Assert(Framework != null);

            DescriptorSetLayout = Seed.SetupDescriptorSetLayout(Device);
            PipelineLayout = SetupPipelineLayout(Device, DescriptorSetLayout);
            Pipeline = Seed.BuildPipeline(Device, PipelineLayout, framework);
        }

        private static IMgPipelineLayout SetupPipelineLayout(IMgDevice device, IMgDescriptorSetLayout descSetLayout)
        {
            var pPipelineLayoutCreateInfo = new MgPipelineLayoutCreateInfo
            {
                SetLayouts = new IMgDescriptorSetLayout[]
                 {
                     descSetLayout,
                 }
            };

            var err = device.CreatePipelineLayout(pPipelineLayoutCreateInfo, null, out IMgPipelineLayout pipelineLayout);
            Debug.Assert(err == MgResult.SUCCESS);
            return pipelineLayout;
        }

        public IMgDescriptorSet[] Allocate( IMgDescriptorPool pool, uint count)
        {
            return AllocateDescriptorSet(Device, pool, DescriptorSetLayout, count);
        }

        public IMgDescriptorPool InitializePool()
        {
            return Seed.SetupDescriptorPool(Device);
        }

        private static IMgDescriptorSet[] AllocateDescriptorSet(IMgDevice device, IMgDescriptorPool pool, IMgDescriptorSetLayout setLayout, uint count)
        {
            var items = new IMgDescriptorSetLayout[count];

            for (var i = 0; i < count; i += 1)
            {
                items[i] = setLayout;
            }

            // Allocate a new descriptor set from the global descriptor pool
            var allocInfo = new MgDescriptorSetAllocateInfo
            {
                DescriptorPool = pool,
                DescriptorSetCount = 1,
                SetLayouts = items,
            };

            var err = device.AllocateDescriptorSets(allocInfo, out IMgDescriptorSet[] dSets);
            Debug.Assert(err == MgResult.SUCCESS);
            return dSets;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                ReleaseUnmanagedResources();

                disposedValue = true;
            }
        }

        private void ReleaseUnmanagedResources()
        {
            if (Device != null)
            {
                if (Pipeline != null)
                    Pipeline.DestroyPipeline(Device, null);

                if (PipelineLayout != null)
                    PipelineLayout.DestroyPipelineLayout(Device, null);

                if (DescriptorSetLayout != null)
                    DescriptorSetLayout.DestroyDescriptorSetLayout(Device, null);

                Device = null;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~SimpleEffectPipeline()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
