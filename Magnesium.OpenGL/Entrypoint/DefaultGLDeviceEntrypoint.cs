using System;

namespace Magnesium.OpenGL
{
	public class DefaultGLDeviceEntrypoint : IGLDeviceEntrypoint
	{
		public DefaultGLDeviceEntrypoint 
		(
			IGLCmdVBOEntrypoint vbo,
			IGLSamplerEntrypoint sampler,
			IGLDeviceImageEntrypoint image,
			IGLDeviceImageViewEntrypoint imageView,
			IGLImageDescriptorEntrypoint imageDescriptor,
			IGLShaderModuleEntrypoint shaderModule,
			IGLDescriptorPoolEntrypoint descriptorPool,
			IGLBufferEntrypoint buffers,
			IGLDeviceMemoryEntrypoint deviceMemory,
			IGLSemaphoreEntrypoint semaphore,
			IGLGraphicsPipelineEntrypoint graphicsPipeline,
			IGLImageFormatEntrypoint imageFormat,
			IGLGraphicsPipelineCompiler graphicsCompiler
		)
		{
			VBO = vbo;
			Sampler = sampler;
			Image = image;
			ImageView = imageView;
			ImageDescriptor = imageDescriptor;
			ShaderModule = shaderModule;
			DescriptorPool = descriptorPool;
			Buffers = buffers;
			DeviceMemory = deviceMemory;
			Semaphore = semaphore;
			GraphicsPipeline = graphicsPipeline;
			ImageFormat = imageFormat;
			GraphicsCompiler = graphicsCompiler;
		}

		#region IGLDeviceCapabilities implementation

		public IGLImageFormatEntrypoint ImageFormat {
			get;
			private set;
		}

		public IGLCmdVBOEntrypoint VBO {
			get ;
			private set;
		}

		public IGLSamplerEntrypoint Sampler {
			get;
			private set;
		}

		public IGLDeviceImageEntrypoint Image {
			get;
			private set;
		}

		public IGLDeviceImageViewEntrypoint ImageView {
			get;
			private set;
		}

		public IGLImageDescriptorEntrypoint ImageDescriptor {
			get;
			private set;
		}

		public IGLShaderModuleEntrypoint ShaderModule {
			get;
			private set;
		}

		public IGLDescriptorPoolEntrypoint DescriptorPool {
			get;
			private set;
		}

		public IGLBufferEntrypoint Buffers
		{
			get;
			private set;
		}

		public IGLDeviceMemoryEntrypoint DeviceMemory {
			get;
			private set;
		}

		public IGLSemaphoreEntrypoint Semaphore {
			get;
			private set;
		}

		public IGLGraphicsPipelineEntrypoint GraphicsPipeline {
			get;
			private set;
		}

		public IGLGraphicsPipelineCompiler GraphicsCompiler
		{
			get;
			private set;
		}
		#endregion
	}
}

