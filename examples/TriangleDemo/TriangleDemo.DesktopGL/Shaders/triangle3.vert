#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inColor;

layout (binding = 0) uniform UBO 
{
	mat4 projectionMatrix;
	mat4 modelMatrix;
	mat4 viewMatrix;
} ubo[5];

layout (location = 0) out vec3 outColor;

out gl_PerVertex 
{
    vec4 gl_Position;   
};


void vertFunc() 
{
	outColor = inColor;
	gl_Position = ubo[0].projectionMatrix * ubo[0].viewMatrix * ubo[0].modelMatrix * vec4(inPos.xyz, 1.0);
	gl_Position.y *= -1.0f;
}
