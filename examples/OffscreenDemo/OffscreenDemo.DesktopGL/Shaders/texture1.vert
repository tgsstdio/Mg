#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec2 inUV;
layout (location = 2) in vec3 inNormal;

layout (std140, binding = 0) uniform UBO 
{
	vec3 viewPos;
	float lodBias;
	mat4 projection;
	mat4 model;
} ubo;

layout (location = 0) out vec2 outUV;
layout (location = 1) out float outLodBias;
layout (location = 2) out vec3 outNormal;
layout (location = 3) out vec3 outViewVec;
layout (location = 4) out vec3 outLightVec;


void vertFunc() 
{
	outUV = inUV;
	outLodBias = ubo.lodBias;

	gl_Position = ubo.projection * ubo.model * vec4(ubo.viewPos + inPos.xyz, 1.0);
	gl_Position.y *= -1.0f;
}
