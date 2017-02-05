using System;

namespace Magnesium.OpenGL
{
	public interface IGLCmdShaderProgramEntrypoint
	{
		void BindProgram(int programID);

		void BindVAO(int vao);

		void BindStorageBuffer(uint binding, int bufferId, long offset, int size);

		void SetUniformBlock(int programID, uint activeIndex, uint bindingPoint);

		void BindUniformBuffers(uint count, int[] buffers, IntPtr[] offsets, int[] sizes);

		void BindCombinedImageSampler(int programID, uint binding, ulong value);
	}
}