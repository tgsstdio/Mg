using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace CommandGen.UnitTests
{
    [TestFixture()]
    public class ImplementationTests
    {
        public enum MethodBodyType
        {
            Enumerate,
            Destroy,
            Create,
            Custom,
        }

        public class MethodBody
        {
            public MethodBody()
            {
                Declarations = new List<MethodBodyDeclaration>();
                NullChecks = new List<MethodBodyNullCheck>();
            }

            public double Handles { get; internal set; }
            internal List<MethodBodyDeclaration> Declarations { get; set; }
            internal List<MethodBodyNullCheck> NullChecks { get; private set; }
            internal VkMethodSignature Interop { get; set; }
        }

        public MethodBodyType DetermineMethodType(VkMethodSignature sign)
        {
            if (sign.Name.StartsWith("Enumerate"))
            {
                return MethodBodyType.Enumerate;
            }
            else if (sign.Name.StartsWith("Destroy"))
            {
                return MethodBodyType.Destroy;
            }
            else if (sign.Name.StartsWith("Create"))
            {
                return MethodBodyType.Create;
            }
            else
            {
                return MethodBodyType.Custom;
            }
        }

        public string GetImplementation(VkMethodSignature m)
        {
            var builder = new StringBuilder();
            builder.Append("public");

            if (m.IsStatic)
                builder.Append(" static");

            builder.Append(" ");
            builder.Append(m.ReturnType);
            builder.Append(" ");

            builder.Append(m.Name);
            builder.Append("(");
            // foreach arg in arguments
            bool needComma = false;
            foreach (var param in m.Parameters)
            {
                if (needComma)
                {
                    builder.Append(", ");
                }
                else
                {
                    needComma = true;
                }

                if (param.UseOut)
                {
                    builder.Append("out ");
                }
                else if (param.UseRef)
                {
                    builder.Append("ref ");
                }

                builder.Append(param.BaseCsType);
                builder.Append(" ");
                builder.Append(param.Name);
            }
            builder.Append(")");

            return builder.ToString();
        }

        public static MethodBody ExtractBody(VkMethodSignature method)
        {
            var body = new MethodBody
            {

            };

            foreach(var param in method.Parameters)
            {
                var temp = new MethodBodyDeclaration
                {
                    Name = "b_" + param.Name,
                };
                body.Declarations.Add(temp);

                var check = new MethodBodyNullCheck
                {
                    Name = param.Name,
                };

                body.NullChecks.Add(check);
            }

            return body;
        }

        [TestCase]
        public void Nullable()
        {            
            var sign = new VkMethodSignature
            {
                Name = "CreateObjectTableNVX",
                Parameters = new List<VkMethodParameter>
                {
                    new VkMethodParameter
                    {
                        Name = "pCreateInfo",
                        BaseCsType = "VkObjectTableCreateInfoNVX",
                        ArgumentCsType = "MgObjectTableCreateInfoNVX",      
                        IsNullableType = true,
                    },
                    new VkMethodParameter
                    {
                        Name = "allocator",
                        BaseCsType = "VkAllocationCallbacks",
                        ArgumentCsType = "IMgAllocationCallbacks",
                        IsHandleArgument = true,  
                        IsNullableType = true,
                        UseOut = false,
                    },
                    new VkMethodParameter
                    {
                        Name = "pObjectTable",
                        BaseCsType = "VkObjectTableNVX",  
                        ArgumentCsType = "IVkObjectTableNVX",
                        IsHandleArgument = true,
                        UseOut = true,
                    }
                },
            };
            var actual = ExtractBody(sign);

            Assert.AreEqual(1, actual.NullChecks.Count);

            {
                var check = actual.NullChecks[0];
                Assert.IsNotNull(check);
                Assert.AreEqual("if (pCreateInfo == null) throw new ArgumentNullException(nameof(pCreateInfo));",
                    check.GetImplementation());
            }

            Assert.AreEqual(1, actual.Declarations.Count);

            {
                var line = actual.Declarations[0];
                Assert.IsNotNull(line);
                Assert.AreEqual("var b_allocator = CreateAllocator(allocator);", line.GetImplementation());
            }

            Assert.AreEqual(1, actual.Handles);



        }

    }

    internal class MethodBodyDeclaration
    {
        internal string Name { get; set; }
        internal string Cast { get; set; }
        internal string Argument { get; set; }

        public string GetImplementation()
        {
            var lhs = new StringBuilder();

            lhs.Append("var ");         
            lhs.Append(Name);
            lhs.Append(" = ");

            if (!string.IsNullOrWhiteSpace(Cast)) {
                lhs.Append(Cast);
                lhs.Append(" ");
            }

            lhs.Append(Argument);
            lhs.Append(";");

            return lhs.ToString();
        }
    }
}
