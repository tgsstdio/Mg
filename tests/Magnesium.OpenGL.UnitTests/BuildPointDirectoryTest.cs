using System;
using System.Collections.Generic;
using Magnesium;
using Magnesium.OpenGL;
using NUnit.Framework;
using Magnesium.OpenGL.Internals;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class BuildPointDirectoryTest
	{
		[Test()]
		public void BuildStructure0()
		{
			var mock = new MockGLDescriptorSetLayout
			{
				Uniforms = new GLUniformBinding[]
				{

				}
			};

			var createInfo = new MgPipelineLayoutCreateInfo
			{
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					mock
				},
			};
			var result = new GLNextPipelineLayout(createInfo);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.NoOfBindingPoints);
		}

		[Test()]
		public void BuildStructure1()
		{
			var mock = new MockGLDescriptorSetLayout
			{
				Uniforms = new GLUniformBinding[]
				{
					new GLUniformBinding
					{
						Binding = 0,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER,
						DescriptorCount = 10,
					}
				}
			};

			var createInfo = new MgPipelineLayoutCreateInfo
			{
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					mock
				},
			};

			var result = new GLNextPipelineLayout(createInfo);

			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.NoOfBindingPoints);
		}

		[Test()]
		public void BuildStructure2()
		{
			var mock = new MockGLDescriptorSetLayout
			{
				Uniforms = new GLUniformBinding[]
				{
					new GLUniformBinding
					{
						Binding = 0,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER,
						DescriptorCount = 1,
					},
					new GLUniformBinding
					{
						Binding = 1,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER,
						DescriptorCount = 2,
					},
				}
			};

			var createInfo = new MgPipelineLayoutCreateInfo
			{
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					mock
				},
			};

			var result = new GLNextPipelineLayout(createInfo);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.NoOfBindingPoints);
		}

		[Test()]
		public void BuildStructure3()
		{
			var mock = new MockGLDescriptorSetLayout
			{
				Uniforms = new GLUniformBinding[]
				{
					new GLUniformBinding
					{
						Binding = 0,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
						DescriptorCount = 1,
					},
					new GLUniformBinding
					{
						Binding = 1,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
						DescriptorCount = 2,
					},
				}
			};

			var createInfo = new MgPipelineLayoutCreateInfo
			{
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					mock
				},
			};

			var result = new GLNextPipelineLayout(createInfo);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.NoOfBindingPoints);
		}

		[Test()]
		public void BuildStructure4()
		{
			var mock = new MockGLDescriptorSetLayout
			{
				Uniforms = new GLUniformBinding[]
				{
					new GLUniformBinding
					{
						Binding = 0,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER_DYNAMIC,
						DescriptorCount = 1,
					},
					new GLUniformBinding
					{
						Binding = 1,
						DescriptorType = Magnesium.MgDescriptorType.UNIFORM_BUFFER,
						DescriptorCount = 2,
					},
				}
			};

			var createInfo = new MgPipelineLayoutCreateInfo
			{
				SetLayouts = new IMgDescriptorSetLayout[]
				{
					mock
				},
			};

			var result = new GLNextPipelineLayout(createInfo);

			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.NoOfBindingPoints);
			Assert.AreEqual(2, result.Ranges.Keys.Count);

			{
				var g1 = result.Ranges[0];
				Assert.AreEqual(0, g1.Binding);
				Assert.AreEqual(0, g1.First);
				Assert.AreEqual(0, g1.Last);
			}

			{
				var g1 = result.Ranges[1];
				Assert.AreEqual(1, g1.Binding);
				Assert.AreEqual(1, g1.First);
				Assert.AreEqual(2, g1.Last);
			}
		}


	}
}
