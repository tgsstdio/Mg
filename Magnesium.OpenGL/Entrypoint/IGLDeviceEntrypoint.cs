namespace Magnesium.OpenGL
{
	public interface IGLDeviceEntrypoint
	{
		IGLCmdVBOEntrypoint VBO { get; }
		IGLSamplerEntrypoint Sampler { get; }
		IGLDeviceImageEntrypoint Image {get; }
		IGLDeviceImageViewEntrypoint ImageView { get; }
		IGLImageDescriptorEntrypoint ImageDescriptor { get; }
		IGLShaderModuleEntrypoint ShaderModule { get; }
		IGLDescriptorPoolEntrypoint DescriptorPool { get; }
		IGLBufferEntrypoint Buffers { get;}
		IGLDeviceMemoryEntrypoint DeviceMemory { get; }
		IGLSemaphoreEntrypoint Semaphore {get; }
		IGLGraphicsPipelineEntrypoint GraphicsPipeline { get; }
		IGLGraphicsPipelineCompiler GraphicsCompiler { get; }
		IGLImageFormatEntrypoint ImageFormat { get; }
	}
}

