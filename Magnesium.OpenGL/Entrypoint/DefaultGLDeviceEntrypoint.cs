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
			//IGLImageDescriptorEntrypoint imageDescriptor,
			IGLShaderModuleEntrypoint shaderModule,
			IGLFutureDescriptorPoolEntrypoint descriptorPool,
			IGLBufferEntrypoint buffers,
			IGLDeviceMemoryEntrypoint deviceMemory,
			IGLSemaphoreEntrypoint semaphore,
			IGLGraphicsPipelineEntrypoint graphicsPipeline,
			IGLImageFormatEntrypoint imageFormat,
			IGLGraphicsPipelineCompiler graphicsCompiler,
            IGLFenceEntrypoint fence,
            IGLCmdShaderProgramEntrypoint shaderProgram,
            IGLDescriptorSetEntrypoint descriptorSet,
            IGLUniformBlockEntrypoint uniformBlocks,
            IGLFramebufferHelperSelector framebuffer
        )
		{
			VBO = vbo;
			Sampler = sampler;
			Image = image;
			ImageView = imageView;
			//ImageDescriptor = imageDescriptor;
			ShaderModule = shaderModule;
			DescriptorPool = descriptorPool;
			Buffers = buffers;
			DeviceMemory = deviceMemory;
			Semaphore = semaphore;
			GraphicsPipeline = graphicsPipeline;
			ImageFormat = imageFormat;
			GraphicsCompiler = graphicsCompiler;
            Fence = fence;
            ShaderProgram = shaderProgram;
            DescriptorSet = descriptorSet;
            UniformBlocks = uniformBlocks;
            Framebuffer = framebuffer;
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

		//public IGLImageDescriptorEntrypoint ImageDescriptor {
		//	get;
		//	private set;
		//}

		public IGLShaderModuleEntrypoint ShaderModule {
			get;
			private set;
		}

		public IGLFutureDescriptorPoolEntrypoint DescriptorPool {
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

        public IGLFenceEntrypoint Fence
        {
            get;
            private set;
        }

        public IGLCmdShaderProgramEntrypoint ShaderProgram
        {
            get;
            private set;
        }

        public IGLUniformBlockEntrypoint UniformBlocks
        {
            get;
            private set;
        }

        public IGLDescriptorSetEntrypoint DescriptorSet
        {
            get;
            private set;
        }

        public IGLFramebufferHelperSelector Framebuffer
        {
            get;
            private set;
        }
        #endregion
    }
}

