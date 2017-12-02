using Magnesium.OpenGL.Internals;
namespace Magnesium.OpenGL
{
    public interface IGLNextCmdShaderProgramCache
	{
        void Initialize();

		int ProgramID { get; }
        void SetProgramID(MgPipelineBindPoint bindingPoint, int programId, GLInternalCache layoutCache, IGLPipelineLayout pipelineLayout);
        uint VAO { get; }
		void SetVAO(uint vao);
        void SetDescriptorSets(GLCmdDescriptorSetParameter ds);
        int FBO { get; }
        void SetFramebuffer(int fbo);
    }
}
