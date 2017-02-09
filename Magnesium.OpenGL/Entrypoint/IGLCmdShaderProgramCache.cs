namespace Magnesium.OpenGL
{
	public interface IGLCmdShaderProgramCache
	{
		byte DescriptorSetIndex {
			get;
			set;
		}

		int ProgramID { get; set; }
		int VBO { get; set; }

		void BindDescriptorSet();
	}

}

