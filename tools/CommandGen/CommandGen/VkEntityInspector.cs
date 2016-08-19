using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CommandGen
{
	public class VkEntityInspector : IVkEntityInspector
	{
		#region Parse Struct info

		readonly Dictionary<string, string> mTypesTranslation = new Dictionary<string, string>() { { "ANativeWindow", "IntPtr" }, { "HWND", "IntPtr" }, { "HINSTANCE", "IntPtr" } };

		public ISet<string> BlittableTypes
		{
			get
			{
				return mBlittableTypes;
			}
		}

		public IDictionary<string, VkHandleInfo> Handles
		{
			get
			{
				return mHandles;
			}
		}

		public IDictionary<string, string> Translations
		{
			get
			{
				return mTypesTranslation;
			}
		}

		public IDictionary<string, VkEnumInfo> Enums
		{
			get
			{
				return mEnums;
			}
		}

		public IDictionary<string, VkStructInfo> Structs
		{
			get
			{
				return mStructures;
			}
		}

		public void Inspect(XElement root)
		{
			ExtractAPIConstants(root);
			GenerateType(root, "handle", InspectHandle);
			GenerateType(root, "enum", InspectEnum);
			GenerateType(root, "struct", InspectStructure);
			GenerateType(root, "union", InspectStructure);
		}

		private Dictionary<string, string> mAPIConstants = new Dictionary<string, string>();
		void ExtractAPIConstants(XElement root)
		{
			var constants = root.Elements("enums");

			if (constants == null)
				return;
						
			var api = constants.FirstOrDefault(e => e.Attribute("name").Value == "API Constants");
			foreach (var field in constants.Elements("enum"))
			{
				var attr = field.Attribute("name");
				var define = field.Attribute("value");

				if (attr != null && define != null)
					mAPIConstants.Add(attr.Value, define.Value);
			}
		}

		readonly Dictionary<string, string> mExtensions = new Dictionary<string, string> {
			{ "EXT", "Ext" },
			{ "IMG", "Img" },
			{ "KHR", "Khr" }
		};

		readonly Dictionary<string, VkHandleInfo> mHandles = new Dictionary<string, VkHandleInfo>();

		readonly HashSet<string> mBlittableTypes = new HashSet<string>()
		{
			"Byte",
			"SByte",
			"Int32",
			"UInt32",
			"Int64",
			"UInt64",
			"float",
			"double",
			"IntPtr",
			"UIntPtr",
			"VkBool32",
			"VkDeviceSize",
		};

		// TODO: validate this mapping
		readonly Dictionary<string, string> mBasicTypesMap = new Dictionary<string, string> {
			{ "int32_t", "Int32" },
			{ "uint32_t", "UInt32" },
			{ "uint64_t", "UInt64" },
			{ "uint8_t", "Byte" },
			{ "size_t", "UIntPtr" },
			{ "xcb_connection_t", "IntPtr" },
			{ "xcb_window_t", "IntPtr" },
			{ "xcb_visualid_t", "IntPtr" },
		};

		public string GetTypeCsName(string name, string typeName = "type")
		{
			if (mTypesTranslation.ContainsKey(name))
				return mTypesTranslation[name];

			string csName;

			if (name.StartsWith("Vk", StringComparison.InvariantCulture))
				csName = name;
			else if (name.EndsWith("_t", StringComparison.InvariantCulture))
			{
				if (!mBasicTypesMap.ContainsKey(name))
					throw new NotImplementedException(string.Format("Mapping for the basic type {0} isn't supported", name));

				csName = mBasicTypesMap[name];
			}
			else
			{
				Console.WriteLine("warning: {0} name '{1}' doesn't start with Vk prefix or end with _t suffix", typeName, name);
				csName = name;
			}

			foreach (var ext in mExtensions)
				if (csName.EndsWith(ext.Key, StringComparison.InvariantCulture))
					csName = csName.Substring(0, csName.Length - ext.Value.Length) + ext.Value;

			return csName;
		}

		void GenerateType(XElement root, string type, Action<XElement> actionFn)
		{
			var elements = from el in root.Elements("types").Elements("type")
						   where (string)el.Attribute("category") == type
						   select el;

			foreach (var e in elements)
			{
				actionFn(e);
			}
		}

		void InspectHandle(XElement handleElement)
		{
			string name = handleElement.Element("name").Value;
			string csName = GetTypeCsName(name, "struct");
			string type = handleElement.Element("type").Value;

			mHandles.Add(csName, new VkHandleInfo { name = csName,
				//type = type, 
				csType = (type == "VK_DEFINE_HANDLE") ? "IntPtr" : "UInt64" });
		}

		Dictionary<string, VkStructInfo> mStructures = new Dictionary<string, VkStructInfo>();
		void InspectStructure(XElement structElement)
		{
			string name = structElement.Attribute("name").Value;
			string csName = GetTypeCsName(name, "struct");

			var returnOnly = structElement.Attribute("returnedonly");

			mTypesTranslation[name] = csName;

			var container = new VkStructInfo()
			{
				Name = name,
				//needsMarshalling = InspectStructureMembers(structElement),
				returnedonly = returnOnly != null && returnOnly.Value == "true"
			};

			mStructures[csName] = container;


			bool isStructBlittable = true;

			// check all members
			foreach (var member in structElement.Elements("member"))
			{
				//if (memberValue.IndexOf('[') > 0)
				//{
				//	isBlittable = false;
				//}
				//else if (!mBlittableTypes.Contains(csTypeName))
				//{
				//	isBlittable = false;
				//}

				var memberInfo = new VkStructMember();
				memberInfo.MemberType = member.Element("type").Value;

				var tokens = member.Value.Split(new[] {
					' ',
					'[',
					']'
				}, StringSplitOptions.RemoveEmptyEntries);
				if (tokens.Length == 2)
				{
					// usually instance
					memberInfo.BaseCppType = tokens[0];
				}
				else if (tokens.Length == 3)
				{
					// possible const pointer
					//arg.IsConst = (tokens[0] == "const");
					memberInfo.BaseCppType = tokens[1];
				}
				else if (tokens.Length == 4)
				{
					// const float array
					//arg.IsConst = (tokens[0] == "const");
					memberInfo.BaseCppType = tokens[1];
					//arg.ArrayConstant = tokens[3];
					//arg.IsFixedArray = true;
				}
				else
				{
					memberInfo.BaseCppType = memberInfo.MemberType;
				}

				memberInfo.Name = member.Element("name").Value;
				memberInfo.CsType = GetTypeCsName(memberInfo.MemberType, "struct");

				if (memberInfo.BaseCppType == "char*")
				{
					memberInfo.CsType = "string";
				}
				else if (memberInfo.BaseCppType == "void*")
				{
					memberInfo.CsType = "IntPtr";
				}
				else if (memberInfo.BaseCppType == "void**")
				{
					memberInfo.CsType = "IntPtr";
				}
				else
				{
					VkHandleInfo found;
					if (Handles.TryGetValue(memberInfo.MemberType, out found))
					{
						memberInfo.CsType = found.csType;
					}	
				}

				// detect if array
				memberInfo.IsArray = (member.Value.IndexOf('[') > 0);
				if (memberInfo.IsArray)
					memberInfo.ArrayLength = GetArrayLength(member);
				memberInfo.IsBlittable = mBlittableTypes.Contains(memberInfo.CsType) && !memberInfo.IsArray;

				if (!memberInfo.IsBlittable)
				{
					isStructBlittable = false;
				}

				if (memberInfo.IsArray && memberInfo.ArrayLength != null && memberInfo.IsBlittable)
				{
					memberInfo.Attribute = string.Format("[MarshalAs(UnmanagedType.LPArray, SizeConst = {0})]", memberInfo.ArrayLength);
				}
				else if (memberInfo.CsType == "string" && memberInfo.ArrayLength != null)
				{
					memberInfo.Attribute = string.Format("[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {0})]", memberInfo.ArrayLength);
				}
					

				container.Members.Add(memberInfo); 
			}

			if (isStructBlittable)
				mBlittableTypes.Add(csName);
		}

		string GetArrayLength(XElement member)
		{
			var enumElement = member.Element("enum");
			string found;
			if (enumElement != null && mAPIConstants.TryGetValue(enumElement.Value, out found))
				return found;

			string len = member.Value.Substring(member.Value.IndexOf('[') + 1);
			return len.Substring(0, len.IndexOf(']'));
		}


		readonly Dictionary<string, VkEnumInfo> mEnums = new Dictionary<string, VkEnumInfo>();

		void InspectEnum(XElement enumElement)
		{
			string name = enumElement.Attribute("name").Value;
			string csName = GetTypeCsName(name, "enum");

			var values = from el in enumElement.Parent.Elements("enums")
						 where (string)el.Attribute("name") == name
						 select el;

			if (values.Count() < 1)
			{
				Console.WriteLine("warning: not adding empty enum {0}", csName);
				return;
			}

			var enumsElement = values.First();

			var typeAttribute = enumsElement.Attribute("type");
			var useFlags = typeAttribute != null && typeAttribute.Value == "bitmask";
			if (useFlags)
			{
				string suffix = null;
				foreach (var ext in mExtensions)
					if (csName.EndsWith(ext.Value, StringComparison.InvariantCulture))
					{
						suffix = ext.Value + suffix;
						csName = csName.Substring(0, csName.Length - ext.Value.Length);
					}
				if (csName.EndsWith("FlagBits", StringComparison.InvariantCulture))
					csName = csName.Substring(0, csName.Length - 4) + "s";
				if (suffix != null)
					csName += suffix;
			}

			mTypesTranslation[name] = csName;
			// enums are blittable too
			mBlittableTypes.Add(csName);

			//foreach (var e in values.Elements("enum"))
			//	WriteEnumField(e, csName);
			//WriteEnumExtensions(csName);

			mEnums.Add(csName,
	          	new VkEnumInfo { 
					name = csName,
					UseFlags = useFlags,
				}
		    );
		}

		#endregion
	}
}

