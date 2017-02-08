using Magnesium.OpenGL.Internals;
using System;

namespace Magnesium.OpenGL
{
	public class GLCmdRenderer : IGLRenderer
	{
        private readonly IGLBlitOperationEntrypoint mBlit;
        private readonly IGLCmdStateRenderer mStateRenderer;
        public GLCmdRenderer
        (
            IGLBlitOperationEntrypoint blit,            
            IGLCmdStateRenderer stateRenderer
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
