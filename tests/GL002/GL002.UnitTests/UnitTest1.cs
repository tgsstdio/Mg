using NUnit.Framework;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL.DesktopGL.UnitTests
{
    public class UnitTest1
    {
        [Test]
        public void UnitTest()
        {
            var actual = FullGLCmdDrawEntrypoint.GetPrimitiveType(Magnesium.MgPrimitiveTopology.LINE_STRIP);
            Assert.AreEqual(PrimitiveType.LineStrip, actual);
        }
    }
}
