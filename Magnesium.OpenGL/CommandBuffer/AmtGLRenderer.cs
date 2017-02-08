using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
	public class AmtGLRenderer : IGLRenderer
	{
        private readonly IGLBlitOperationEntrypoint mBlit;
        private readonly IAmtStateRenderer mStateRenderer;
        public AmtGLRenderer
        (
            IGLBlitOperationEntrypoint blit,            
            IAmtStateRenderer stateRenderer
        )
		{
            mBlit = blit;
            mStateRenderer = stateRenderer;
        }

        public void Initialize()
        {
            mStateRenderer.Initialize();
        }

    }

}
