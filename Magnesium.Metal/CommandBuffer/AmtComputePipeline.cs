using System;
using System.Diagnostics;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtComputePipeline : IMgPipeline
	{
		public AmtComputePipeline(IMTLDevice device, MgComputePipelineCreateInfo createInfo)
		{
			if (createInfo == null)
				throw new ArgumentNullException(nameof(createInfo));

			if (createInfo.Layout == null)
				throw new ArgumentNullException(nameof(createInfo.Layout));

			if (createInfo.Stage == null)
				throw new ArgumentNullException(nameof(createInfo.Stage));

			if (createInfo.Stage.Module == null)
				throw new ArgumentNullException(nameof(createInfo.Stage.Module));
			InitializeThreadsPerGroupSize(createInfo);

			// TODO : layout extraction

			InitializeShaderModule(device, createInfo);

			MTLComputePipelineReflection reflection;
			Foundation.NSError err;
			var options = MTLPipelineOption.ArgumentInfo | MTLPipelineOption.BufferTypeInfo;
			var pipelineState = device.CreateComputePipelineState(mShaderModule, options, out reflection, out err);
			Debug.Assert(pipelineState != null);

		}

		void InitializeThreadsPerGroupSize(MgComputePipelineCreateInfo createInfo)
		{
			var groupSize = createInfo.ThreadsPerWorkgroup;
			if (IntPtr.Size == 4)
			{
				if (groupSize.X < 0 || groupSize.X > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(nameof(createInfo.ThreadsPerWorkgroup.X) 
					                                      + " must be between 0 and " + nint.MaxValue);
				}

				if (groupSize.Y < 0 || groupSize.Y > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(nameof(createInfo.ThreadsPerWorkgroup.Y)
					                                      + " must be between 0 and " + nint.MaxValue);
				}

				if (groupSize.Z < 0 || groupSize.Z > nint.MaxValue)
				{
					throw new ArgumentOutOfRangeException(nameof(createInfo.ThreadsPerWorkgroup.Z) 
					                                      + " must be between 0 and " + nint.MaxValue);
				}
			}

			ThreadsPerGroupSize = new MTLSize(
				(nint)createInfo.ThreadsPerWorkgroup.X,
				(nint)createInfo.ThreadsPerWorkgroup.Y,
				(nint)createInfo.ThreadsPerWorkgroup.Z);
		}

		void InitializeShaderModule(IMTLDevice device, MgComputePipelineCreateInfo createInfo)
		{
			var stage = createInfo.Stage;
			var module = (AmtShaderModule)stage.Module;
			Debug.Assert(module != null);
			if (module.Function != null)
			{
				mShaderModule = module.Function;
			}
			else
			{
				using (var ms = new MemoryStream())
				{
					module.Info.Code.CopyTo(ms, (int)module.Info.CodeSize.ToUInt32());

					// UPDATE SHADERMODULE wIth FUNCTION FOR REUSE
					using (NSData data = NSData.FromArray(ms.ToArray()))
					{
						NSError err;
						IMTLLibrary library = device.CreateLibrary(data, out err);
						if (library == null)
						{
							// TODO: better error handling
							throw new Exception(err.ToString());
						}
						module.Function = library.CreateFunction(stage.Name);
						mShaderModule = module.Function;
					}
				}
			}
			Debug.Assert(mShaderModule != null);
		}

		public IMTLComputePipelineState Compute { get; set;}
		public MTLSize ThreadsPerGroupSize { get; internal set; }
		private IMTLFunction mShaderModule;

		public void DestroyPipeline(IMgDevice device, IMgAllocationCallbacks allocator)
		{

		}
	}
}