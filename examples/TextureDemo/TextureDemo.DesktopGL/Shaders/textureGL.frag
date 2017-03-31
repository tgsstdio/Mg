#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
#extension GL_ARB_bindless_texture : enable

layout (binding = 1) uniform sampler2D samplerColor;

varying vec2 localUV;
varying float localLodBias;
varying vec3 localNormal;
layout (location = 3) in vec3 inViewVec;
layout (location = 4) in vec3 inLightVec;

layout (location = 0) out vec4 outFragColor;

void fragFunc() 
{
	vec4 color = texture(samplerColor, localUV, localLodBias);

	vec3 N = normalize(localNormal);
	vec3 L = normalize(inLightVec);
	vec3 V = normalize(inViewVec);
	vec3 R = reflect(-L, N);
	vec3 diffuse = max(dot(N, L), 0.0) * vec3(1.0);
	float specular = pow(max(dot(R, V), 0.0), 16.0) * color.a;

	outFragColor = color;
}