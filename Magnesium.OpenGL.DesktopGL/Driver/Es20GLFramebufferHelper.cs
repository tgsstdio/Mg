using System;
using OpenTK.Graphics.ES20;
using System.Diagnostics;

namespace Magnesium.OpenGL.DesktopGL
{
	public class Es20GLFramebufferHelper : IGLFramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		#region GL_EXT_discard_framebuffer

		internal const All AllColorExt = (All)0x1800;
		internal const All AllDepthExt = (All)0x1801;
		internal const All AllStencilExt = (All)0x1802;

		#endregion

		internal delegate void GLInvalidateFramebufferDelegate(All target, int numAttachments, All[] attachments);
		internal delegate void GLRenderbufferStorageMultisampleDelegate(All target, int samples, All internalFormat, int width, int height);
		internal delegate void GLBlitFramebufferDelegate(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, TextureMagFilter filter);
		internal delegate void GLFramebufferTexture2DMultisampleDelegate(All target, All attachment, All textarget, int texture, int level, int samples);

		internal GLInvalidateFramebufferDelegate GLInvalidateFramebuffer;
		internal GLRenderbufferStorageMultisampleDelegate GLRenderbufferStorageMultisample;
		internal GLFramebufferTexture2DMultisampleDelegate GLFramebufferTexture2DMultisample;
		internal GLBlitFramebufferDelegate GLBlitFramebuffer;

		internal All AllReadFramebuffer = All.Framebuffer;
		internal All AllDrawFramebuffer = All.Framebuffer;

		IGLErrorHandler mErrHandler;

		public Es20GLFramebufferHelper(IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			this.SupportsBlitFramebuffer = this.GLBlitFramebuffer != null;
		}

		public virtual void GenRenderbuffer(out int renderbuffer)
		{
			renderbuffer = 0;
			GL.GenRenderbuffers(1, out renderbuffer);

			mErrHandler.CheckGLError();
		}

		public virtual void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public virtual void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			mErrHandler.CheckGLError();
		}

		public virtual void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			if (samples > 0 && this.GLRenderbufferStorageMultisample != null)
				GLRenderbufferStorageMultisample(All.Renderbuffer, samples, (All)internalFormat, width, height);
			else
				GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, (RenderbufferInternalFormat)internalFormat, width, height);
			mErrHandler.CheckGLError();
		}

		public virtual void GenFramebuffer(out int framebuffer)
		{
			GL.GenFramebuffers(1, out framebuffer);

			mErrHandler.CheckGLError();
		}

		public virtual void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
			mErrHandler.CheckGLError();
		}

		public virtual void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer((FramebufferTarget)AllReadFramebuffer, readFramebuffer);
			mErrHandler.CheckGLError();
		}

		internal readonly All[] GLDiscardAttachementsDefault = { AllColorExt, AllDepthExt, AllStencilExt, };
		internal readonly All[] GLDiscardAttachements = { All.ColorAttachment0, All.DepthAttachment, All.StencilAttachment, };

		public void InvalidateDrawFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
			this.GLInvalidateFramebuffer(AllDrawFramebuffer, 3, GLDiscardAttachements);
		}

		public void InvalidateReadFramebuffer()
		{
			Debug.Assert(this.SupportsInvalidateFramebuffer);
			this.GLInvalidateFramebuffer(AllReadFramebuffer, 3, GLDiscardAttachements);
		}

		public void DeleteFramebuffer(int framebuffer)
		{
			GL.DeleteFramebuffers(1, ref framebuffer);
			mErrHandler.CheckGLError();
		}

		public void FramebufferTexture2D(int attachement, int target, int texture, int level = 0, int samples = 0)
		{
			if (samples > 0 && this.GLFramebufferTexture2DMultisample != null)
				this.GLFramebufferTexture2DMultisample(All.Framebuffer, (All)attachement, (All)target, texture, level, samples);
			else
				GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (All) attachement, (TextureTarget2d) target, texture, level);
			mErrHandler.CheckGLError();
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, (All)attachement, RenderbufferTarget.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((TextureTarget)target);
			mErrHandler.CheckGLError();
		}

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{
			this.GLBlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, TextureMagFilter.Nearest);
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
				case FramebufferErrorCode.FramebufferIncompleteDimensions: message = "Not all attached images have the same width and height."; break;
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: message = "No images are attached to the framebuffer."; break;
				case FramebufferErrorCode.FramebufferUnsupported: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break; 
				}
				throw new InvalidOperationException(message);
			}
		}
	}

}

