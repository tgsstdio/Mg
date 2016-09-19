using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;

namespace CommandGen.UnitTests
{
	[TestFixture]
	public class InspectorTests
	{
		[TestCase]
		public void TestCase1()
		{
			string xml = @"
			<registry>
			<types>
			<type category=""struct"" name=""VkImageViewCreateInfo"">
            <member><type>VkStructureType</type>        <name>sType</name></member>                          <!-- Must be VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO -->
            <member>const <type>void</type>*            <name>pNext</name></member>                          <!-- Pointer to next structure -->
            <member optional=""true""><type>VkImageViewCreateFlags</type> <name>flags</name></member>                          <!-- Reserved -->
            <member><type>VkImage</type>                <name>image</name></member>
            <member><type>VkImageViewType</type>        <name>viewType</name></member>
            <member><type>VkFormat</type>               <name>format</name></member>
            <member><type>VkComponentMapping</type>     <name>components</name></member>
            <member><type>VkImageSubresourceRange</type> <name>subresourceRange</name></member>
            <validity>
                <usage>If pname:image was not created with ename:VK_IMAGE_CREATE_CUBE_COMPATIBLE_BIT then pname:viewType mustnot: be ename:VK_IMAGE_VIEW_TYPE_CUBE or ename:VK_IMAGE_VIEW_TYPE_CUBE_ARRAY</usage>
                <usage>If the &lt;&lt;features-features-imageCubeArray,image cubemap arrays&gt;&gt; feature is not enabled, pname:viewType mustnot: be ename:VK_IMAGE_VIEW_TYPE_CUBE_ARRAY</usage>
                <usage>If the &lt;&lt;features-features-textureCompressionETC2,ETC2 texture compression&gt;&gt; feature is not enabled, pname:format mustnot: be ename:VK_FORMAT_ETC2_R8G8B8_UNORM_BLOCK, ename:VK_FORMAT_ETC2_R8G8B8_SRGB_BLOCK, ename:VK_FORMAT_ETC2_R8G8B8A1_UNORM_BLOCK, ename:VK_FORMAT_ETC2_R8G8B8A1_SRGB_BLOCK, ename:VK_FORMAT_ETC2_R8G8B8A8_UNORM_BLOCK, ename:VK_FORMAT_ETC2_R8G8B8A8_SRGB_BLOCK, ename:VK_FORMAT_EAC_R11_UNORM_BLOCK, ename:VK_FORMAT_EAC_R11_SNORM_BLOCK, ename:VK_FORMAT_EAC_R11G11_UNORM_BLOCK, or ename:VK_FORMAT_EAC_R11G11_SNORM_BLOCK</usage>
                <usage>If the &lt;&lt;features-features-textureCompressionASTC_LDR,ASTC LDR texture compression&gt;&gt; feature is not enabled, pname:format mustnot: be ename:VK_FORMAT_ASTC_4x4_UNORM_BLOCK, ename:VK_FORMAT_ASTC_4x4_SRGB_BLOCK, ename:VK_FORMAT_ASTC_5x4_UNORM_BLOCK, ename:VK_FORMAT_ASTC_5x4_SRGB_BLOCK, ename:VK_FORMAT_ASTC_5x5_UNORM_BLOCK, ename:VK_FORMAT_ASTC_5x5_SRGB_BLOCK, ename:VK_FORMAT_ASTC_6x5_UNORM_BLOCK, ename:VK_FORMAT_ASTC_6x5_SRGB_BLOCK, ename:VK_FORMAT_ASTC_6x6_UNORM_BLOCK, ename:VK_FORMAT_ASTC_6x6_SRGB_BLOCK, ename:VK_FORMAT_ASTC_8x5_UNORM_BLOCK, ename:VK_FORMAT_ASTC_8x5_SRGB_BLOCK, ename:VK_FORMAT_ASTC_8x6_UNORM_BLOCK, ename:VK_FORMAT_ASTC_8x6_SRGB_BLOCK, ename:VK_FORMAT_ASTC_8x8_UNORM_BLOCK, ename:VK_FORMAT_ASTC_8x8_SRGB_BLOCK, ename:VK_FORMAT_ASTC_10x5_UNORM_BLOCK, ename:VK_FORMAT_ASTC_10x5_SRGB_BLOCK, ename:VK_FORMAT_ASTC_10x6_UNORM_BLOCK, ename:VK_FORMAT_ASTC_10x6_SRGB_BLOCK, ename:VK_FORMAT_ASTC_10x8_UNORM_BLOCK, ename:VK_FORMAT_ASTC_10x8_SRGB_BLOCK, ename:VK_FORMAT_ASTC_10x10_UNORM_BLOCK, ename:VK_FORMAT_ASTC_10x10_SRGB_BLOCK, ename:VK_FORMAT_ASTC_12x10_UNORM_BLOCK, ename:VK_FORMAT_ASTC_12x10_SRGB_BLOCK, ename:VK_FORMAT_ASTC_12x12_UNORM_BLOCK, or ename:VK_FORMAT_ASTC_12x12_SRGB_BLOCK</usage>
                <usage>If the &lt;&lt;features-features-textureCompressionBC,BC texture compression&gt;&gt; feature is not enabled, pname:format mustnot: be ename:VK_FORMAT_BC1_RGB_UNORM_BLOCK, ename:VK_FORMAT_BC1_RGB_SRGB_BLOCK, ename:VK_FORMAT_BC1_RGBA_UNORM_BLOCK, ename:VK_FORMAT_BC1_RGBA_SRGB_BLOCK, ename:VK_FORMAT_BC2_UNORM_BLOCK, ename:VK_FORMAT_BC2_SRGB_BLOCK, ename:VK_FORMAT_BC3_UNORM_BLOCK, ename:VK_FORMAT_BC3_SRGB_BLOCK, ename:VK_FORMAT_BC4_UNORM_BLOCK, ename:VK_FORMAT_BC4_SNORM_BLOCK, ename:VK_FORMAT_BC5_UNORM_BLOCK, ename:VK_FORMAT_BC5_SNORM_BLOCK, ename:VK_FORMAT_BC6H_UFLOAT_BLOCK, ename:VK_FORMAT_BC6H_SFLOAT_BLOCK, ename:VK_FORMAT_BC7_UNORM_BLOCK, or ename:VK_FORMAT_BC7_SRGB_BLOCK</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_LINEAR and pname:usage containing ename:VK_IMAGE_USAGE_SAMPLED_BIT, pname:format must: be supported for sampled images, as specified by the ename:VK_FORMAT_FEATURE_SAMPLED_IMAGE_BIT flag in sname:VkFormatProperties::pname:linearTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_LINEAR and pname:usage containing ename:VK_IMAGE_USAGE_STORAGE_BIT, pname:format must: be supported for storage images, as specified by the ename:VK_FORMAT_FEATURE_STORAGE_IMAGE_BIT flag in sname:VkFormatProperties::pname:linearTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_LINEAR and pname:usage containing ename:VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT, pname:format must: be supported for color attachments, as specified by the ename:VK_FORMAT_FEATURE_COLOR_ATTACHMENT_BIT flag in sname:VkFormatProperties::pname:linearTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_LINEAR and pname:usage containing ename:VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT, pname:format must: be supported for depth/stencil attachments, as specified by the ename:VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT flag in sname:VkFormatProperties::pname:linearTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_OPTIMAL and pname:usage containing ename:VK_IMAGE_USAGE_SAMPLED_BIT, pname:format must: be supported for sampled images, as specified by the ename:VK_FORMAT_FEATURE_SAMPLED_IMAGE_BIT flag in sname:VkFormatProperties::pname:optimalTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_OPTIMAL and pname:usage containing ename:VK_IMAGE_USAGE_STORAGE_BIT, pname:format must: be supported for storage images, as specified by the ename:VK_FORMAT_FEATURE_STORAGE_IMAGE_BIT flag in sname:VkFormatProperties::pname:optimalTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_OPTIMAL and pname:usage containing ename:VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT, pname:format must: be supported for color attachments, as specified by the ename:VK_FORMAT_FEATURE_COLOR_ATTACHMENT_BIT flag in sname:VkFormatProperties::pname:optimalTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>If pname:image was created with ename:VK_IMAGE_TILING_OPTIMAL and pname:usage containing ename:VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT, pname:format must: be supported for depth/stencil attachments, as specified by the ename:VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT flag in sname:VkFormatProperties::pname:optimalTilingFeatures returned by fname:vkGetPhysicalDeviceFormatProperties</usage>
                <usage>pname:subresourceRange must: be a valid subresource range for pname:image (see &lt;&lt;resources-image-views&gt;&gt;)</usage>
                <usage>If pname:image was created with the ename:VK_IMAGE_CREATE_MUTABLE_FORMAT_BIT flag, pname:format must: be compatible with the pname:format used to create pname:image, as defined in &lt;&lt;features-formats-compatibility-classes,Format Compatibility Classes&gt;&gt;</usage>
                <usage>If pname:image was not created with the ename:VK_IMAGE_CREATE_MUTABLE_FORMAT_BIT flag, pname:format must: be identical to the pname:format used to create pname:image</usage>
                <usage>pname:subResourceRange and pname:viewType must: be compatible with the image, as described in the &lt;&lt;resources-image-views-compatibility,table below&gt;&gt;</usage>
            </validity>
        	</type>
		</types>
		</registry>
		";

			XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

			var inspector = new VkEntityInspector();

			inspector.Handles.Add("VkImage", new VkHandleInfo { name = "VkImage", csType = "IntPtr" });

			inspector.Inspect(top);

			Assert.AreEqual(1, inspector.Handles.Count);
			Assert.AreEqual(1, inspector.Structs.Count);
			var info = inspector.Structs["VkImageViewCreateInfo"];
			Assert.AreEqual("VkImageViewCreateInfo", info.Name);
			var members = info.Members;
			Assert.IsNotNull(members);
			Assert.AreEqual(8, members.Count);

			{
				var member = members[0];
				Assert.AreEqual("sType", member.Name);
				Assert.AreEqual("VkStructureType", member.CsType);
			}

			{
				var member = members[1];
				Assert.AreEqual("pNext", member.Name);
				Assert.AreEqual("IntPtr", member.CsType);
			}

			{
				var member = members[2];
				Assert.AreEqual("flags", member.Name);
				Assert.AreEqual("VkImageViewCreateFlags", member.CsType);
			}

			{
				var member = members[3];
				Assert.AreEqual("image", member.Name);
				Assert.AreEqual("IntPtr", member.CsType);
			}

			{
				var member = members[4];
				Assert.AreEqual("viewType", member.Name);
				Assert.AreEqual("VkImageViewType", member.CsType);
			}

			{
				var member = members[5];
				Assert.AreEqual("format", member.Name);
				Assert.AreEqual("VkFormat", member.CsType);
			}

			{
				var member = members[6];
				Assert.AreEqual("components", member.Name);
				Assert.AreEqual("VkComponentMapping", member.CsType);
			}

			{
				var member = members[7];
				Assert.AreEqual("subresourceRange", member.Name);
				Assert.AreEqual("VkImageSubresourceRange", member.CsType);
			}
		}

