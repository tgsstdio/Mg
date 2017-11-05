using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class FullGLFramebufferHelper : IGLFramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		IGLErrorHandler mErrHandler;

		public FullGLFramebufferHelper(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			this.SupportsBlitFramebuffer = true;
			this.SupportsInvalidateFramebuffer = false;
		}

		public void GenRenderbuffer(out int renderbuffer)
		{
			GL.GenRenderbuffers(1, out renderbuffer);
			mErrHandler.LogGLError("GenRenderbuffer");
		}

		public void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.LogGLError("BindRenderbuffer");
		}

		public void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			mErrHandler.LogGLError("DeleteRenderbuffer");
		}

		public void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, (RenderbufferStorage)internalFormat, width, height);
			mErrHandler.LogGLError("RenderbufferStorageMultisample");
		}

		public void GenFramebuffer(out int framebuffer)
		{
			GL.GenFramebuffers(1, out framebuffer);
			mErrHandler.LogGLError("GenFramebuffer");
		}

		public void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
			mErrHandler.LogGLError("BindFramebuffer");
		}

		public void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, readFramebuffer);
			mErrHandler.LogGLError("BindReadFramebuffer");
		}

		public void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public void DeleteFramebuffer(int framebuffer)
		{
			GL.DeleteFramebuffers(1, ref framebuffer);
			mErrHandler.LogGLError("DeleteFramebuffer");
		}

        private int TranslateViewType(MgImageViewType target)
        {
            switch (target)
            {
                case MgImageViewType.TYPE_1D:
                    return (int)All.Texture1D;
                case MgImageViewType.TYPE_2D:
                    return (int)All.Texture2D;
                default:
                    throw new NotSupportedException();
            }
        }

        public void FramebufferColorAttachment(int attachement, MgImageViewType target, int texture, int level = 0, int samples = 0)
        {
            var baseAttachment = (int)(All.ColorAttachment0 + attachement);
            FramebufferTexture2D(baseAttachment, TranslateViewType(target), texture, level, samples);
        }

        public void FramebufferDepthStencil(MgImageViewType target, int texture, int level = 0, int samples = 0)
        {
            const int GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
            FramebufferTexture2D(GL_DEPTH_STENCIL_ATTACHMENT, TranslateViewType(target), texture, level, samples);
        }

        private void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, (TextureTarget)target, texture, level);
			mErrHandler.LogGLError("FramebufferTexture2D");
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.LogGLError("FramebufferRenderbuffer");
		}

		public void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((GenerateMipmapTarget)target);
			mErrHandler.LogGLError("GenerateMipmap");
        }

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{
			GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.LogGLError("BlitFramebuffer.ReadBuffer");
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.LogGLError("BlitFramebuffer.DrawBuffer");
			GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            mErrHandler.LogGLError("BlitFramebuffer.BlitFramebuffer");
		}

		public void CheckFramebufferStatus()
		{
			var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
			if (status != FramebufferErrorCode.FramebufferComplete)
			{
				string message = "Framebuffer Incomplete.";
				switch (status)
				{
				case FramebufferErrorCode.FramebufferIncompleteAttachment: message = "Not all framebuffer attachment points are framebuffer attachment complete."; break;
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: message = "No images are attached to the framebuffer."; break;
				case FramebufferErrorCode.FramebufferUnsupported: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break;
				case FramebufferErrorCode.FramebufferIncompleteMultisample: message = "Not all attached images have the same number of samples."; break;
				}
				throw new InvalidOperationException(message);
			}
		}
	}
}

