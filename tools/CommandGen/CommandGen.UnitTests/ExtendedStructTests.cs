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
            var inspector = new VkEntityInspector();
           
            Assert.AreEqual(0, inspector.EnumExtensions.Count);

            const string _ENUM_NAME_ = "VkStructureType";
            const string _MEMBER_NAME_ = "VK_STRUCTURE_TYPE_VIEWPORT";
            const string _MEMBER_ALIAS_ = "VK_STRUCTURE_TYPE_VIEWPORT_KHR";

            inspector.AppendEnumMemberAlias(_ENUM_NAME_, _MEMBER_NAME_, _MEMBER_ALIAS_);
            Assert.AreEqual(1, inspector.EnumExtensions.Count);
            var actual = inspector.EnumExtensions.TryGetValue(_ENUM_NAME_, out List<VkEnumExtensionInfo> found);
            Assert.IsTrue(actual);
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);

            var first = found[0];
            Assert.IsNotNull(first);


            const string _EXPECTED_ = "ViewportKhr";
            Assert.AreEqual(_EXPECTED_, first.Name);

            const string _EXPECTED_1_ = "Viewport";
            Assert.AreEqual(_EXPECTED_1_, first.Value);
        }

        [TestCase()]
        public void Test3()
        {
            var actual = ExtractEnumAlias("VK_STRUCTURE_TYPE_RENDER_PASS_MULTIVIEW_CREATE_INFO_KHR");
            var expected = "VkRenderPassMultiviewCreateInfoKhr";

            Assert.AreEqual(expected, actual);
        }

        static string ExtractEnumAlias(string key)
        {
            return VkEntityInspector.ParseVkStructureTypeKey(key);
        }
    }
}
