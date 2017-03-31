#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : require
#extension ARB_explicit_uniform_location: require

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec2 inUV;
layout (location = 2) in vec3 inNormal;

layout (std140, binding = 1) uniform UBO 
{
	vec3 viewPos;
	float lodBias;
	mat4 projection;
	mat4 model;
} ubo;

varying vec2 localUV;
varying float localLodBias;
varying vec3 localNormal;
varying vec3 localViewVec;
varying vec3 localLightVec;


void vertFunc() 
{
	localUV = inUV;
	localLodBias = ubo.lodBias;
	localNormal = inNormal;

	gl_Position = ubo.projection * vec4(ubo.viewPos + inPos.xyz, 1.0);
	gl_Position.y *= -1.0f;
}
