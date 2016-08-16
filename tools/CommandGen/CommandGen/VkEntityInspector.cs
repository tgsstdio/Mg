using System;
using System.Collections.Generic;
using System.Linq;
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

		public void Inspect(XElement root)
		{
			GenerateType(root, "handle", InspectHandle);
			GenerateType(root, "enum", InspectEnum);
			GenerateType(root, "struct", InspectStructure);
			GenerateType(root, "union", InspectStructure);
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
			"Bool32",
			"DeviceSize",
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
				csName = name.Substring(2);
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

			mHandles.Add(csName, new VkHandleInfo { name = csName, type = type });
		}

		Dictionary<string, VkStructInfo> mStructures = new Dictionary<string, VkStructInfo>();
		void InspectStructure(XElement structElement)
		{
			string name = structElement.Attribute("name").Value;
			string csName = GetTypeCsName(name, "struct");

			mTypesTranslation[name] = csName;
			mStructures[csName] = new VkStructInfo() { name = name, needsMarshalling = InspectStructureMembers(structElement) };

			IsStructBlittable(structElement, csName);
		}

		void IsStructBlittable(XElement structElement, string csName)
		{
			// check all members
			foreach (var member in structElement.Elements("member"))
			{
				string type = member.Element("type").Value;
				string memberName = member.Element("name").Value;
				string csTypeName = GetTypeCsName(type);
				string memberValue = member.Value;

				// detect if array
				if (memberValue.IndexOf('[') > 0)
				{
					return;
				}
				else if (!mBlittableTypes.Contains(csTypeName))
				{
					return;
				}
			}

			mBlittableTypes.Add(csName);
		}

		bool InspectStructureMembers(XElement structElement)
		{
			foreach (var memberElement in structElement.Elements("member"))
			{
				string member = memberElement.Value;
				var typeElement = memberElement.Element("type");
				var csMemberType = GetTypeCsName(typeElement.Value, "member");

				if (member.Contains("*") || member.Contains("[") || (mStructures.ContainsKey(csMemberType) && mStructures[csMemberType].needsMarshalling) || mHandles.ContainsKey(csMemberType))
					return true;
			}

			return false;
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

