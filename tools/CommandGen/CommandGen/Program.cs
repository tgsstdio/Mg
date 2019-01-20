using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CommandGen
{
	class MainClass
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct LayerProperties
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string layerName;
			public UInt32 specVersion;
			public UInt32 implementationVersion;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string description;
		}

		[DllImport("vulkan-1", CallingConvention = CallingConvention.Winapi)]
		extern static unsafe UInt32 vkEnumerateInstanceLayerProperties(ref UInt32 pPropertyCount, [In, Out] LayerProperties[] pProperties);

		[DllImport("vulkan-1", CallingConvention = CallingConvention.Winapi)]
		extern static unsafe void vkCmdSetBlendConstants(IntPtr commandBuffer, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] float[] blendConstants);

		public static void Main(string[] args)
		{
			//NativeVk();
			try
			{
				const string DLLNAME = "vulkan-1.dll";

				var doc = XDocument.Load("TestData/vk.xml", LoadOptions.PreserveWhitespace);

				var inspector = new VkEntityInspector();
				inspector.Inspect(doc.Root);

				var parser = new VkCommandParser(inspector);

				//parser.Handles.Add("Instance", new HandleInfo { name = "Instance", type = "VK_DEFINE_HANDLE" });

				var lookup = new Dictionary<string, VkCommandInfo>();
				foreach (var child in doc.Root.Descendants("command"))
				{
					VkCommandInfo command;
					if (parser.Parse(child, out command))
					{
						lookup.Add(command.Name, command);
					}
				}

				int noOfUnsafe = 0;
				int totalNativeInterfaces = 0;

				var implementation = new VkInterfaceCollection();
				GenerateInterops(DLLNAME, lookup, ref noOfUnsafe, ref totalNativeInterfaces);
				GenerateHandles(implementation, inspector);
                GenerateInterfaces(implementation, inspector);
                GenerateVkEnums(inspector);
				GenerateVkStructs(inspector);
                GenerateVkClasses(inspector);
                GenerateVkStructureTypes(inspector);

                Console.WriteLine("totalNativeInterfaces :" + totalNativeInterfaces);
				Console.WriteLine("noOfUnsafe :" + noOfUnsafe);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

        static void GenerateVkStructureTypes(VkEntityInspector inspector)
        {
            var container = inspector.Enums["VkStructureType"];          
            {
                using (var interfaceFile = new StreamWriter("VkStructureType.cs", false))
                {
                    interfaceFile.WriteLine("using System;");
                    interfaceFile.WriteLine("");
                    interfaceFile.WriteLine("namespace Magnesium.Vulkan");
                    interfaceFile.WriteLine("{");
                    string tabbedField = "\t";

                    if (container.UseFlags)
                        interfaceFile.WriteLine(tabbedField + "[Flags]");
                    interfaceFile.WriteLine(tabbedField + "internal enum {0} : uint", container.name);
                    interfaceFile.WriteLine(tabbedField + "{");

                    var methodTabs = tabbedField + "\t";

                  //  var uniques = new SortedSet<string>();
                    foreach (var member in container.Members)
                    {
                      //  string key = member.Id;
                     //   if (!uniques.Contains(key))
                      //  {
                            interfaceFile.WriteLine(methodTabs 
                            + VkEntityInspector.ParseVkStructureTypeEnum(member.UnmodifiedKey)
                            + " = " + VkEntityInspector.ParseVkStructureTypeEnum(member.UnmodifiedValue) + ",");
                        //    uniques.Add(key);
                       // }
                    }
                    interfaceFile.WriteLine(tabbedField + "}");
                    interfaceFile.WriteLine("}");
                }
            }
        }

        static void GenerateVkEnums(VkEntityInspector inspector)
		{
			if (!Directory.Exists("Enums"))
			{
				Directory.CreateDirectory("Enums");
			}

			foreach (var container in inspector.Enums.Values)
			{
                if (container.name == "VkSturctureType")
                    continue;

				using (var interfaceFile = new StreamWriter(Path.Combine("Enums", "Mg" + (container.name + ".cs").Substring(2)), false))
				{
					interfaceFile.WriteLine("using System;");
					interfaceFile.WriteLine("");
					interfaceFile.WriteLine("namespace Magnesium");
					interfaceFile.WriteLine("{");
					string tabbedField = "\t";

					if (container.UseFlags)
						interfaceFile.WriteLine(tabbedField + "[Flags]");
					interfaceFile.WriteLine(tabbedField + "public enum {0} : UInt32", "Mg" + container.name.Substring(2));
					interfaceFile.WriteLine(tabbedField + "{");

					var methodTabs = tabbedField + "\t";

                    var uniques = new SortedSet<string>();
					foreach (var member in container.Members)
					{
                        string key = member.Id;
                        //if (!uniques.Contains(key))
                        //{
                            if (!string.IsNullOrWhiteSpace(member.Comment))
                            {
                                interfaceFile.WriteLine(methodTabs + "/// <summary> ");
                                interfaceFile.WriteLine(methodTabs + "/// " + member.Comment);
                                interfaceFile.WriteLine(methodTabs + "/// </summary> ");
                            }
                            interfaceFile.WriteLine(methodTabs + member.Id + " = " + member.Value + ",");
                            //uniques.Add(key);
                        //}
					}
					interfaceFile.WriteLine(tabbedField + "}");
					interfaceFile.WriteLine("}");
				}
			}
		}

		static void GenerateVkStructs(IVkEntityInspector inspector)
		{
			if (!Directory.Exists("Structs"))
			{
				Directory.CreateDirectory("Structs");
			}

			foreach (var container in inspector.Structs.Values)
			{
				using (var interfaceFile = new StreamWriter(Path.Combine("Structs", container.Name + ".cs"), false))
				{
					interfaceFile.WriteLine("using System;");
					interfaceFile.WriteLine("using System.Runtime.InteropServices;");
					interfaceFile.WriteLine("");
					interfaceFile.WriteLine("namespace Magnesium.Vulkan");
					interfaceFile.WriteLine("{");
					string tabbedField = "\t";

					interfaceFile.WriteLine(tabbedField + "[StructLayout(LayoutKind.Sequential)]");
					interfaceFile.WriteLine(tabbedField + "internal struct {0}", container.Name);
					interfaceFile.WriteLine(tabbedField + "{");

					var methodTabs = tabbedField + "\t";

					foreach (var member in container.Members)
					{
                        if (!string.IsNullOrWhiteSpace(member.Comment))
                            interfaceFile.WriteLine(methodTabs + "// " + member.Comment);
                        interfaceFile.WriteLine(methodTabs + member.GetImplementation());
					}
					interfaceFile.WriteLine(tabbedField + "}");
					interfaceFile.WriteLine("}");
				}
			}
		}

        static void GenerateVkClasses(IVkEntityInspector inspector)
        {
            if (!Directory.Exists("Classes"))
            {
                Directory.CreateDirectory("Classes");
            }


            foreach (var container in inspector.Structs.Values)
            {
                string mgClassFile = container.Name.Replace("Vk", "Mg");
                using (var interfaceFile = new StreamWriter(Path.Combine("Classes", mgClassFile + ".cs"), false))
                {
                    interfaceFile.WriteLine("using System;");
                    interfaceFile.WriteLine("");
                    interfaceFile.WriteLine("namespace Magnesium");
                    interfaceFile.WriteLine("{");
                    string tabbedField = "\t";


                    interfaceFile.WriteLine(tabbedField + "public class {0}", mgClassFile);
                    interfaceFile.WriteLine(tabbedField + "{");

                    var methodTabs = tabbedField + "\t";

                    foreach (var member in container.Members)
                    {
                        if (member.Name == "sType")
                        {
                            continue;
                        }
                        if (member.Name == "pNext")
                        {
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(member.Comment))
                        {
                            interfaceFile.WriteLine(methodTabs + "///");
                            interfaceFile.WriteLine(methodTabs + "/// " + member.Comment);
                            interfaceFile.WriteLine(methodTabs + "///");
                        }
                        interfaceFile.WriteLine(methodTabs + member.GetClassLine());
                    }
                    interfaceFile.WriteLine(tabbedField + "}");
                    interfaceFile.WriteLine("}");
                }
            }
        }

        static void GenerateHandles(VkInterfaceCollection implementation, IVkEntityInspector inspector)
		{
			if (!Directory.Exists("Handles"))
			{
				Directory.CreateDirectory("Handles");
			}

			foreach (var container in implementation.Interfaces)
			{
				VkHandleInfo found;
				if (inspector.Handles.TryGetValue(container.Name, out found))
				{
					container.Handle = found;
				}

				using (var interfaceFile = new StreamWriter(Path.Combine("Handles", container.Name + ".cs"), false))
                {
                    FillOutHandleImplementation(container, interfaceFile);
                }
            }
		}

        private static void FillOutHandleImplementation(VkContainerClass container, StreamWriter interfaceFile)
        {
            interfaceFile.WriteLine("using System;");
            interfaceFile.WriteLine("namespace Magnesium.Vulkan");
            interfaceFile.WriteLine("{");
            string tabbedField = "\t";

            interfaceFile.WriteLine(tabbedField + "public class {0} : {1}", container.Name, container.InterfaceName);
            interfaceFile.WriteLine(tabbedField + "{");

            var methodTabs = tabbedField + "\t";

            if (container.Handle != null)
            {
                // create internal field
                interfaceFile.WriteLine(string.Format("{0}internal {1} Handle = {2};", methodTabs, container.Handle.csType, container.Handle.csType == "IntPtr" ? "IntPtr.Zero" : "0L"));

                // create constructor
                interfaceFile.WriteLine(string.Format("{0}internal {1}({2} handle)", methodTabs, container.Name, container.Handle.csType));
                interfaceFile.WriteLine(methodTabs + "{");
                interfaceFile.WriteLine(methodTabs + "\tHandle = handle;");
                interfaceFile.WriteLine(methodTabs + "}");
                interfaceFile.WriteLine("");
            }

            foreach (var method in container.Methods)
            {

                interfaceFile.WriteLine(methodTabs + method.GetImplementation());
                interfaceFile.WriteLine(methodTabs + "{");
                interfaceFile.WriteLine(methodTabs + "}");
                interfaceFile.WriteLine("");
            }
            interfaceFile.WriteLine(tabbedField + "}");
            interfaceFile.WriteLine("}");
        }

        static void GenerateInterfaces(VkInterfaceCollection implementation, IVkEntityInspector inspector)
        {
            if (!Directory.Exists("Interfaces"))
            {
                Directory.CreateDirectory("Interfaces");
            }

            foreach (var container in implementation.Interfaces)
            {
                VkHandleInfo found;
                if (inspector.Handles.TryGetValue(container.Name, out found))
                {
                    container.Handle = found;
                }

                using (var interfaceFile = new StreamWriter(Path.Combine("Interfaces", container.InterfaceName + ".cs"), false))
                {
                    interfaceFile.WriteLine("using System;");
                    interfaceFile.WriteLine("namespace Magnesium");
                    interfaceFile.WriteLine("{");
                    string tabbedField = "\t";

                    interfaceFile.WriteLine(tabbedField + "public interface {0}", container.InterfaceName);
                    interfaceFile.WriteLine(tabbedField + "{");

                    var methodTabs = tabbedField + "\t";

                    foreach (var method in container.Methods)
                    {
                        interfaceFile.WriteLine(methodTabs + method.GetInterfaceLine());
                    }
                    interfaceFile.WriteLine(tabbedField + "}");
                    interfaceFile.WriteLine("}");
                }
            }
        }

        static void GenerateInterops(string DLLNAME, Dictionary<string, VkCommandInfo> lookup, ref int noOfUnsafe, ref int totalNativeInterfaces)
		{
			using (var interfaceFile = new StreamWriter("Interops.cs", false))
			{
				interfaceFile.WriteLine("using Magnesium;");
				interfaceFile.WriteLine("using System;");
				interfaceFile.WriteLine("using System.Runtime.InteropServices;");
				interfaceFile.WriteLine("namespace Magnesium.Vulkan");
				interfaceFile.WriteLine("{");

				var tabbedField = "\t";
				interfaceFile.WriteLine(tabbedField + "internal static class Interops");
				interfaceFile.WriteLine(tabbedField + "{");

				var methodTabs = tabbedField + "\t";


				interfaceFile.WriteLine(methodTabs + "const string VULKAN_LIB = \"" + DLLNAME + "\";");
				interfaceFile.WriteLine("");

				foreach (var command in lookup.Values)
				{

					++totalNativeInterfaces;
					if (command.NativeFunction.UseUnsafe)
						++noOfUnsafe;

					interfaceFile.WriteLine(methodTabs + "[DllImport(VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]");
					interfaceFile.WriteLine(methodTabs + command.NativeFunction.GetImplementation());
					interfaceFile.WriteLine("");
					//WriteVkInterface(methodFile, command);
				}

				interfaceFile.WriteLine(tabbedField + "}");
				interfaceFile.WriteLine("}");
			}
		}

		//static void WriteVkInterface(StreamWriter interfaceFile, VkCommandInfo command)
		//{


		//	interfaceFile.WriteLine(tabbedField + "public class {0} : {1}", command.Name, command.InterfaceName);
		//	interfaceFile.WriteLine(tabbedField + "{");
		//	foreach (var method in command.Methods)
		//	{
		//		var methodTabs = tabbedField + "\t";
		//		interfaceFile.WriteLine(methodTabs + method.GetImplementation());
		//		interfaceFile.WriteLine(methodTabs + "{");
		//		interfaceFile.WriteLine(methodTabs + "}");
		//	}
		//	interfaceFile.WriteLine(tabbedField + "}");
		//	interfaceFile.WriteLine("}");
		//}

		static void VkMethod(StreamWriter methodFile, VkCommandInfo command)
		{
			methodFile.WriteLine(command.MethodSignature.GetImplementation());
			methodFile.WriteLine("{");
			foreach (var line in command.Lines)
			{
				methodFile.WriteLine(line.GetImplementation());
			}
			methodFile.WriteLine("}");
		}




		static unsafe void NativeVk()
		{
			var p = Marshal.SizeOf(typeof(LayerProperties));

			Console.WriteLine("Hello World!" + p);

			UInt32 pPropertyCount = 0;
			var result = vkEnumerateInstanceLayerProperties(ref pPropertyCount, null);
			Console.WriteLine("Count!" + result + " : " + pPropertyCount);

			LayerProperties[] pProperties = new LayerProperties[pPropertyCount];
			var result2 = vkEnumerateInstanceLayerProperties(ref pPropertyCount, pProperties);

			Console.WriteLine("Count!" + result2 + " : " + pPropertyCount);

			foreach (var prop in pProperties)
			{
				Console.WriteLine(prop.layerName + " - " + prop.description);
			}
		}
}
}
