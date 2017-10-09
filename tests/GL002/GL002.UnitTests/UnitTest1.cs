using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UnitTest()
        {
            var actual = FullGLCmdDrawEntrypoint.GetPrimitiveType(Magnesium.MgPrimitiveTopology.LINE_STRIP);
            Assert.AreEqual(PrimitiveType.LineStrip, actual);
        }
    }
}
