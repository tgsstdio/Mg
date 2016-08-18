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

				typeof(IMgEntrypoint),
				typeof(IMgInstance),
				typeof(IMgPhysicalDevice),
				typeof(IMgDevice),
				typeof(IMgQueue),
				typeof(IMgCommandBuffer),

				typeof(IMgDeviceMemory),
				typeof(IMgCommandPool),
				typeof(IMgBuffer),
				typeof(IMgBufferView),
				typeof(IMgImage),

				typeof(IMgImageView),
				typeof(IMgShaderModule),
				typeof(IMgPipeline),
				typeof(IMgPipelineLayout),
				typeof(IMgSampler),

				typeof(IMgDescriptorSet),
				typeof(IMgDescriptorSetLayout),
				typeof(IMgDescriptorPool),
				typeof(IMgFence),
				typeof(IMgSemaphore),

				typeof(IMgEvent),
				typeof(IMgQueryPool),
				typeof(IMgFramebuffer),
				typeof(IMgRenderPass),
				typeof(IMgPipelineCache),

				typeof(IMgSurfaceKHR),
				typeof(IMgSwapchainKHR),
				typeof(MgDebugReportCallbackEXT),
				typeof(IMgAllocationCallbacks),
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
						IsStatic = false,
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
						};

						// Name
						if (reservedNames.Contains(p.Name))
						{
							p.Name = "@" + p.Name;
						}


						// BaseCsType
						if (p.UseOut)
						{
							p.BaseCsType = p.BaseCsType.Replace("&", "");
						}
						else if (p.BaseCsType == "Void")
						{
							p.BaseCsType = "void";
						}
						else if (p.BaseCsType == "String")
						{
							p.BaseCsType = "string";
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

