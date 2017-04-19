using Magnesium.OpenGL;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
    [TestFixture]
    public class BlockTokenizerTest
    {
        [Test]
        public void Test0()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("CAMERA[0]");
            Assert.IsNotNull(token);
            Assert.AreEqual("CAMERA", token.Prefix);
            Assert.AreEqual(0, token.X);
            Assert.AreEqual(0, token.Y);
            Assert.AreEqual(0, token.Z);
        }

        [Test]
        public void Test1()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("UBO[0]");
            Assert.IsNotNull(token);
            Assert.AreEqual("UBO", token.Prefix);
            Assert.AreEqual(0, token.X);
            Assert.AreEqual(0, token.Y);
            Assert.AreEqual(0, token.Z);
        }

        [Test]
        public void Test2()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("UBO[1]");
            Assert.IsNotNull(token);
            Assert.AreEqual("UBO", token.Prefix);
            Assert.AreEqual(1, token.X);
            Assert.AreEqual(0, token.Y);
            Assert.AreEqual(0, token.Z);
        }

        [Test]
        public void Test3()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("UBO[1][4]");
            Assert.IsNotNull(token);
            Assert.AreEqual("UBO", token.Prefix);
            Assert.AreEqual(1, token.X);
            Assert.AreEqual(4, token.Y);
            Assert.AreEqual(0, token.Z);
        }

        [Test]
        public void Test4()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("UBO[1][4]");
            Assert.IsNotNull(token);
            Assert.AreEqual("UBO", token.Prefix);
            Assert.AreEqual(1, token.X);
            Assert.AreEqual(4, token.Y);
            Assert.AreEqual(0, token.Z);
        }

        [Test]
        public void Test5()
        {
            var tokenizer = new DefaultGLUniformBlockNameParser();
            var token = tokenizer.Parse("CAMERA[10][1][2]");
            Assert.IsNotNull(token);
            Assert.AreEqual("CAMERA", token.Prefix);
            Assert.AreEqual(10, token.X);
            Assert.AreEqual(1, token.Y);
            Assert.AreEqual(2, token.Z);
        }
    }
}
