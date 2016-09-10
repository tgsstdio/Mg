namespace Magnesium.OpenGL
{
	public interface IGLImageFormatEntrypoint
	{
		GLInternalImageFormat GetGLFormat (MgFormat format, bool supportsSRgb);
	}

}

