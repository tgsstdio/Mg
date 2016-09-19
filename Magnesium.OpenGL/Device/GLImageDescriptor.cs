namespace Magnesium.OpenGL
{
	public class GLImageDescriptor
	{
		readonly IGLImageDescriptorEntrypoint mImgDescriptor;		
		public GLImageDescriptor (IGLImageDescriptorEntrypoint imgDescriptor)
		{
			mImgDescriptor = imgDescriptor;	
		}

		public ulong? SamplerHandle { get; set; }

		public void Replace (ulong handle)
		{
			Destroy ();
			SamplerHandle = handle;
		}

		public void Destroy ()
		{
			if (SamplerHandle.HasValue)
			{
				mImgDescriptor.ReleaseHandle (SamplerHandle.Value);
				SamplerHandle = null;
			}
		}
	}
}

