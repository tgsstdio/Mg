namespace Magnesium.OpenGL
{
	public interface IGLExtensionLookup
	{
		void Initialize();

		bool HasExtension (string extension);
	}
}

