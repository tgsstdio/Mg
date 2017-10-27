using Metal;
using MetalKit;

namespace Magnesium.Metal
{
    public class AmtDepthStencilImageView : IAmtImageView
    {
        private MTKView mApplicationView;
        public AmtDepthStencilImageView(MTKView view)
        {
            mApplicationView = view;
        }

        public MgFormat Format { get; set; }

        public void DestroyImageView(IMgDevice device, IMgAllocationCallbacks allocator)
        {
            
        }

        public IMTLTexture GetTexture()
        {
            return mApplicationView.DepthStencilTexture;
        }
    }
}
