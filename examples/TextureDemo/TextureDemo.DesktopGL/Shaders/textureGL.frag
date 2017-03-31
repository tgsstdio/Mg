#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : require
#extension GL_ARB_bindless_texture : require
#extension ARB_explicit_uniform_location: require

layout (location = 0) uniform sampler2D samplerColor;

varying vec2 localUV;
varying float localLodBias;
varying vec3 localNormal;
varying vec3 localViewVec;
varying vec3 localLightVec;

layout (location = 0) out vec4 outFragColor;

void fragFunc() 
{
	uvec2 handleCheck = uvec2(samplerColor);

	// only perform texture lookup if not null
	if (any(notEqual(uvec2(0,0), handleCheck)))
	{
		vec4 color = texture(samplerColor, localUV, localLodBias);

		vec3 N = normalize(localNormal);
		vec3 L = normalize(localLightVec);
		vec3 V = normalize(localViewVec);
		vec3 R = reflect(-L, N);
		vec3 diffuse = max(dot(N, L), 0.0) * vec3(1.0);
		float specular = pow(max(dot(R, V), 0.0), 16.0) * color.a;

		outFragColor = color;
		outFragColor.r += 0.9;
	}
	else
	{
		outFragColor = vec4(localUV, 0, 1);
	}
}