		[TestCase]
		public void TestCase2()
		{
			string xml = @"
    <enums name=""VkStructureType"" type=""enum"" expand=""VK_STRUCTURE_TYPE"" comment=""Structure type enumerant"">
        <enum value=""0""     name=""VK_STRUCTURE_TYPE_APPLICATION_INFO""/>
        <enum value=""1""     name=""VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO""/>
        <enum value=""2""     name=""VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO""/>
        <enum value=""3""     name=""VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO""/>
        <enum value=""4""     name=""VK_STRUCTURE_TYPE_SUBMIT_INFO""/>
        <enum value=""5""     name=""VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO""/>
        <enum value=""6""     name=""VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE""/>
        <enum value=""7""     name=""VK_STRUCTURE_TYPE_BIND_SPARSE_INFO""/>
        <enum value=""8""     name=""VK_STRUCTURE_TYPE_FENCE_CREATE_INFO""/>
        <enum value=""9""     name=""VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO""/>
        <enum value=""10""    name=""VK_STRUCTURE_TYPE_EVENT_CREATE_INFO""/>
        <enum value=""11""    name=""VK_STRUCTURE_TYPE_QUERY_POOL_CREATE_INFO""/>
        <enum value=""12""    name=""VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO""/>
        <enum value=""13""    name=""VK_STRUCTURE_TYPE_BUFFER_VIEW_CREATE_INFO""/>
        <enum value=""14""    name=""VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO""/>
        <enum value=""15""    name=""VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO""/>
        <enum value=""16""    name=""VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO""/>
        <enum value=""17""    name=""VK_STRUCTURE_TYPE_PIPELINE_CACHE_CREATE_INFO""/>
        <enum value=""18""    name=""VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO""/>
        <enum value=""19""    name=""VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO""/>
        <enum value=""20""    name=""VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO""/>
        <enum value=""21""    name=""VK_STRUCTURE_TYPE_PIPELINE_TESSELLATION_STATE_CREATE_INFO""/>
        <enum value=""22""    name=""VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO""/>
        <enum value=""23""    name=""VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO""/>
        <enum value=""24""    name=""VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO""/>
        <enum value=""25""    name=""VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO""/>
        <enum value=""26""    name=""VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO""/>
        <enum value=""27""    name=""VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO""/>
        <enum value=""28""    name=""VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO""/>
        <enum value=""29""    name=""VK_STRUCTURE_TYPE_COMPUTE_PIPELINE_CREATE_INFO""/>
        <enum value=""30""    name=""VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO""/>
        <enum value=""31""    name=""VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO""/>
        <enum value=""32""    name=""VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO""/>
        <enum value=""33""    name=""VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO""/>
        <enum value=""34""    name=""VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO""/>
        <enum value=""35""    name=""VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET""/>
        <enum value=""36""    name=""VK_STRUCTURE_TYPE_COPY_DESCRIPTOR_SET""/>
        <enum value=""37""    name=""VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO""/>
        <enum value=""38""    name=""VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO""/>
        <enum value=""39""    name=""VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO""/>
        <enum value=""40""    name=""VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO""/>
        <enum value=""41""    name=""VK_STRUCTURE_TYPE_COMMAND_BUFFER_INHERITANCE_INFO""/>
        <enum value=""42""    name=""VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO""/>
        <enum value=""43""    name=""VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO""/>
        <enum value=""44""    name=""VK_STRUCTURE_TYPE_BUFFER_MEMORY_BARRIER""/>
        <enum value=""45""    name=""VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER""/>
        <enum value=""46""    name=""VK_STRUCTURE_TYPE_MEMORY_BARRIER""/>
        <enum value=""47""    name=""VK_STRUCTURE_TYPE_LOADER_INSTANCE_CREATE_INFO""/> <!-- Reserved for internal use by the loader, layers, and ICDs -->
        <enum value=""48""    name=""VK_STRUCTURE_TYPE_LOADER_DEVICE_CREATE_INFO""/> <!-- Reserved for internal use by the loader, layers, and ICDs -->
    </enums>";

			var inspector = new VkEntityInspector();

			var actual_0 = VkEntityInspector.ParseVkStructureTypeKey("VK_STRUCTURE_TYPE_APPLICATION_INFO");
			Assert.AreEqual("VkApplicationInfo", actual_0);

			var actual_1 = VkEntityInspector.ParseVkStructureTypeKey("VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO");
			Assert.AreEqual("VkInstanceCreateInfo", actual_1);

			var actual_2 = VkEntityInspector.ParseVkStructureTypeKey("VK_STRUCTURE_TYPE_COMMAND_BUFFER_INHERITANCE_INFO");
			Assert.AreEqual("VkCommandBufferInheritanceInfo", actual_2);
		}
	}
}

