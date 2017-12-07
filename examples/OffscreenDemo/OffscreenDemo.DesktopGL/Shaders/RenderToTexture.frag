#version 450

precision highp float;

const vec3 ambient = vec3(0.1, 0.1, 0.1);
const vec3 lightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 lightColor = vec3(0.0, 1.0, 0.0);

in vec3 normal;
in vec2 texCoords;

layout(location = 0) out vec4 out_frag_color_0;
layout(location = 1) out vec4 out_frag_color_1;

void fragFunc(void)
{
    vec3 unitNormal = normalize(normal);
    float diffuse = clamp(dot(lightVecNormalized, unitNormal), 0.0, 1.0);
    out_frag_color_0 = vec4(ambient + diffuse * lightColor, 1.0);
    out_frag_color_1 = vec4(unitNormal, 1.0);
}