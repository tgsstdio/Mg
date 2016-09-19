namespace Magnesium.OpenGL
{
	public interface IGLShaderModuleEntrypoint
	{
		/// int CompileProgram (MgGraphicsPipelineCreateInfo info);

		int CreateShaderModule(MgShaderStageFlagBits stage);

		bool HasCompilerMessages(int module);

		void CompileShaderModule(int module, string sourceCode);

		bool IsCompiled(int module);

		void DeleteShaderModule (int module);
		string GetCompilerMessages(int module);
	}
}

