using System;
using OpenTK.Graphics.ES20;
using System.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Magnesium.OpenGL.AndroidGL
{
	public class AndroidGLFramebufferHelper : IGLFramebufferHelper
	{
		public bool SupportsInvalidateFramebuffer { get; private set; }

		public bool SupportsBlitFramebuffer { get; private set; }

		internal const string OpenGLLibrary = "libGLESv2.dll";
		[DllImport("libEGL.dll", EntryPoint = "eglGetProcAddress")]
		public static extern IntPtr EGLGetProcAddress(string funcname);

		#region GL_EXT_discard_framebuffer

		internal const All AllColorExt = (All)0x1800;
		internal const All AllDepthExt = (All)0x1801;
		internal const All AllStencilExt = (All)0x1802;

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glDiscardFramebufferEXT", ExactSpelling = true)]
		internal extern static void GLDiscardFramebufferExt(All target, int numAttachments, [MarshalAs(UnmanagedType.LPArray)] All[] attachments);

		#endregion

		#region GL_APPLE_framebuffer_multisample

		internal const All AllFramebufferIncompleteMultisampleApple = (All)0x8D56;
		internal const All AllMaxSamplesApple = (All)0x8D57;
		internal const All AllReadFramebufferApple = (All)0x8CA8;
		internal const All AllDrawFramebufferApple = (All)0x8CA9;
		internal const All AllRenderBufferSamplesApple = (All)0x8CAB;

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glRenderbufferStorageMultisampleAPPLE", ExactSpelling = true)]
		internal extern static void GLRenderbufferStorageMultisampleApple(All target, int samples, All internalformat, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(OpenGLLibrary, EntryPoint = "glResolveMultisampleFramebufferAPPLE", ExactSpelling = true)]
		internal extern static void GLResolveMultisampleFramebufferApple();

		internal void GLBlitFramebufferApple(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, TextureMagFilter filter)
		{
			GLResolveMultisampleFramebufferApple();
		}

		#endregion

		#region GL_NV_framebuffer_multisample

		internal const All AllFramebufferIncompleteMultisampleNV = (All)0x8D56;
		internal const All AllMaxSamplesNV = (All)0x8D57;
		internal const All AllReadFramebufferNV = (All)0x8CA8;
		internal const All AllDrawFramebufferNV = (All)0x8CA9;
		internal const All AllRenderBufferSamplesNV = (All)0x8CAB;

		#endregion

		#region GL_IMG_multisampled_render_to_texture

		internal const All AllFramebufferIncompleteMultisampleImg = (All)0x9134;
		internal const All AllMaxSamplesImg = (All)0x9135;

		#endregion

		#region GL_EXT_multisampled_render_to_texture

		internal const All AllFramebufferIncompleteMultisampleExt = (All)0x8D56;
		internal const All AllMaxSamplesExt = (All)0x8D57;

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

		private IGLErrorHandler mErrHandler;

		public AndroidGLFramebufferHelper(IGLExtensionLookup lookup, IGLErrorHandler errHandler)
		{
			mErrHandler = errHandler;
			// eglGetProcAddress doesn't guarantied returning NULL if the entry point doesn't exist. The returned address *should* be the same for all invalid entry point
			var invalidFuncPtr = EGLGetProcAddress("InvalidFunctionName");

			if (lookup.HasExtension("GL_EXT_discard_framebuffer"))
			{
				var glDiscardFramebufferEXTPtr = EGLGetProcAddress("glDiscardFramebufferEXT");
				if (glDiscardFramebufferEXTPtr != invalidFuncPtr)
				{
				this.GLInvalidateFramebuffer = Marshal.GetDelegateForFunctionPointer<GLInvalidateFramebufferDelegate>(glDiscardFramebufferEXTPtr);
				this.SupportsInvalidateFramebuffer = true;
				}
			}
			if (lookup.HasExtension("GL_EXT_multisampled_render_to_texture"))
			{
				var glRenderbufferStorageMultisampleEXTPtr = EGLGetProcAddress("glRenderbufferStorageMultisampleEXT");
				var glFramebufferTexture2DMultisampleEXTPtr = EGLGetProcAddress("glFramebufferTexture2DMultisampleEXT");
				if (glRenderbufferStorageMultisampleEXTPtr != invalidFuncPtr && glFramebufferTexture2DMultisampleEXTPtr != invalidFuncPtr)
				{
					this.GLRenderbufferStorageMultisample = Marshal.GetDelegateForFunctionPointer<GLRenderbufferStorageMultisampleDelegate>(glRenderbufferStorageMultisampleEXTPtr);
					this.GLFramebufferTexture2DMultisample = Marshal.GetDelegateForFunctionPointer<GLFramebufferTexture2DMultisampleDelegate>(glFramebufferTexture2DMultisampleEXTPtr);
				}
			}
			else if (lookup.HasExtension("GL_IMG_multisampled_render_to_texture"))
			{
				var glRenderbufferStorageMultisampleIMGPtr = EGLGetProcAddress("glRenderbufferStorageMultisampleIMG");
				var glFramebufferTexture2DMultisampleIMGPtr = EGLGetProcAddress("glFramebufferTexture2DMultisampleIMG");
				if (glRenderbufferStorageMultisampleIMGPtr != invalidFuncPtr && glFramebufferTexture2DMultisampleIMGPtr != invalidFuncPtr)
				{
					this.GLRenderbufferStorageMultisample = Marshal.GetDelegateForFunctionPointer<GLRenderbufferStorageMultisampleDelegate>(glRenderbufferStorageMultisampleIMGPtr);
					this.GLFramebufferTexture2DMultisample = Marshal.GetDelegateForFunctionPointer<GLFramebufferTexture2DMultisampleDelegate>(glFramebufferTexture2DMultisampleIMGPtr);
				}
			}
			else if (lookup.HasExtension("GL_NV_framebuffer_multisample"))
			{
				var glRenderbufferStorageMultisampleNVPtr = EGLGetProcAddress("glRenderbufferStorageMultisampleNV");
				var glBlitFramebufferNVPtr = EGLGetProcAddress("glBlitFramebufferNV");
				if (glRenderbufferStorageMultisampleNVPtr != invalidFuncPtr && glBlitFramebufferNVPtr != invalidFuncPtr)
				{
					this.GLRenderbufferStorageMultisample = Marshal.GetDelegateForFunctionPointer<GLRenderbufferStorageMultisampleDelegate>(glRenderbufferStorageMultisampleNVPtr);
					this.GLBlitFramebuffer = Marshal.GetDelegateForFunctionPointer<GLBlitFramebufferDelegate>(glBlitFramebufferNVPtr);
					this.AllReadFramebuffer = AllReadFramebufferNV;
					this.AllDrawFramebuffer = AllDrawFramebufferNV;
				}
			}

			this.SupportsBlitFramebuffer = this.GLBlitFramebuffer != null;
		}

		public void GenRenderbuffer(out int renderbuffer)
		{
			renderbuffer = 0;
			GL.GenRenderbuffers(1, ref renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void BindRenderbuffer(int renderbuffer)
		{
			GL.BindRenderbuffer(All.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void DeleteRenderbuffer(int renderbuffer)
		{
			GL.DeleteRenderbuffers(1, ref renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void RenderbufferStorageMultisample(int samples, int internalFormat, int width, int height)
		{
			if (samples > 0 && this.GLRenderbufferStorageMultisample != null)
				GLRenderbufferStorageMultisample(All.Renderbuffer, samples, (All)internalFormat, width, height);
			else
				GL.RenderbufferStorage(All.Renderbuffer, (All)internalFormat, width, height);
			mErrHandler.CheckGLError();
		}

		public void GenFramebuffer(out int framebuffer)
		{
			framebuffer = 0;
			GL.GenFramebuffers(1, ref framebuffer);
			mErrHandler.CheckGLError();
		}

		public void BindFramebuffer(int framebuffer)
		{
			GL.BindFramebuffer(All.Framebuffer, framebuffer);
			mErrHandler.CheckGLError();
		}

		public void BindReadFramebuffer(int readFramebuffer)
		{
			GL.BindFramebuffer((All)AllReadFramebuffer, readFramebuffer);
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
				GL.FramebufferTexture2D(All.Framebuffer, (All)attachement, (All)target, texture, level);
			mErrHandler.CheckGLError();
		}

		public void FramebufferRenderbuffer(int attachement, int renderbuffer, int level = 0)
		{
			GL.FramebufferRenderbuffer(All.Framebuffer, (All)attachement, All.Renderbuffer, renderbuffer);
			mErrHandler.CheckGLError();
		}

		public void GenerateMipmap(int target)
		{
			GL.GenerateMipmap((All)target);
			mErrHandler.CheckGLError();
		}

		public void BlitFramebuffer(int iColorAttachment, int width, int height)
		{
			this.GLBlitFramebuffer(0, 0, width, height, 0, 0, width, height, ClearBufferMask.ColorBufferBit, TextureMagFilter.Nearest);
			mErrHandler.CheckGLError();
		}

		public void CheckFramebufferStatus()
		{
			var status = GL.CheckFramebufferStatus(All.Framebuffer);
			if (status != All.FramebufferComplete)
			{
				string message = "Framebuffer Incomplete.";
				switch (status)
				{
				case All.FramebufferIncompleteAttachment: message = "Not all framebuffer attachment points are framebuffer attachment complete."; break;
				case All.FramebufferIncompleteDimensions: message = "Not all attached images have the same width and height."; break;
				case All.FramebufferIncompleteMissingAttachment: message = "No images are attached to the framebuffer."; break;
				case All.FramebufferUnsupported: message = "The combination of internal formats of the attached images violates an implementation-dependent set of restrictions."; break; 
				}
				throw new InvalidOperationException(message);
			}
		}
	}

}

