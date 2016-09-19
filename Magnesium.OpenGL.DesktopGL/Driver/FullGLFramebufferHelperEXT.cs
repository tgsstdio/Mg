using System;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public sealed class FullGLFramebufferHelperEXT : IGLFramebufferHelper
	{
		public void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
		}

		public bool SupportsInvalidateFramebuffer {
			get;
			private set;
		}

		public bool SupportsBlitFramebuffer {
			get;
			private set;
		}

		IGLErrorHandler mErrHandler;

		public FullGLFramebufferHelperEXT(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			this.SupportsBlitFramebuffer = true;
			this.SupportsInvalidateFramebuffer = false;			
		}

		public void GenRenderbuffer(out int id)
		{
			GL.Ext.GenRenderbuffers(1, out id);
			mErrHandler.CheckGLError();
		}

		public void BindRenderbuffer(int id)
		{
			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, id);
			mErrHandler.CheckGLError();
		}

		public void DeleteRenderbuffer(int id)
		{
			GL.Ext.DeleteRenderbuffers(1, ref id);
			mErrHandler.CheckGLError();
		}

		public void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			GL.Ext.RenderbufferStorageMultisample(RenderbufferTarget.RenderbufferExt, samples, (RenderbufferStorage)internalFormat, width, height);
			mErrHandler.CheckGLError();
		}

		public void GenFramebuffer(out int id)
		{
			GL.Ext.GenFramebuffers(1, out id);
			mErrHandler.CheckGLError();
		}

		public void BindFramebuffer(int id)
		{
			GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, id);
			mErrHandler.CheckGLError();
		}

		public void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, readFramebuffer);
			mErrHandler.CheckGLError();
		}

		public void DeleteFramebuffer(int id)
		{
			GL.Ext.DeleteFramebuffers(1, ref id);
			mErrHandler.CheckGLError();
		}

		public void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, (FramebufferAttachment)attachement, (TextureTarget)target, texture, level);
			mErrHandler.CheckGLError();
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, (FramebufferAttachment)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void GenerateMipmap(int target)
		{
			GL.Ext.GenerateMipmap((GenerateMipmapTarget)target);
			mErrHandler.CheckGLError();
		}

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{
			GL.ReadBuffer(ReadBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.CheckGLError();
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0 + iColorAttachment);
			mErrHandler.CheckGLError();
			GL.Ext.BlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
			mErrHandler.CheckGLError();
		}

		public void CheckFramebufferStatus()
		{
			var status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
			if (status != FramebufferErrorCode.FramebufferComplete)
			{
				string message = "Framebuffer Incomplete.";
				switch (status)
				{
				case FramebufferErrorCode.FramebufferIncompleteAttachmentExt: message = "Not all framebuffer attachment points are framebuffer attachment complete."; break;
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt: message = "No images are attached to the framebuffer."; break;
				case FramebufferErrorCode.FramebufferUnsupportedExt: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break;
				case FramebufferErrorCode.FramebufferIncompleteMultisample: message = "Not all attached images have the same number of samples."; break;
				}
				throw new InvalidOperationException(message);
			}
		}
	}
}

