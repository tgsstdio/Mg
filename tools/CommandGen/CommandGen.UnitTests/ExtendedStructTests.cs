using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CommandGen.UnitTests
{
    [TestFixture()]
    public class ExtendedStructTests
    {
        [TestCase()]
        public void Test1()
        {
            const string xml = @"
        <extension name=""VK_KHR_multiview"" number=""54"" type=""device"" author=""KHR"" requires=""VK_KHR_get_physical_device_properties2"" contact=""Jeff Bolz @jeffbolznv"" supported=""vulkan"">
            <require>
                <enum extends=""VkStructureType""                                 name=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_PROPERTIES_KHR"" alias=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_PROPERTIES""/>
                <type name = ""VkRenderPassMultiviewCreateInfoKHR"" />
                <type name=""VkPhysicalDeviceMultiviewFeaturesKHR""/>
                <type name = ""VkPhysicalDeviceMultiviewPropertiesKHR"" />
            </require>
        </extension>";

            XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

            var result = VkEntityInspector.ExtractTypeAliases(top);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual("enum", first.GroupCategory);
            Assert.AreEqual("VkStructureType", first.GroupName);
            Assert.AreEqual("VkPhysicalDeviceMultiviewPropertiesKhr", first.UniqueKey);
            Assert.AreEqual("VkPhysicalDeviceMultiviewProperties", first.Alias);
            Assert.AreEqual("StructureTypePhysicalDeviceMultiviewProperties", first.EnumAlias);
        }

        [TestCase()]
        public void Test3()
        {
            var actual = ExtractEnumAlias("VK_STRUCTURE_TYPE_RENDER_PASS_MULTIVIEW_CREATE_INFO_KHR");
            var expected = "StructureTypePhysicalDeviceMultiviewProperties";

            Assert.AreEqual(expected, actual);
        }

        static string ExtractEnumAlias(string key)
        {
            return VkEntityInspector.ParseVkStructureTypeKey(key);
        }

        [TestCase()]
        public void Test2()
        {
            const string xml = @"
        <extension name=""VK_KHR_multiview"" number=""54"" type=""device"" author=""KHR"" requires=""VK_KHR_get_physical_device_properties2"" contact=""Jeff Bolz @jeffbolznv"" supported=""vulkan"">
            <require>
                <enum value=""1""                                                 name=""VK_KHR_MULTIVIEW_SPEC_VERSION""/>
                <enum value=""&quot;VK_KHR_multiview&quot;""                      name=""VK_KHR_MULTIVIEW_EXTENSION_NAME""/>
                <enum extends=""VkStructureType""                                 name=""VK_STRUCTURE_TYPE_RENDER_PASS_MULTIVIEW_CREATE_INFO_KHR"" alias=""VK_STRUCTURE_TYPE_RENDER_PASS_MULTIVIEW_CREATE_INFO""/>
                <enum extends=""VkStructureType""                                 name=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_FEATURES_KHR"" alias=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_FEATURES""/>
                <enum extends=""VkStructureType""                                 name=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_PROPERTIES_KHR"" alias=""VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MULTIVIEW_PROPERTIES""/>
                <enum extends=""VkDependencyFlagBits""                            name=""VK_DEPENDENCY_VIEW_LOCAL_BIT_KHR"" alias=""VK_DEPENDENCY_VIEW_LOCAL_BIT""/>
                <type name = ""VkRenderPassMultiviewCreateInfoKHR"" />
                <type name=""VkPhysicalDeviceMultiviewFeaturesKHR""/>
                <type name = ""VkPhysicalDeviceMultiviewPropertiesKHR"" />
            </require>
        </extension>";

            XElement top = XElement.Parse(xml, LoadOptions.PreserveWhitespace);

            var result = VkEntityInspector.ExtractTypeAliases(top);
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);

            {
                var first = result[0];
                Assert.AreEqual("enum", first.GroupCategory);
                Assert.AreEqual("VkStructureType", first.GroupName);
                Assert.AreEqual("VkRenderPassMultiviewCreateInfoKhr", first.UniqueKey);
                Assert.AreEqual("VkRenderPassMultiviewCreateInfo", first.Alias);
            }

            {
                var second = result[1];
                Assert.AreEqual("enum", second.GroupCategory);
                Assert.AreEqual("VkStructureType", second.GroupName);
                Assert.AreEqual("VkPhysicalDeviceMultiviewFeaturesKhr", second.UniqueKey);
                Assert.AreEqual("VkPhysicalDeviceMultiviewFeatures", second.Alias);
            }
        }
    }
}
