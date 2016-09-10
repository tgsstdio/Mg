namespace Magnesium.OpenGL
{
	public interface IGLDescriptorPool : IMgDescriptorPool
	{
		void Add (GLDescriptorSet localSet);

		bool TryTake (out GLDescriptorSet dSet);

		int NoOfSets {
			get;
		}		
	}
}

