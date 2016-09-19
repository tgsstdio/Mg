namespace Magnesium.OpenGL
{
	public interface IGLFramebufferHelper
	{
		bool SupportsInvalidateFramebuffer { get;  }

		bool SupportsBlitFramebuffer { get; }

		void GenRenderbuffer (out int renderbuffer);

		void BindRenderbuffer (int renderbuffer);

		void DeleteRenderbuffer (int renderbuffer);

		void RenderbufferStorageMultisample (int samples, int internalFormat, int width, int height);

		void GenFramebuffer (out int framebuffer);

		void BindFramebuffer (int framebuffer);

		void BindReadFramebuffer (int readFramebuffer);

		void InvalidateDrawFramebuffer ();

		void InvalidateReadFramebuffer ();

		void DeleteFramebuffer (int framebuffer);

		void FramebufferTexture2D (int attachement, int target, int texture, int level = 0, int samples = 0);

	 	void FramebufferRenderbuffer (int attachement, int renderbuffer, int level = 0);

		void GenerateMipmap (int target);

		void BlitFramebuffer (int iColorAttachment, int width, int height);

		void CheckFramebufferStatus ();
	}
}

