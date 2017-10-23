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

out gl_PerVertex 
{
    vec4 gl_Position;   
};

void main() 
{
	gl_Position = ubo.projection * vec4(ubo.viewPos + inPos.xyz, 1.0);
}
