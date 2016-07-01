namespace Magnesium
{
	public interface IMgTextureGenerator
	{
		MgTextureInfo Load(byte[] imageData, MgImageSource source, MgAllocationCallbacks allocator, IMgFence fence);
	}

}

