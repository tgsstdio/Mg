using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Foundation;
using Metal;

namespace Magnesium.Metal
{
	public class AmtMetalShaderCompiler
	{
		public AmtMetalShaderCompiler()
		{
		}

		public void Compile(IMTLDevice device, MgGraphicsPipelineCreateInfo info)
		{
			IMTLFunction frag = null;
			IMTLFunction vert = null;
			foreach (var stage in info.Stages)
			{
				//var shaderType = ShaderType.VertexShader;
				//if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				//{
				//	shaderType = ShaderType.FragmentShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				//{
				//	shaderType = ShaderType.VertexShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.COMPUTE_BIT)
				//{
				//	shaderType = ShaderType.ComputeShader;
				//}
				//else if (stage.Stage == MgShaderStageFlagBits.GEOMETRY_BIT)
				//{
				//	shaderType = ShaderType.GeometryShader;
				//}

				IMTLFunction shaderFunc = null;

				var module = (AmtShaderModule)stage.Module;
				Debug.Assert(module != null);
				if (module.Function != null)
				{
					shaderFunc = module.Function;
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
							shaderFunc = module.Function;
						}
					}
				}
				Debug.Assert(shaderFunc != null);
				if (stage.Stage == MgShaderStageFlagBits.VERTEX_BIT)
				{
					vert = shaderFunc;
				}
				else if (stage.Stage == MgShaderStageFlagBits.FRAGMENT_BIT)
				{
					frag = shaderFunc;
				}
			
			}
		}
	}
}
