using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CommandGen
{
	public class VkCommandParser
	{
		IVkEntityInspector mInspector;
		public VkCommandParser (IVkEntityInspector inspector)
		{
			mInspector = inspector;
		}

		void ParseNativeInterface (XElement top, VkCommandInfo result)
		{
			var function = new VkNativeInterface ();
			function.Name = result.Name;
			function.ReturnType = result.CsReturnType;

			int index = 0;
			foreach (var param in top.Elements ("param"))
			{
				var arg = new VkFunctionArgument {
					Index = index
				};
				var tokens = param.Value.Split (new[] {
					' ',
					'[',
					']'
				}, StringSplitOptions.RemoveEmptyEntries);
				arg.IsFixedArray = false;
				if (tokens.Length == 2)
				{
					// usually instance
					arg.ArgumentCppType = tokens [0];
					arg.IsConst = false;
					// or int
				}
				else if (tokens.Length == 3)
				{
					// possible const pointer
					arg.IsConst = (tokens [0] == "const");
					arg.ArgumentCppType = tokens [1];
				}
				else if (tokens.Length == 4)
				{
					// const float array
					arg.IsConst = (tokens [0] == "const");
					arg.ArgumentCppType = tokens [1];
					arg.ArrayConstant = tokens [3];
					arg.IsFixedArray = true;
				}
				arg.IsPointer = arg.ArgumentCppType.IndexOf ('*') >= 0;

				arg.Name = param.Element ("name").Value;

				arg.BaseCppType = param.Element ("type").Value;
				arg.BaseCsType = mInspector.GetTypeCsName (arg.BaseCppType, "type");

				XAttribute optionalAttr = param.Attribute ("optional");
				arg.IsOptional = optionalAttr != null && optionalAttr.Value == "true";
				arg.ByReference = optionalAttr != null && optionalAttr.Value == "false,true";

				VkStructInfo structFound;
				arg.IsStruct = mInspector.Structs.TryGetValue(arg.BaseCsType, out structFound);

				XAttribute lengthAttr = param.Attribute ("len");
				if (lengthAttr != null)
				{
					var lengths = lengthAttr.Value.Split (',').Where (s => s != "null-terminated").ToArray ();
					if (lengths.Length != 1)
					{
						//throw new NotImplementedException(string.Format("param.len != 1 ({0} found)", lengths.Length));
						arg.LengthVariable = null;
					}
					else
					{
						arg.LengthVariable = lengths[0];
					}
				}
				function.Arguments.Add (arg);

				arg.IsNullableIntPtr = arg.IsOptional && arg.IsPointer && !arg.IsFixedArray && arg.LengthVariable == null;

				// DETERMINE CSHARP TYPE 
				if (arg.ArgumentCppType == "char*")
				{
					arg.ArgumentCsType = "string";
				}
				else if (arg.ArgumentCppType == "void*")
				{
					arg.ArgumentCsType = "IntPtr";
				}
				else if (arg.ArgumentCppType == "void**")
				{
					arg.ArgumentCsType = "IntPtr";
				}
				else 
				{
					VkHandleInfo found;
					if (mInspector.Handles.TryGetValue(arg.BaseCsType, out found))
					{
                        arg.IsHandleArgument = true;
						arg.ArgumentCsType = found.csType;
					}
					else if (arg.IsNullableIntPtr)
					{
						// REQUIRES MARSHALLING FOR NULLS IN ALLOCATOR
						arg.ArgumentCsType = "IntPtr";
					}
					else
					{
						arg.ArgumentCsType = arg.BaseCsType;
					}
				}

				if (structFound != null && structFound.returnedonly)
					arg.UseOut = false;
				else
					arg.UseOut = !arg.IsConst && arg.IsPointer;
				
				arg.IsBlittable = mInspector.BlittableTypes.Contains(arg.ArgumentCsType);

				++index;
			}

			// USE POINTER / unsafe ONLY IF
			// ALL ARGUMENTS ARE BLITTABLE
			// && >= 1 ARGUMENTS
			// && >= 1 BLITTABLE STRUCT && NOT OPTIONAL POINTER

			bool useUnsafe = function.Arguments.Count > 0;
			uint noOfBlittableStructs = 0;
			foreach (var arg in function.Arguments)
			{
				if (!arg.IsBlittable)
				{
					useUnsafe = false;
				}

				if (arg.IsStruct && arg.IsBlittable && !arg.IsNullableIntPtr)
				{
					noOfBlittableStructs++;
				}
			}
			function.UseUnsafe = useUnsafe && noOfBlittableStructs > 0;

			// Add [In, Out] attribute to struct array variables
			if (!function.UseUnsafe)
			{
				foreach (var arg in function.Arguments)
				{
					if (arg.LengthVariable != null && arg.IsStruct && !arg.IsBlittable)
					{
						// SINGULAR MARSHALLED STRUCT ARRAY INSTANCES
						arg.Attribute = "[In, Out]";
					}
					else if (arg.IsFixedArray && !arg.IsStruct && arg.IsBlittable)
					{
						// PRETTY MUCH FOR BLENDCONSTANTS
						arg.Attribute = string.Format("[MarshalAs(UnmanagedType.LPArray, SizeConst = {0})]", arg.ArrayConstant);
					}
					else if (arg.IsStruct && !arg.IsBlittable && !arg.IsNullableIntPtr)
					{
						// SINGULAR MARSHALLED STRUCT INSTANCES
						arg.Attribute = "[In, Out]";
					}
					//else 
					//{
					//	// Add attribute
					//	VkStructInfo readOnlyStruct;
					//	if (!arg.IsOptional && mInspector.Structs.TryGetValue(arg.BaseCsType, out readOnlyStruct) && readOnlyStruct.returnedonly)
					//		arg.Attribute = "[In, Out]";
					//}
				}
			}

			result.NativeFunction = function;
		}

		void ParseMethodSignature (XElement top, VkCommandInfo result)
		{
			var signature = new VkMethodSignature ();
			signature.Name = (result.Name.StartsWith ("vk", StringComparison.InvariantCulture)) ? result.Name.Substring (2) : result.Name;
			signature.ReturnType = result.CsReturnType;

			var arguments = new Dictionary<string, VkFunctionArgument> ();
			foreach (var arg in result.NativeFunction.Arguments)
			{
				arguments.Add (arg.Name, arg);
			}

			// FILTER OUT VALUES FOR SIGNATURE
			foreach (var arg in result.NativeFunction.Arguments)
			{
				// object reference
				if (mInspector.Handles.ContainsKey(arg.BaseCsType) && ! arg.IsPointer)
				{
					if (arg.Index == 0)
					{
						result.FirstInstance = arg;
						arguments.Remove(arg.Name);
					}
				}

				VkFunctionArgument localLength;
				if (arg.LengthVariable != null && arguments.TryGetValue (arg.LengthVariable, out localLength))
				{
					result.LocalVariables.Add (localLength);
					arguments.Remove (arg.LengthVariable);
				}
			}

			signature.Parameters = (from arg in arguments.Values
			                        orderby arg.Index ascending
			                        select new VkMethodParameter{ Name = arg.Name, Source = arg }).ToList ();

			foreach (var param in signature.Parameters)
			{				
				param.BaseCsType = param.Source.BaseCsType;
				param.ArgumentCsType = param.Source.ArgumentCsType;
				param.UseOut = param.Source.UseOut;
				param.IsFixedArray = param.Source.IsFixedArray;
				param.IsArrayParameter = !param.Source.IsConst && param.Source.LengthVariable != null;
				param.IsNullableType = param.Source.IsPointer && param.Source.IsOptional;
				param.UseRef = param.Source.UseOut && param.Source.IsBlittable && !param.IsArrayParameter;
                param.IsHandleArgument = param.Source.IsHandleArgument;
                param.IsNullableIntPtr = param.Source.IsNullableIntPtr;
			}

			result.MethodSignature = signature;
		}

		void ParseFunctionCalls (XElement top, VkCommandInfo result)
		{
			//throw new NotImplementedException ();

			int noOfOuts = 0;
			int noOfArguments = result.NativeFunction.Arguments.Count;
			for (int i = 0; i < noOfArguments; ++i)
			{
				var arg = result.NativeFunction.Arguments [i];
				if (arg.UseOut)
				{
					++noOfOuts;
				}
			}

			if (result.LocalVariables.Count > 0)
			{
				foreach (var local in result.LocalVariables)
				{
					var letVar = new VkVariableDeclaration{ Source = local };
					result.Lines.Add (letVar);
				}

				var fetch = new VkFunctionCall{ Call = result.NativeFunction };
				result.Calls.Add (fetch);
				// method call
				result.Lines.Add (fetch);

				if (result.NativeFunction.ReturnType != "void")
				{
					var errorCheck = new VkReturnTypeCheck{ReturnType=result.NativeFunction.ReturnType};
					result.Lines.Add (errorCheck);
				}

				foreach (var arg in result.NativeFunction.Arguments)
				{
					var item = new VkCallArgument{Source = arg };
					item.IsNull = (arg.UseOut && arg.LengthVariable != null);
					fetch.Arguments.Add (item);
				}
			}

			foreach (var param in result.MethodSignature.Parameters)
			{
				if (param.UseOut)
				{
					var letVar = new VkVariableDeclaration{ Source = param.Source };
					result.Lines.Add (letVar);
				}
			}

			var body = new VkFunctionCall{ Call = result.NativeFunction };
			result.Calls.Add (body);
			foreach (var arg in result.NativeFunction.Arguments)
			{
				var item = new VkCallArgument{Source = arg };
				item.IsNull = (arg.UseOut && arg.LengthVariable != null);
				body.Arguments.Add (item);
			}

			result.Lines.Add (body);
		}

		public bool Parse (XElement top, out VkCommandInfo info)
		{

			var protoElem = top.Element ("proto");

			if (protoElem != null)
			{
				var result = new VkCommandInfo();
				result.Name = protoElem.Element("name").Value;
				result.CppReturnType = protoElem.Element("type").Value;
				result.CsReturnType = mInspector.GetTypeCsName(result.CppReturnType, "type");

				ParseNativeInterface(top, result);

				ParseMethodSignature(top, result);

				ParseFunctionCalls(top, result);

				info = result;
				return true;
			}
			else
			{
				info = null;
				return false;
			}
		}
	}
}

