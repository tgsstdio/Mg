using System.Diagnostics;
using Magnesium.Metal.Internals;
using MetalKit;

namespace Magnesium.Metal
{
    public class AmtGraphicsDeviceContext : IMgGraphicsDeviceContext
    {
        private readonly MTKView mApplicationView;
        public AmtGraphicsDeviceContext(MTKView view)
        {
            mApplicationView = view;
        }

        public void Initialize(MgGraphicsDeviceCreateInfo createInfo)
        {

        }

        public void ReleaseDepthStencil()
        {
            mDepthStencilView = null;
        }

        public void SetupContext(MgGraphicsDeviceCreateInfo createInfo, MgFormat colorPassFormat, MgFormat depthPassFormat)
        {
            var colorFormat = AmtFormatExtensions
                .GetPixelFormat(colorPassFormat);
            var depthFormat = AmtFormatExtensions
                .GetPixelFormat(depthPassFormat);
            var sampleCount = AmtSampleCountFlagBitExtensions
                .TranslateSampleCount(createInfo.Samples);
       
			mApplicationView.ColorPixelFormat = colorFormat;
			mApplicationView.DepthStencilPixelFormat = depthFormat;
			mApplicationView.SampleCount = sampleCount;

        }

        private IAmtImageView mDepthStencilView;
        public IMgImageView SetupDepthStencil(
            MgGraphicsDeviceCreateInfo createInfo,
            IMgCommandBuffer setupCmdBuffer,
            MgFormat depthPassFormat)
        {
            Debug.Assert(mApplicationView != null);

            var view = new AmtDepthStencilImageView(mApplicationView);
            view.Format = depthPassFormat;
            mDepthStencilView = view;
            return view;
        }
    }
}
