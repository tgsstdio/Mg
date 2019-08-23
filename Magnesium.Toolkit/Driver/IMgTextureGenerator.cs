namespace Magnesium.Toolkit
{
	public interface IMgTextureGenerator
	{
		MgTextureInfo Load(byte[] imageData, MgImageSource source, IMgAllocationCallbacks allocator, IMgFence fence);
	}

}

