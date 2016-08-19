using System;
using System.Collections.Generic;
using System.IO;
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
			LearnExtensions(root);
			ExtractEnumConstants(root, "API Constants", StoreAPIConstants);
			ExtractEnumConstants(root, "VkStructureType", ParseVkStructureType);
			GenerateType(root, "handle", InspectHandle);
			InspectEnums(root);
			GenerateType(root, "struct", InspectStructure);
			GenerateType(root, "union", InspectStructure);
		}

		private Dictionary<string, string> mVkStructureKeys = new Dictionary<string, string>();
		public void ParseVkStructureType(string attr, string value)
		{
			var structKey = ParseVkStructureTypeKey(attr);
			mVkStructureKeys.Add(structKey, value);
		}

		public static string ParseVkStructureTypeKey(string name)
		{
			//throw new NotImplementedException();
			var tokens = name.Replace("_STRUCTURE_TYPE_", "_").Split('_');

			var collection = new List<string>();
			foreach (var token in tokens)
			{
				if (token.Length > 1)
					collection.Add(token[0] + token.Substring(1).ToLowerInvariant());
				else
					collection.Add(token[0].ToString());
			}

			return string.Join("", collection.ToArray());
		}

		private Dictionary<string, string> mAPIConstants = new Dictionary<string, string>();
		void ExtractEnumConstants(XElement root, string enumCategory, Action<string, string> enumAction)
		{
			var constants = root.Elements("enums");

			if (constants == null)
				return;
						
			var api = constants.FirstOrDefault(e => e.Attribute("name").Value == enumCategory);
			foreach (var field in constants.Elements("enum"))
			{
				var attr = field.Attribute("name");
				var attrValue = field.Attribute("value");

				if (attr != null && attrValue != null)
					enumAction(attr.Value, attrValue.Value);
			}
		}

		void StoreAPIConstants(string attr, string value)
		{
			mAPIConstants.Add(attr, value);
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

		void InspectEnums(XElement root)
		{
			foreach (var parent in root.Elements("enums"))
			{
				string name = parent.Attribute("name").Value;

				if (!name.StartsWith("Vk", StringComparison.InvariantCulture))
				{
					Console.WriteLine("warning: not adding name doesn't start with Vk -  {0}", name);
					continue;
				}

				string csName = GetTypeCsName(name, "enum");


				var children = from el in parent.Elements("enum")
							 select el;


				if (children.Count() < 1)
				{
					Console.WriteLine("warning: not adding empty enum {0}", csName);
					continue;
				}


				var typeAttribute = parent.Attribute("type");
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

				var container = new VkEnumInfo
				{
					name = csName,
					UseFlags = useFlags,
				};

				foreach (var e in children)
				{
					container.Members.Add(WriteEnumField(e, csName));
				}

				if (enumExtensions.ContainsKey(csName))
				{
					foreach (var info in enumExtensions[csName])
					{
						container.Members.Add(WriteEnumField(info.Name, info.Value, csName));
					}
				}

				mEnums.Add(csName, container);

			}
		}

		void LearnExtensions(XElement root)
		{
			var elements = from e in root.Elements("extensions").Elements("extension") where e.Attribute("supported").Value != "disabled" select e;

			foreach (var element in elements)
				LearnExtension(element);
		}

		void LearnExtension(XElement extensionElement)
		{
			var extensions = from e in extensionElement.Element("require").Elements("enum") where e.Attribute("extends") != null select e;
			int number = Int32.Parse(extensionElement.Attribute("number").Value);
			foreach (var element in extensions)
			{
				string enumName = GetTypeCsName(element.Attribute("extends").Value, "enum");
				var info = new VkEnumExtensionInfo 
				{ 
					Name = element.Attribute("name").Value,
					Value = EnumExtensionValue(element, number, ref enumName)
				};
				if (!enumExtensions.ContainsKey(enumName))
					enumExtensions[enumName] = new List<VkEnumExtensionInfo>();

				enumExtensions[enumName].Add(info);
			}
		}

		string EnumExtensionValue(XElement element, int number, ref string csEnumName)
		{
			var offsetAttribute = element.Attribute("offset");
			if (offsetAttribute != null)
			{
				int direction = 1;
				var dirAttr = element.Attribute("dir");
				if (dirAttr != null && dirAttr.Value == "-")
					direction = -1;
				int offset = Int32.Parse(offsetAttribute.Value);

				return (direction * (1000000000 + (number - 1) * 1000 + offset)).ToString();
			}
			var valueAttribute = element.Attribute("value");
			if (valueAttribute != null)
				return valueAttribute.Value;

			var bitposAttribute = element.Attribute("bitpos");
			if (bitposAttribute != null)
			{
				if (csEnumName.EndsWith("FlagBits", StringComparison.InvariantCulture))
					csEnumName = csEnumName.Substring(0, csEnumName.Length - 4) + "s";

				return FormatFlagValue(Int32.Parse(bitposAttribute.Value));
			}

			throw new Exception(string.Format("unexpected extension enum value in: {0}", element));
		}

		Dictionary<string, List<VkEnumExtensionInfo>> enumExtensions = new Dictionary<string, List<VkEnumExtensionInfo>>();

		string FormatFlagValue(int pos)
		{
			return string.Format("0x{0:X}", 1 << pos);
		}

		VkEnumMemberInfo WriteEnumField(XElement e, string csEnumName)
		{
			var valueAttr = e.Attribute("value");
			string value;
			if (valueAttr == null)
				value = FormatFlagValue(Convert.ToInt32(e.Attribute("bitpos").Value));
			else
				value = valueAttr.Value;

			return WriteEnumField(e.Attribute("name").Value, value, csEnumName);
		}

		static Dictionary<string, string> mSpecialParts = new Dictionary<string, string> {
			{ "AMD", "Amd" },
			{ "API", "Api" },
			{ "EXT", "Ext" },
			{ "KHR", "Khr" },
			{ "1D", "1D" },
			{ "2D", "2D" },
			{ "3D", "3D" },
			{ "NV", "Nv" }
		};

		string TranslateCName(string name)
		{
			using (StringWriter sw = new StringWriter())
			{
				bool first = true;

				foreach (var part in name.Split('_'))
				{
					if (first)
					{
						first = false;
						if (name.StartsWith("VK", StringComparison.OrdinalIgnoreCase))
							continue;
						else
							Console.WriteLine("warning: name '{0}' doesn't start with VK prefix", name);
					}

					if (mSpecialParts.ContainsKey(part))
						sw.Write(mSpecialParts[part]);
					else if (part.Length > 0)
					{
						if (part.ToCharArray().All(c => char.IsUpper(c) || char.IsDigit(c)))
							sw.Write(part[0] + part.Substring(1).ToLower());
						else
							sw.Write(char.ToUpper(part[0]) + part.Substring(1));
					}
				}

				return sw.ToString();
			}
		}

		VkEnumMemberInfo WriteEnumField(string name, string value, string csEnumName)
		{
			string fName = TranslateCName(name);
			string prefix = csEnumName, suffix = null;
			bool isExtensionField = false;
			string extension = null;

			foreach (var ext in mExtensions)
			{
				if (prefix.EndsWith(ext.Value, StringComparison.InvariantCulture))
				{
					prefix = prefix.Substring(0, prefix.Length - ext.Value.Length);
					suffix = ext.Value;
				}
				else if (fName.EndsWith(ext.Value, StringComparison.InvariantCulture))
				{
					isExtensionField = true;
					extension = ext.Value;
				}
			}

			if (prefix.EndsWith("Flags", StringComparison.InvariantCulture))
			{
				prefix = prefix.Substring(0, prefix.Length - 5);
				suffix = "Bit" + suffix;
			}

			if (fName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				fName = fName.Substring(prefix.Length);

			if (!char.IsLetter(fName[0]))
			{
				switch (csEnumName)
				{
					case "ImageType":
						fName = "Image" + fName;
						break;
					case "ImageViewType":
						fName = "View" + fName;
						break;
					case "QueryResultFlags":
						fName = "Result" + fName;
						break;
					case "SampleCountFlags":
						fName = "Count" + fName;
						break;
				}
			}
			if (suffix != null)
			{
				if (fName.EndsWith(suffix, StringComparison.InvariantCulture))
					fName = fName.Substring(0, fName.Length - suffix.Length);
				else if (isExtensionField && fName.EndsWith(suffix + extension, StringComparison.InvariantCulture))
					fName = fName.Substring(0, fName.Length - suffix.Length - extension.Length) + extension;
			}

			return new VkEnumMemberInfo
			{
				Id = fName,
				Value = value,
			};

			//IndentWriteLine("{0} = {1},", fName, value);
		}

		#endregion
	}
}

