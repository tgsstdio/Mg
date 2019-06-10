using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
                const string libFolder = "Toolkit";
                const string platformFolder = "Vulkan";
                GenerateNeuHandles(libFolder, platformFolder, "Toolkit", "Vulkan", "Vk", null, implementation, inspector, lookup);

                Console.WriteLine("totalNativeInterfaces :" + totalNativeInterfaces);
				Console.WriteLine("noOfUnsafe :" + noOfUnsafe);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

        const string TAB_FIELD = "\t";

        static void GenerateNeuHandles(string toolkitFolder, string platformFolder, string toolkitNamespace, string platformNamespace, string platformPrefix, string argumentPrefix, VkInterfaceCollection implementation, IVkEntityInspector inspector, Dictionary<string, VkCommandInfo> lookup)
        {
            const string VALIDATION_NAMESPACE = "Validation";
            const string FUNCTION_NAMESPACE = "Functions";

            CreateFolderIfMissing(toolkitFolder);
            CreateFolderIfMissing(platformFolder);

            var platformDirectoryPath = System.IO.Path.Combine(platformFolder, FUNCTION_NAMESPACE);
            CreateFolderIfMissing(platformDirectoryPath);

            var toolkitDirectoryPath = System.IO.Path.Combine(toolkitFolder, VALIDATION_NAMESPACE);
            CreateFolderIfMissing(toolkitDirectoryPath);

            var groups = new SortedSet<string>();

            groups.Add("VkQueue");
            groups.Add("VkDevice");
            groups.Add("VkPhysicalDevice");
            groups.Add("VkCommandBuffer");
            groups.Add("VkInstance");

            foreach (var group in implementation.Interfaces)
            {
                if (groups.Contains(group.Name))
                {

                    if (inspector.Handles.TryGetValue(group.Name, out VkHandleInfo found))
                    {
                        group.Handle = found;
                    }

                    string subCategoryNs = group.Name.Substring(2);

                    // 1. TOP LEVEL FILE 

                    foreach (var flag in new bool[] { true, false })
                    {
                        string decoratorName = ((flag) ? "Safe" : "Fast") + subCategoryNs;

                        string topLevelFile = System.IO.Path.Combine(toolkitFolder, decoratorName + ".cs");
                        GeneratorDecoratorFile(toolkitNamespace, argumentPrefix, group, subCategoryNs, flag, decoratorName, topLevelFile);
                    }

                    foreach (var fn in group.Methods)
                    {
                        var methodTabs = TAB_FIELD + "\t";

                        CreateValidateStubFiles(toolkitNamespace, VALIDATION_NAMESPACE, toolkitDirectoryPath, subCategoryNs, fn, methodTabs);

                        CreateImplSectionFiles(platformNamespace, platformPrefix, FUNCTION_NAMESPACE, platformDirectoryPath, subCategoryNs, fn, methodTabs, lookup);
                    }

                    string wiringName = platformPrefix + subCategoryNs;
                    string wiringFile = System.IO.Path.Combine(platformFolder, wiringName + ".cs");
                    GeneratePlatformWiring(group, wiringFile, platformNamespace, FUNCTION_NAMESPACE, wiringName, subCategoryNs, platformPrefix);
                }
            }
        }

        private static void GeneratePlatformWiring(VkContainerClass group, string wiringFile, string libraryName, string category, string wiringName, string subCategoryNs, string platformPrefix)
        {
            var methodTabs = TAB_FIELD + "\t";

            using (var fs = new StreamWriter(wiringFile, false))
            {
                fs.WriteLine("using System;");
                fs.WriteLine("using System.Runtime.InteropServices;");
                fs.WriteLine();

                fs.WriteLine("namespace Magnesium." + libraryName + "." + category);
                fs.WriteLine("{");
                fs.WriteLine(TAB_FIELD + "public class {0} : {1}", wiringName, group.InterfaceName);
                fs.WriteLine(TAB_FIELD + "{");

                var infoClassName = platformPrefix + subCategoryNs + "Info";

                if (group.Handle != null)
                {
                    // create internal field
                    fs.WriteLine(string.Format("{0}internal readonly {1} Info;", methodTabs, infoClassName));

                    // create constructor
                    fs.WriteLine(string.Format("{0}internal {1}({2} handle)", methodTabs, group.Name, group.Handle.csType));
                    fs.WriteLine(methodTabs + "{");
                    fs.WriteLine(methodTabs + "\tInfo = new {0}(handle);", infoClassName);
                    fs.WriteLine(methodTabs + "}");
                    fs.WriteLine("");
                }

                foreach (var fn in group.Methods)
                {
                    fs.WriteLine(methodTabs + GetWiringInterface(fn, platformPrefix, subCategoryNs));
                    fs.WriteLine(methodTabs + "{");
                    fs.WriteLine(GetWiringMethod(fn, platformPrefix, subCategoryNs, methodTabs));
                    fs.WriteLine(methodTabs + "}");
                    fs.WriteLine();
                }
                fs.WriteLine(TAB_FIELD + "}");
                fs.WriteLine("}");
            }
        }

        public static string GetWiringInterface(VkMethodSignature fn, string libraryPrefix, string subCategoryNs)
        {
            var builder = new StringBuilder();
            builder.Append("public");

            builder.Append(" ");
            builder.Append(fn.ReturnType);
            builder.Append(" ");

            builder.Append(fn.Name);
            builder.Append("(");
            // foreach arg in arguments

            bool needComma = false;

            foreach (var param in fn.Parameters)
            {
                if (needComma)
                {
                    builder.Append(", ");
                }
                else
                {
                    needComma = true;
                }

                if (param.UseOut)
                {
                    builder.Append("out ");
                }
                else if (param.UseRef)
                {
                    builder.Append("ref ");
                }

                builder.Append(param.BaseCsType);
                builder.Append(" ");
                builder.Append(param.Name);
            }
            builder.Append(")");

            return builder.ToString();
        }

        public static string GetWiringMethod(VkMethodSignature fn, string libraryPrefix, string subCategoryNs, string methodTabs)
        {
            var returnType = fn.ReturnType == "void"
                ? ""
                : "return ";


            var validateArgs = new StringBuilder();
            validateArgs.Append(methodTabs);
            validateArgs.Append(TAB_FIELD);
            validateArgs.Append(returnType);

            string className = libraryPrefix + fn.Name + "Section";

            validateArgs.Append(className);
            validateArgs.Append(".");

            validateArgs.Append(fn.Name);
            validateArgs.Append("(");

            bool needComma = false;

            var combos = new List<VkMethodParameter>();
            combos.Add(
                new VkMethodParameter
                {
                    BaseCsType = libraryPrefix + subCategoryNs + "Info",
                    Name = "info",
                }
            );
            combos.AddRange(fn.Parameters);

            foreach (var param in combos)
            {
                if (needComma)
                {
                    validateArgs.Append(", ");
                }
                else
                {
                    needComma = true;
                }

                if (param.UseOut)
                {
                    validateArgs.Append("out ");
                }
                else if (param.UseRef)
                {
                    validateArgs.Append("ref ");
                }

                validateArgs.Append(param.Name);
            }
            validateArgs.Append(");");

            return validateArgs.ToString();
        }

        private static void CreateFolderIfMissing(string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
        }

        private static void GeneratorDecoratorFile(string libraryName, string argumentPrefix, VkContainerClass group, string subCategoryNs, bool flag, string decoratorName, string topLevelFile)
        {
            using (var fs = new StreamWriter(topLevelFile, false))
            {
                var methodTabs = TAB_FIELD + "\t";
                var validationPrefix = methodTabs + TAB_FIELD + "Validation." + subCategoryNs + ".";

                fs.WriteLine("using System;");

                fs.WriteLine("namespace Magnesium." + libraryName);
                fs.WriteLine("{");

                fs.WriteLine(TAB_FIELD + "public class {0} : {1}", decoratorName, group.InterfaceName);
                fs.WriteLine(TAB_FIELD + "{");

                // ctor
                // create internal field
                fs.WriteLine(string.Format("{0}internal {1} mImpl = null;", methodTabs, group.InterfaceName));

                // create constructor
                fs.WriteLine(string.Format("{0}internal {1}({2} impl)", methodTabs, decoratorName, group.InterfaceName));
                fs.WriteLine(methodTabs + "{");
                fs.WriteLine(methodTabs + "\tmImpl = impl;");
                fs.WriteLine(methodTabs + "}");
                fs.WriteLine("");


                foreach (var fn in group.Methods)
                {
                    GenerateDecoratorStub(flag, argumentPrefix, fs, methodTabs, validationPrefix, fn);
                }

                fs.WriteLine(TAB_FIELD + "}");
                fs.WriteLine("}");
            }
        }

        private static void CreateImplSectionFiles(string libraryName, string libraryPrefix, string category, string methodDirectoryPath, string subCategoryNs, VkMethodSignature fn, string methodTabs, Dictionary<string, VkCommandInfo> lookup)
        {
            var implDirectoryPath = CreateSubDirectory(methodDirectoryPath, subCategoryNs);

            string className = libraryPrefix + fn.Name + "Section";
            string implFilePath = System.IO.Path.Combine(implDirectoryPath, className + ".cs");
            using (var fs = new StreamWriter(implFilePath, false))
            {
                fs.WriteLine("using System;");
                fs.WriteLine("using System.Runtime.InteropServices;");
                fs.WriteLine();

                fs.WriteLine("namespace Magnesium." + libraryName + "." + category + "." + subCategoryNs);

                fs.WriteLine("{");

                fs.WriteLine(TAB_FIELD + "public class {0}", className);
                fs.WriteLine(TAB_FIELD + "{");

                if (lookup.TryGetValue("vk" + fn.Name, out VkCommandInfo found))
                {
                    fs.WriteLine(methodTabs + "[DllImport(Interops.VULKAN_LIB, CallingConvention=CallingConvention.Winapi)]");
                    fs.WriteLine(methodTabs + found.NativeFunction.GetImplementation());
                    fs.WriteLine();
                }


                fs.WriteLine(methodTabs + GetImplementationStubMethod(fn, libraryPrefix, subCategoryNs));
                fs.WriteLine(methodTabs + "{");
                fs.WriteLine(methodTabs + TAB_FIELD + "// TODO: add implementation");

                if (fn.ReturnType != "void")
                {
                    fs.WriteLine(methodTabs + TAB_FIELD + "throw new NotImplementedException();");
                }

                fs.WriteLine(methodTabs + "}");


                fs.WriteLine(TAB_FIELD + "}");
                fs.WriteLine("}");
            }
        }

        public static string GetImplementationStubMethod(VkMethodSignature fn, string libraryPrefix, string subCategoryNs)
        {
            var builder = new StringBuilder();
            builder.Append("public");

            if (fn.IsStatic)
                builder.Append(" static");

            builder.Append(" ");
            builder.Append(fn.ReturnType);
            builder.Append(" ");

            builder.Append(fn.Name);
            builder.Append("(");
            // foreach arg in arguments

            var combos = new List<VkMethodParameter>();
            combos.Add(
                new VkMethodParameter {
                    BaseCsType = libraryPrefix + subCategoryNs + "Info",
                    Name = "info",
                }
            );
            combos.AddRange(fn.Parameters);

            bool needComma = false;

            foreach (var param in combos)
            {
                if (needComma)
                {
                    builder.Append(", ");
                }
                else
                {
                    needComma = true;
                }

                if (param.UseOut)
                {
                    builder.Append("out ");
                }
                else if (param.UseRef)
                {
                    builder.Append("ref ");
                }

                builder.Append(param.BaseCsType);
                builder.Append(" ");
                builder.Append(param.Name);
            }
            builder.Append(")");

            return builder.ToString();
        }

        private static void CreateValidateStubFiles(string libraryName, string category, string checkDirectoryPath, string subCategoryNs, VkMethodSignature fn, string methodTabs)
        {
            var validateDirectoryPath = CreateSubDirectory(checkDirectoryPath, subCategoryNs);

            string checkFilePath = System.IO.Path.Combine(validateDirectoryPath, fn.Name + ".cs");

            var validateArgs = new StringBuilder();
            validateArgs.Append(methodTabs);
            validateArgs.Append("public static void Validate(");

            var noOfParameters = 0;
            bool needComma = false;
            foreach (var param in fn.Parameters)
            {
                if (param.UseOut)
                {

                    continue;
                }

                if (needComma)
                {
                    validateArgs.Append(", ");
                }
                else
                {
                    needComma = true;
                }


                if (param.UseRef)
                {
                    validateArgs.Append("ref ");
                }

                validateArgs.Append(param.BaseCsType);
                validateArgs.Append(" ");

                validateArgs.Append(param.Name);
                noOfParameters++;
            }

            if (noOfParameters <= 0)
            {
                return;
            }

            validateArgs.Append(")");

            var sr = new StringBuilder();

            sr.AppendLine("using System;");

            sr.AppendLine("namespace Magnesium." + libraryName + "." + category + "." + subCategoryNs);
            sr.AppendLine("{");

            sr.AppendFormat(TAB_FIELD + "public class {0}", fn.Name);
            sr.AppendLine();
            sr.AppendLine(TAB_FIELD + "{");

            sr.AppendLine(validateArgs.ToString());

            sr.AppendLine(methodTabs + "{");
            sr.AppendLine(methodTabs + TAB_FIELD + "// TODO: add validation");

            if (fn.ReturnType != "void")
            {
                sr.AppendLine(methodTabs + TAB_FIELD + "throw new NotImplementedException();");
            }

            sr.AppendLine(methodTabs + "}");


            sr.AppendLine(TAB_FIELD + "}");
            sr.AppendLine("}");

            using (var fs2 = new StreamWriter(checkFilePath, false))
            {
                fs2.Write(sr.ToString());
            }
            
        }

        private static void GenerateDecoratorStub(bool enableValidation, string argumentPrefix, StreamWriter fs, string methodTabs, string validationPrefix, VkMethodSignature fn)
        {
            fs.WriteLine(methodTabs + fn.GetImplementation() + " {");
            if (enableValidation)
            {
                GenerateValidateMethodCall(fs, validationPrefix, fn);
            }

            GenerateImplMethodCall(argumentPrefix, fs, methodTabs, fn);

            fs.WriteLine(methodTabs + "}");
            fs.WriteLine("");
        }

        private static void GenerateImplMethodCall(string argumentPrefix, StreamWriter fs, string methodTabs, VkMethodSignature fn)
        {
            var returnType = fn.ReturnType == "void"
                ? ""
                : "return ";


            var validateArgs = new StringBuilder();
            validateArgs.Append(methodTabs);
            validateArgs.Append(TAB_FIELD);
            validateArgs.Append(returnType);
            validateArgs.Append("mImpl.");
            validateArgs.Append(fn.Name);
            validateArgs.Append("(");

            if (!string.IsNullOrWhiteSpace(argumentPrefix))
            {
                validateArgs.Append(argumentPrefix);
            }

            bool needComma = false;
            foreach (var param in fn.Parameters)
            {
                if (needComma)
                {
                    validateArgs.Append(", ");
                }
                else
                {
                    needComma = true;
                }

                if (param.UseOut)
                {
                    validateArgs.Append("out ");
                }
                else if (param.UseRef)
                {
                    validateArgs.Append("ref ");
                }

                validateArgs.Append(param.Name);
            }
            validateArgs.Append(");");

            fs.WriteLine(validateArgs.ToString());

        }

        private static void GenerateValidateMethodCall(StreamWriter fs, string validationPrefix, VkMethodSignature fn)
        {
            var validateArgs = new StringBuilder();
            validateArgs.Append(validationPrefix);
            validateArgs.Append(fn.Name);
            validateArgs.Append(".Validate(");

            var noOfParameters = 0;
            bool needComma = false;
            foreach (var param in fn.Parameters)
            {
                if (param.UseOut)
                {
                    continue;
                }

                if (needComma)
                {
                    validateArgs.Append(", ");
                }
                else
                {
                    needComma = true;
                }


                if (param.UseRef)
                {
                    validateArgs.Append("ref ");
                }

                validateArgs.Append(param.Name);
                ++noOfParameters;
            }
            validateArgs.Append(");");

            if (noOfParameters > 0)
                fs.WriteLine(validateArgs.ToString());
        }

        private static string CreateSubDirectory(string methodDirectoryPath, string folderName)
        {
            // 2A. DELEGATE
            var subDirectoryPath = System.IO.Path.Combine(methodDirectoryPath, folderName);
            if (!Directory.Exists(subDirectoryPath))
            {
                Directory.CreateDirectory(subDirectoryPath);
            }
            return subDirectoryPath;
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
                        interfaceFile.WriteLine(methodTabs + member.GetStructField());
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
