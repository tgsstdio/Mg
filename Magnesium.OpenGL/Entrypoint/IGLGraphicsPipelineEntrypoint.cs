namespace Magnesium.OpenGL
{
	public interface IGLGraphicsPipelineEntrypoint
	{
		void DeleteProgram (int programID);

		void CompileProgram(int programID);

		int CreateProgram();

		string GetCompilerMessages(int programID);

		bool HasCompilerMessages(int programID);

		void AttachShaderToProgram(int programID, int shader);

		bool IsCompiled(int programID);

		bool CheckUniformLocation (int programId, uint location);

		int GetActiveUniforms (int programId);

        string[] GetUniformBlocks(int programID);

    }
}

