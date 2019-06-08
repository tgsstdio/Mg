using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Magnesium;

namespace CommandGen
{
	public class VkInterfaceCollection
	{
		public List<VkContainerClass> Interfaces { get; private set; }
		public VkInterfaceCollection()
		{
			Interfaces = RefactorMgInterfaces();
		}

		static List<VkContainerClass> RefactorMgInterfaces()
		{
			var interfaces = new Type[] {
                typeof(IMgAllocationCallbacks),
				typeof(IMgBuffer),
				typeof(IMgBufferView),
				typeof(IMgCommandBuffer),
				typeof(IMgCommandPool),
				typeof(IMgDescriptorPool),
				typeof(IMgDescriptorSet),
				typeof(IMgDescriptorSetLayout),
				typeof(IMgDevice),
				typeof(IMgDeviceMemory),
                typeof(IMgDisplayModeKHR),
				typeof(IMgEvent),
				typeof(IMgFence),
				typeof(IMgFramebuffer),
				typeof(IMgImage),
				typeof(IMgImageView),
                typeof(IMgIndirectCommandsLayoutNVX),
				typeof(IMgInstance),
                typeof(IMgObjectTableNVX),
				typeof(IMgPipeline),
				typeof(IMgPipelineCache),
				typeof(IMgPipelineLayout),
				typeof(IMgQueryPool),
				typeof(IMgQueue),
				typeof(IMgRenderPass),
				typeof(IMgSampler),
				typeof(IMgSemaphore),
				typeof(IMgShaderModule),
				typeof(IMgSurfaceKHR),
				typeof(IMgSwapchainKHR),
                typeof(IMgEntrypoint),
				typeof(IMgPhysicalDevice),

				typeof(IMgDebugReportCallbackEXT),
			 };

			var uniques = new StringCollection();


			foreach (var handle in interfaces)
			{
				if (uniques.Contains(handle.Name))
				{
					throw new InvalidOperationException();
				}
				else
				{
					uniques.Add(handle.Name);
				}
			}

			var reservedNames = new StringCollection() { "event", "object" };

    		var implementation = new List<VkContainerClass>();
			foreach (var i in interfaces)
			{
				var container = new VkContainerClass { Name = i.Name.Replace("IMg", "Vk") };
				container.InterfaceName = i.Name;
				foreach (var info in i.GetMethods())
				{
					var method = new VkMethodSignature
					{
						Name = info.Name,
						IsStatic = true,
						ReturnType = info.ReturnType.Name,
					};

					if (method.ReturnType == "Void")
					{
						method.ReturnType = "void";
					}

					foreach (var param in info.GetParameters())
					{
						var p = new VkMethodParameter
						{
							Name = param.Name,
							BaseCsType = param.ParameterType.Name,
							UseOut = param.IsOut,
                            UseRef = param.ParameterType.IsByRef,
						};

						// Name
						if (reservedNames.Contains(p.Name))
						{
							p.Name = "@" + p.Name;
						}


                        if (p.BaseCsType == "Void")
						{
							p.BaseCsType = "void";
						}
						else if (p.BaseCsType == "String")
						{
							p.BaseCsType = "string";
						}
                        else if (p.BaseCsType == "Single")
                        {
                            p.BaseCsType = "float";
                        }

                        // BaseCsType
                        if (p.BaseCsType.EndsWith("&"))
                        {
                            //Remove & suffix
                            var len = p.BaseCsType.Length;
                            p.BaseCsType = p.BaseCsType.Substring(0, len - 1);
                        }

                        method.Parameters.Add(p);
					}

					container.Methods.Add(method);
				}
				implementation.Add(container);
			}
			return implementation;
		}
	}
}

