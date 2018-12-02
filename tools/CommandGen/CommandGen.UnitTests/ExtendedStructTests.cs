using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
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


            const string _EXPECTED_ = "VIEWPORT_KHR";
            Assert.AreEqual(_EXPECTED_, first.Key);

            const string _EXPECTED_1_ = "VIEWPORT";
            Assert.AreEqual(_EXPECTED_1_, first.Value);
        }

        [TestCase()]
        public void Test2()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_IMAGE_TYPE_2D", "VkImageType");
            var expected = "TYPE_2D";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test4()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_IMAGE_VIEW_TYPE_2D", "VkImageViewType");
            var expected = "TYPE_2D";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test5()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_SAMPLE_COUNT_1_BIT", "VkSampleCountFlagBits");
            var expected = "COUNT_1_BIT";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test3()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_STRUCTURE_TYPE_RENDER_PASS_MULTIVIEW_CREATE_INFO_KHR", "VkStructureType");
            var expected = "RENDER_PASS_MULTIVIEW_CREATE_INFO_KHR";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test6()
        {
            var actual = VkEntityInspector.SetUppercase("VkStructureType");
            Assert.AreEqual("VK_STRUCTURE_TYPE", actual);
        }

        [TestCase()]
        public void Test7()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_SUCCESS", "VkResult");
            var expected = "SUCCESS";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test8()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_BUFFER_USAGE_TRANSFER_SRC_BIT", "VkBufferUsageFlagBits");
            var expected = "TRANSFER_SRC_BIT";

            Assert.AreEqual(expected, actual);
        }

        [TestCase()]
        public void Test9()
        {
            var inspector = new VkEntityInspector();

            var actual = inspector.ExtractStandardizedEnum("VK_SAMPLE_COUNT_1_BIT", "VkSampleCountFlags");
            var expected = "COUNT_1_BIT";

            Assert.AreEqual(expected, actual);
        }
    }
}
