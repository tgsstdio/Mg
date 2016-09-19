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
			mErrHandler.CheckGLError();
		}

		public void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, (RenderbufferStorage)internalFormat, width, height);
			mErrHandler.CheckGLError();
		}

		public void GenFramebuffer(out int framebuffer)
		{
			GL.GenFramebuffers(1, out framebuffer);
			mErrHandler.CheckGLError();
		}

		public void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
			mErrHandler.CheckGLError();
		}

		public void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, readFramebuffer);
			mErrHandler.CheckGLError();
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
			mErrHandler.CheckGLError();
		}

		public void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, (TextureTarget)target, texture, level);
			mErrHandler.CheckGLError();
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (FramebufferAttachment)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((GenerateMipmapTarget)target);
			mErrHandler.CheckGLError();
		}

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{

			GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.CheckGLError();
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.CheckGLError();

			GL.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

			mErrHandler.CheckGLError();

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

