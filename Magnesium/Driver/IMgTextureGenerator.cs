namespace Magnesium
{
	public interface IMgTextureGenerator
	{
		MgTextureInfo Load(byte[] imageData, MgImageSource source, IMgAllocationCallbacks allocator, IMgFence fence);
	}

}

